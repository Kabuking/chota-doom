using Levels.L_Testing.Scripts;
using Modules.Enemy;
using UnityEngine;

namespace Modules.Player.Scripts.Components.TargetAssist
{
    public class AutoLookAtEnemyInRange: MonoBehaviour
    {
        [SerializeField] private float maximumLookAtRange = 10;
        [SerializeField] private bool resetLookAtToForward = true;
        
        [Header("Debug")]
        [SerializeField] private EnemyBase nearestEnemyTransform;
        private PCharacterMovement _characterMovement;
        private void Awake()
        {
            _characterMovement = transform.root.GetComponent<PCharacterMovement>();
        }

        private void OnEnable()
        {
            LevelEventBridge.NoEnemiesLeftInGame += OnAllEnemiesGone;

        }

        private void OnDisable()
        {
            LevelEventBridge.NoEnemiesLeftInGame -= OnAllEnemiesGone;
        }

        void OnAllEnemiesGone()
        {
            ResetLookAtToForward();
        }

        void ResetLookAtToForward()
        {
            _characterMovement.ResetLookForward();
        }

        private void Update()
        {
            //TODO: Possibility of optimization
            UpdateLookAtEnemyTarget();
        }

        private Transform _nearestEnemyTemp;
        void ScanNearestEnemy()
        {
            if(EnemyManager.instance != null)
            {
                nearestEnemyTransform = EnemyManager.instance.GetNearestEnemy(transform.position, maximumLookAtRange);
            }
        }

        void UpdateLookAtEnemyTarget()
        {
            ScanNearestEnemy();
            if (nearestEnemyTransform != null)
            {
                var d = Vector3.Distance(transform.position, nearestEnemyTransform.transform.position);
                // DebugX.LogWithColorCyan(" d " + d + " --");

                if (d <= maximumLookAtRange)
                {
                    _characterMovement.SetEnemyTargetLock(nearestEnemyTransform.transform);
                }
            }
            else
            {
                if (resetLookAtToForward)
                {
                    // DebugX.LogWithColorCyan("Out of look at range again");
                    ResetLookAtToForward();
                }
            }
        }
    }
}