using System.Collections.Generic;
using Modules.CommonEventBus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Enemy
{
    public class EnemyBase: MonoBehaviour
    {
        [SerializeField] private PlayerCoopEventBus playerCoopEventBus;
        [SerializeField] private Renderer enemyRender;
        [SerializeField] private Material lockedMaterial;
        [SerializeField] private Material nonLockedMaterial;

        private Dictionary<int, int> _playerLockingMeMap = new Dictionary<int, int>();
        
        private void Awake()
        {
            InitiatePlayerLocking();
            SetNonLockedMaterial();
        }

        private void OnEnable()
        {
            playerCoopEventBus.PlayerJoinedGameEvent += OnPlayerJoinedSession;
            playerCoopEventBus.PlayerLeftGameEvent += OnPlayerLeftSession;
        }

        private void OnDisable()
        {
            playerCoopEventBus.PlayerJoinedGameEvent -= OnPlayerJoinedSession;
            playerCoopEventBus.PlayerLeftGameEvent -= OnPlayerLeftSession;
        }

        private void SetLockedRenderer() => enemyRender.material = lockedMaterial;
        private void SetNonLockedMaterial() => enemyRender.material = nonLockedMaterial;


        
        void InitiatePlayerLocking()
        {
            _playerLockingMeMap[0] = 0;
            _playerLockingMeMap[1] = 0;
        }

        void UpdateLockingRenderer()
        {
            int count = 0;
            foreach (var keyValuePair in _playerLockingMeMap)
            {
                if (keyValuePair.Value == 1)
                    count++;
            }

            if (count > 0)
            {
                SetLockedRenderer();
            }
            else
            {
                SetNonLockedMaterial();
            }
        }
        
        public void OnLockedMe(PlayerInput playerInput)
        {
            _playerLockingMeMap[playerInput.playerIndex] = 1;
            UpdateLockingRenderer();
        }

        public void OnLockRelease(PlayerInput playerInput)
        {
            _playerLockingMeMap[playerInput.playerIndex] = 0;
            UpdateLockingRenderer();
        }

        
        void OnPlayerJoinedSession(PlayerInput playerInput)
        {
            
        }

        void OnPlayerLeftSession(PlayerInput playerInput)
        {
            RemovePlayerFromLockingMe(playerInput);
        }

        void RemovePlayerFromLockingMe(PlayerInput playerInput)
        {
            //Removing player is locking me from dictionary as player left
            _playerLockingMeMap[playerInput.playerIndex] = 0;
            UpdateLockingRenderer();   
        }
    }
}