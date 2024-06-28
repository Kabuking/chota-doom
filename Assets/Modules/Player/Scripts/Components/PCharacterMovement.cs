using Modules.Player.Scripts.ComponentEventBus;
using Modules.Player.Scripts.InputSystem;
using Modules.Player.Scripts.PlayerData;
using UnityEngine;

namespace Modules.Player.Scripts.Components
{
    public class PCharacterMovement: MonoBehaviour
    {
        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private bool orientRotationToMovement = false;
        [SerializeField] private float rotationRate = 1.5f;
        [SerializeField] private bool limitMovementRotation = false;
        [SerializeField] private Vector2 limitRotationAngle = new Vector2(-0.7f, 0.7f);
        
        [SerializeField] private float lookUPDown = 1;
        
        [SerializeField] private PlayerComponentEventBus playerComponentEventBus;
        private CharacterController _characterController;
        private PlayerInputMapping _playerInputMapping;

        [SerializeField] private Transform _foundEnemyTarget;

        public EvadeType evadeType = EvadeType.FollowMovementDirection;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerInputMapping = GetComponent<PlayerInputMapping>();
        }

        private void OnEnable()
        {
            // playerComponentEventBus.OnFoundEnemy += OnFoundEnemyCharacter;
        }

        public void ApplyXZVelocity(float speed)
        {
            _characterController.SimpleMove(new Vector3(
                _playerInputMapping.incomingMovementVector.x,
                0,
                _playerInputMapping.incomingMovementVector.y) * speed);

            if (orientRotationToMovement)
            {
                ApplyLookForwardMovement();
            }
        }
        
        public void ApplyXZVelocity(float speed, Vector2 direction)
        {
            _characterController.SimpleMove(new Vector3(direction.x, 0, direction.y) * speed);

            if (orientRotationToMovement)
            {
                ApplyLookForwardMovement();
            }
        }

        /*
         * TODO: Confirmed, where to keep this function
         */
        public void ApplyLookTowardEnemy()
        {
            if (_foundEnemyTarget)
            {
                // DebugX.LogWithColorYellow("Applying look at "+_foundEnemyTarget.position + " player fwd"+transform.forward);
                transform.LookAt(new Vector3(_foundEnemyTarget.position.x, lookUPDown, _foundEnemyTarget.position.z));
            }
            else
            {
                // DebugX.LogWithColorYellow("Not Applying look at");
            }
        }
        
        private Vector3 _destination;
        public void ApplyLookForwardMovement()
        {
            if (limitMovementRotation)
            {
                //make y > 0
                
                // make x clamp
                float xclamped = Mathf.Clamp(_playerInputMapping.incomingMovementVector.x, limitRotationAngle.x, limitRotationAngle.y);
                _destination = transform.position + new Vector3(
                    xclamped,
                    0,
                    1) * rotationRate;
            }
            else
            {
                // DebugX.LogWithColorCyan("Applying morient to look at");
                _destination = transform.position + new Vector3(
                    _playerInputMapping.incomingMovementVector.x,
                    0,
                    _playerInputMapping.incomingMovementVector.y) * rotationRate;
            }

            transform.LookAt(_destination);
        }

        public Transform testDummyObject;
        public void ResetLookForward()
        {
            if (orientRotationToMovement)
            {
                ApplyLookForwardMovement();
            }
            else
            {
                SetEnemyTargetLock(null);
                transform.rotation = Quaternion.identity;
                // DebugX.LogWithColorCyan("Reset look at "+transform.rotation );
            }
        }

        public void StopVelocityXYZ() => _characterController.SimpleMove(Vector3.zero);
        public void SetLimitOrientToRotation(bool limitOrientationMovement) => limitMovementRotation = limitOrientationMovement;
        public void SetOrientToRotation(bool orientB) => orientRotationToMovement = orientB;
        public void SetRotationRate(float rotationRateF) => rotationRate = rotationRateF;
        public void SetEnemyTargetLock(Transform enemyT) => _foundEnemyTarget = enemyT;
        public void ApplyEvade()=> ApplyXZVelocity(_playerStats.evadeVelocity, GetEvadeDirection());

        public enum EvadeType
        {
            FollowMovementDirection,
            ReverseForward,
            IgnoreForward
        }
        
        public Vector2 GetEvadeDirection()
        {
            float yInput = 0;
            switch (evadeType)
            {
                case EvadeType.IgnoreForward:
                    if (_playerInputMapping.lastIncomingMovementVector.y > 0)
                        yInput = 0;
                    break;
                case EvadeType.ReverseForward:
                    if (_playerInputMapping.lastIncomingMovementVector.y > 0)
                        yInput = _playerInputMapping.lastIncomingMovementVector.y * -1;
                    break;
                case EvadeType.FollowMovementDirection:
                    yInput = _playerInputMapping.lastIncomingMovementVector.y;
                    break;
            }

            return new Vector2(_playerInputMapping.lastIncomingMovementVector.x, yInput);
        }
    }
}