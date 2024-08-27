using System.Collections.Generic;
using System.Linq;
using Characters.Player.Global;
using Levels.L_Testing.Scripts;
using Modules.CommonEventBus;
using Modules.Enemy;
using Modules.Player.Scripts.ComponentEventBus;
using Modules.Player.Scripts.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Player.Scripts.Components.TargetAssist
{
    public class ManualSwitchAtEnemyInRange : MonoBehaviour
    {
        public enum TargetSwitchTypeForButtons
        {
            CurrentLockedToNextTargetNearest,
            SecondNearestTarget,
            RightStickFlick,

            // AccordingToScriptSetup,
            LeftRightSwitch
        }

        public enum SortingPriorityOnLeftRightSwitch
        {
            XYZ,
            XZY,
            YZX,
            YXZ,
            ZXY,
            ZYX
        }

        [SerializeField] private PlayerInput _playerInput;

        // [SerializeField] private TargetSwitchTypeForButtons targetSwitchTypeForButtons;
        [SerializeField] private float maximumLookAtRange = 10;
        [SerializeField] private PlayerComponentEventBus _playerComponentEventBus;
        [SerializeField] private List<EnemyBase> enemiesInRange = new List<EnemyBase>();
        private PlayerInputMapping _playerInputMapping;


        [Header("Debug")] [SerializeField] private EnemyBase currentTargetEnemy;
        public EnemyBase GetCurrentEnemyTarget => currentTargetEnemy;
        
        private PCharacterMovement _characterMovement;

        // [SerializeField] private bool SwitchTargetBool = false;

        [SerializeField] private EnemyBase previousTargetLockedEnemy;

        private void Awake()
        {
            _playerInput = transform.root.GetComponent<PlayerInput>();
            _playerInputMapping = transform.root.GetComponent<PlayerInputMapping>();
            _playerComponentEventBus = transform.root.GetComponent<PlayerComponentEventBus>();
            _characterMovement = transform.root.GetComponent<PCharacterMovement>();
        }

        private void OnEnable()
        {
            LevelEventBridge.NoEnemiesLeftInGame += OnAllEnemiesGone;
            LevelEventBridge.OnEnemyDestroyed += OnEnemyRemoved;
            _playerComponentEventBus.DoPerformTargetSwitchOnButtonPressed += TargetSwitch;
        }

        private void OnDisable()
        {
            LevelEventBridge.NoEnemiesLeftInGame -= OnAllEnemiesGone;
            LevelEventBridge.OnEnemyDestroyed -= OnEnemyRemoved;
            _playerComponentEventBus.DoPerformTargetSwitchOnButtonPressed -= TargetSwitch;
        }

        void OnAllEnemiesGone() => ResetLookAtToForward();
        void ResetLookAtToForward() => _characterMovement.ResetLookForward();

        private void Start()
        {
            _allEnemiesB = new List<EnemyBase>(EnemyManager.instance.GetAllEnemies());

            if (_allEnemiesB.Count > 0)
            {
                SetCurrentTargetLockedEnemy(GetNearestEnemy());
                if (currentTargetEnemy != null)
                {
                    // LockOnTarget(currentTargetEnemy);
                    SetCurrentTargetLockedEnemy(currentTargetEnemy);
                }
            }
        }

        /// <summary>
        /// UPDATE FUNCTION
        /// </summary>
        private void Update()
        {
            ScanForEnemies();
            LookAtCurrentTarget();

            /*if (targetSwitchTypeForButtons == TargetSwitchTypeForButtons.LeftRightSwitch)
            {
                ScanForEnemies();
                LookAtCurrentTarget();

            }
            else
            {
                ScanForEnemies();
                LookAtCurrentTarget();
            }*/
        }


        /// <summary>
        /// SCAN FOR ENEMIES every frame
        /// Add whoever is in range, sort them according to there distance
        /// TODO below
        /// </summary>
        private List<EnemyBase> _allEnemiesB;

        private void ScanForEnemies()
        {
            enemiesInRange.Clear();
            _allEnemiesB = new List<EnemyBase>(EnemyManager.instance.GetAllEnemies());

            //TODO iteration wont work if some element disappeared or out of list
            foreach (EnemyBase enemy in _allEnemiesB)
            {
                if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <=
                    maximumLookAtRange)
                {
                    enemiesInRange.Add(enemy);
                }
            }

            // Sort enemies by distance to the player
            enemiesInRange.Sort((enemy1, enemy2) =>
                Vector3.Distance(transform.position, enemy1.transform.position).CompareTo(
                    Vector3.Distance(transform.position, enemy2.transform.position))
            );

            if (currentTargetEnemy == null && enemiesInRange.Count > 0) { AutoTargetNearestEnemy(); }
            else if (enemiesInRange.Count <= 0) { SetCurrentTargetLockedEnemy(null); }
        }

        private void AutoTargetNearestEnemy() { SetCurrentTargetLockedEnemy(enemiesInRange[0]); }

        /// <summary>
        /// Sets the target lock
        /// </summary>
        /// <param name="newTargetLocked"></param>
        void SetCurrentTargetLockedEnemy(EnemyBase newTargetLocked)
        {
            // DebugX.LogWithColorCyan("Locked target " + newTargetLocked);
            if (currentTargetEnemy != null)
            {
                previousTargetLockedEnemy =
                    currentTargetEnemy; //TODO: THis may still store null references if Enemy is destroyed
                if (previousTargetLockedEnemy != null)
                    previousTargetLockedEnemy.OnLockRelease(_playerInput);
            }

            currentTargetEnemy = newTargetLocked;
            
            // PlayerInventoryEventBus.ShowItemPickupInfo
            if (currentTargetEnemy != null)
                currentTargetEnemy.OnLockedMe(_playerInput);
        }


        /// <summary>
        /// Makes the character/Player look at the locked target.
        /// Setting Look at Locked Target
        /// Called at Update
        /// </summary>
        private void LookAtCurrentTarget()
        {
            if (currentTargetEnemy != null)
            {
                _characterMovement.SetEnemyTargetLock(currentTargetEnemy.transform);
                // Vector3 targetDirection = currentTargetEnemy.transform.position - transform.position;
                // transform.rotation = Quaternion.LookRotation(targetDirection);
            }
            else
            {
                // Reset rotation to zero (straight ahead)
                //TODO : orient to movement ignore
                // transform.rotation = Quaternion.identity;
                // _characterMovement.SetEnemyTargetLock(null);
                ResetLookAtToForward();
            }
        }

        /// <summary>
        /// Target Switch Sequentially
        /// Not using this 
        /// </summary>
        private void SwitchTargetSequentiallyNearest()
        {
            // DebugX.LogWithColorCyan("Player: Target Switch triggered sequentially");
            // TODO NULL Check currentTargetEnemy
            if (enemiesInRange.Count > 1)
            {
                int currentIndex = enemiesInRange.IndexOf(currentTargetEnemy);
                int nextIndex = (currentIndex + 1) % enemiesInRange.Count;
                SetCurrentTargetLockedEnemy(enemiesInRange[nextIndex]);
            }
        }

        /// <summary>
        /// SWITCH To SECOND-NEAREST
        /// Currently using
        /// </summary>
        private void SwitchSecondNearestTarget()
        {
            // TODO NULL Check currentTargetEnemy

            DebugX.LogWithColorYellow("Player: Second target switcfh");
            if (enemiesInRange.Count > 1)
            {
                int nearestIndex = enemiesInRange.IndexOf(currentTargetEnemy);
                if (nearestIndex == 0 && enemiesInRange.Count > 1)
                {
                    SetCurrentTargetLockedEnemy(enemiesInRange[1]);
                }
                else if (nearestIndex > 0)
                {
                    SetCurrentTargetLockedEnemy(enemiesInRange[0]);
                }
            }
        }

        /// <summary>
        /// MAIN FUNCTION
        /// Gets Triggered whenever Player does Target Switch button press
        /// According to Script Setup : The default target switch behaviour set in the script,
        /// it should be other then AccordingToScriptSetup
        /// </summary>
        /// <param name="incomingTargetSwitchTypeForButtons"></param>
        public void TargetSwitch(TargetSwitchTypeForButtons incomingTargetSwitchTypeForButtons)
        {
            // DebugX.LogWithColorYellow("Target switch triggered "+incomingTargetSwitchTypeForButtons);
            switch (incomingTargetSwitchTypeForButtons)
            {
                case TargetSwitchTypeForButtons.RightStickFlick:
                    SwitchToTargetRightStickFlick();
                    break;
                case TargetSwitchTypeForButtons.LeftRightSwitch:
                    SwitchTargetLR(_playerInputMapping.lastTargetSwitchDirectionLR);
                    break;
                case TargetSwitchTypeForButtons.CurrentLockedToNextTargetNearest:
                    SwitchTargetSequentiallyNearest();
                    break;
                case TargetSwitchTypeForButtons.SecondNearestTarget:
                    SwitchSecondNearestTarget();
                    break;
                default:
                    // DebugX.LogWithColor("Unkown Target switch triggered "+incomingTargetSwitchType, Color.red);
                    break;
            }
        }

        //Enemy on removed from game session
        private void OnEnemyRemoved(EnemyBase enemy)
        {
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }

            if (currentTargetEnemy == enemy)
            {
                SetCurrentTargetLockedEnemy(null);
            }
        }

        /// <summary>
        /// Right Stick Flick Target Switch
        /// Current Implement partial - No in priority
        /// </summary>
        private void SwitchToTargetRightStickFlick()
        {
            // DebugX.LogWithColorYellow("Target switch using RS ");
            EnemyBase bestTarget = null;
            float bestScore = float.MaxValue;

            foreach (EnemyBase enemy in enemiesInRange)
            {
                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                float angleToEnemy = Vector3.Angle(_playerInputMapping.lastRsFlickDirection, directionToEnemy);

                // Combine distance and angle into a single score
                float score = distanceToEnemy + angleToEnemy / maximumLookAtRange; // Normalized angle

                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = enemy;
                }
            }

            if (bestTarget != null)
            {
                SetCurrentTargetLockedEnemy(bestTarget);
                // DebugX.LogWithColorCyan("RS Flick switch "+currentTargetEnemy.name+ " "+_playerInputMapping.currentRsFlickDirection);
            }
        }

        EnemyBase GetNearestEnemy()
        {
            EnemyBase nearestEnemy = null;
            float nearestDistance = Mathf.Infinity;

            foreach (EnemyBase enemy in _allEnemiesB)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            return nearestEnemy;
        }

        /// <summary>
        /// Left Right Target Switch
        /// </summary>
        /// <param name="direction"></param>
        private void SwitchTargetLR(float direction)
        {
            // DebugX.LogWithColorCyan("LR Target switch type " + direction);
            
            //For first time
            if (currentTargetEnemy == null)
            {
                currentTargetEnemy = GetNearestEnemy();
                if (currentTargetEnemy != null)
                {
                    // LockOnTarget(currentTargetEnemy);
                    SetCurrentTargetLockedEnemy(currentTargetEnemy);
                }

                return;
            }

            // Sort enemies based on the relative X position first and then Y position
            /*List<EnemyBase> sortedEnemies = enemiesInRange
                .Where(enemy => enemy != currentTargetEnemy)
                .OrderBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.x - enemy.transform.position.x))
                .ThenBy(enemy => enemy.transform.position.y)
                .ToList();*/

            List<EnemyBase> sortedEnemies = SortEnemies(enemiesInRange);
            foreach (EnemyBase enemy in sortedEnemies)
            {
                if (IsTargetInDirection(currentTargetEnemy.transform, enemy.transform, direction))
                {
                    // Debug.Log("Found next target: " + enemy.name);
                    // currentTargetEnemy.RevertMaterial(); // Revert the material of the previous target
                    SetCurrentTargetLockedEnemy(enemy);
                    // LockOnTarget(currentTargetEnemy);
                    return;
                }
                else
                {
                    // Debug.Log("No more target found in that direction " + enemy.name);

                }
            }
        }

        private float GetRelativeAngle(Transform from, Transform to)
        {
            Vector3 directionToTarget = to.position - from.position;
            float angle = Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;
            return angle;
        }

        private bool IsTargetInDirection(Transform current, Transform target, float direction)
        {
            Vector3 toTarget = target.position - current.position;
            Vector3 currentForward = current.forward;

            // Calculate the angle between the current target's forward direction and the target
            float angle = Vector3.SignedAngle(currentForward, toTarget, Vector3.up);
            // Debug.Log("Angle: " + angle);
            if (direction < 0)
            {
                // For left direction, we want angles between -180 and -5 degrees
                return angle < -5f;
            }
            else
            {
                // For right direction, we want angles between 5 and 180 degrees
                return angle > 5f;
            }
        }

        /// <summary>
        /// This function will sort the enemies based on the given priority set XYZ
        /// Common is XYZ => First horizontal, then height , then by distant to the player
        /// </summary>
        /// <param name="enemies"></param>
        /// <returns></returns>
        private List<EnemyBase> SortEnemies(List<EnemyBase> enemies)
        {
            switch (_playerInputMapping.sortingPriorityLeftRightSwitch)
            {
                case SortingPriorityOnLeftRightSwitch.XYZ:
                    return enemies.Where(enemy => enemy != currentTargetEnemy)
                        .OrderBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.x - enemy.transform.position.x))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.y - enemy.transform.position.y))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.z - enemy.transform.position.z))
                        .ToList();

                case SortingPriorityOnLeftRightSwitch.XZY:
                    return enemies.Where(enemy => enemy != currentTargetEnemy)
                        .OrderBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.x - enemy.transform.position.x))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.z - enemy.transform.position.z))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.y - enemy.transform.position.y))
                        .ToList();

                case SortingPriorityOnLeftRightSwitch.YZX:
                    return enemies.Where(enemy => enemy != currentTargetEnemy)
                        .OrderBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.y - enemy.transform.position.y))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.z - enemy.transform.position.z))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.x - enemy.transform.position.x))
                        .ToList();

                case SortingPriorityOnLeftRightSwitch.YXZ:
                    return enemies.Where(enemy => enemy != currentTargetEnemy)
                        .OrderBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.y - enemy.transform.position.y))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.x - enemy.transform.position.x))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.z - enemy.transform.position.z))
                        .ToList();

                case SortingPriorityOnLeftRightSwitch.ZXY:
                    return enemies.Where(enemy => enemy != currentTargetEnemy)
                        .OrderBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.z - enemy.transform.position.z))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.x - enemy.transform.position.x))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.y - enemy.transform.position.y))
                        .ToList();

                case SortingPriorityOnLeftRightSwitch.ZYX:
                    return enemies.Where(enemy => enemy != currentTargetEnemy)
                        .OrderBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.z - enemy.transform.position.z))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.y - enemy.transform.position.y))
                        .ThenBy(enemy => Mathf.Abs(currentTargetEnemy.transform.position.x - enemy.transform.position.x))
                        .ToList();

                default:
                    return enemies; // Default case, should not occur
            }
        }
    }
}