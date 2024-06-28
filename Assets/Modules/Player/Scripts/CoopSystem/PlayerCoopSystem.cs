using System.Collections;
using Characters.Player.Global;
using Modules.CommonEventBus;
using Modules.Player.Scripts.Global;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Modules.Player.Scripts.CoopSystem
{
    public class PlayerCoopSystem: MonoBehaviour
    {
        public int maxPlayerJoin=2;
        
        // [SerializeField] private GlobalGameStateSo _globalGameStateSo;
        
            
        [Header("PLayer Properties")] 
        [SerializeField] private PlayerInput playerPrefabToJoin;
        [SerializeField] private PlayerCoopEventBus playerConnectionEBus;
        [SerializeField] private Transform player1SpawnPointT;
        [SerializeField] private Transform player2SpawnPointT;
        
        [FormerlySerializedAs("_sceneManagementBridgeSo")]
        [Space(10)]
        // [Header("Event Bus")]
        // [SerializeField] private SceneManagementBusSO sceneManagementBusSo;
        
        // [Header("Listening")]
        // [SerializeField] private VoidEventChannelSO onSceneReady = default; //Raised by SceneLoader when the scene is set to active

        [Space(10)] 
        [SerializeField] private InputAction joinInputAction;
        [SerializeField] private InputAction leaveInputAction;
        
        private PlayerInputManager _playerInputManager;

        private void Awake()
        {
            PlayerGlobalState.AllPlayersCurrent = new PlayerInput[maxPlayerJoin];
            
            _playerInputManager = GetComponent<PlayerInputManager>();
            _playerInputManager.playerPrefab = playerPrefabToJoin.gameObject;
        }

        private void OnEnable()
        {
            // sceneManagementBusSo.SceneReady.OnEventRaised += SpawnPlayerOnStartOfLevel;
            playerConnectionEBus.PlayerJoinedGameEvent += OnPlayerJoinedGameSession;
            playerConnectionEBus.PlayerDeviceDisconnectedEvent += OnPlayerDeviceDisconnected;
            playerConnectionEBus.PlayerLeftGameEvent += OnPlayerLeftGame;
            
            joinInputAction.Enable();
            // leaveInputAction.Enable();
            joinInputAction.performed += OnManualJoinAction;
            leaveInputAction.performed += OnManualLeaveAction;
        }
        private void OnDisable()
        {
            // sceneManagementBusSo.SceneReady.OnEventRaised -= SpawnPlayerOnStartOfLevel;
            playerConnectionEBus.PlayerJoinedGameEvent -= OnPlayerJoinedGameSession;
            playerConnectionEBus.PlayerDeviceDisconnectedEvent -= OnPlayerDeviceDisconnected;
            playerConnectionEBus.PlayerLeftGameEvent -= OnPlayerLeftGame;

            joinInputAction.performed -= OnManualJoinAction;
            leaveInputAction.performed -= OnManualLeaveAction;
        }


        void OnManualJoinAction(InputAction.CallbackContext context)
        {
            if(_playerInputManager.playerCount >= 2)
                return;
            _playerInputManager.JoinPlayerFromActionIfNotAlreadyJoined(context);
        }
        
        void OnManualLeaveAction(InputAction.CallbackContext context)
        {
            foreach (var playerInput in PlayerGlobalState.AllPlayersCurrent)
            {
                if (playerInput != null)
                {
                    foreach (var playerInputDevice in playerInput.devices)
                    {
                        if (playerInputDevice != null && playerInputDevice == context.control.device)
                        {
                            RemovePlayer(playerInput);
                            return;
                        }
                    }
                }
            }
        }
        
        IEnumerator UnregisterPlayerOnDeviceDisconnected(PlayerInput playerInput)
        {
            //Check null
            _playerInputManager.DisableJoining();
            PlayerGlobalState.AllPlayersCurrent[playerInput.playerIndex] = null;
            yield return new WaitForSeconds(3f);
            RemovePlayer(playerInput);
            _playerInputManager.EnableJoining();
        }

        void RemovePlayer(PlayerInput playerInput)
        {
            Destroy(playerInput.gameObject);
        }
        
        
        private void SpawnPlayerOnStartOfLevel()
        {
        }

        void OnPlayerJoinedGameSession(PlayerInput playerInput)
        {
            // playerInput.GetComponent<PlayerController>().ActivatePlayer();
            
            PlayerGlobalState.AllPlayersCurrent[playerInput.playerIndex] = playerInput;

            RefreshActivePlayers();            
            
            RefreshManualJoiningInputAction();
            Debug.Log(PlayerGlobalState.activePlayers+ " <<" + " current player index joined "+playerInput.playerIndex );
                
            playerInput.gameObject.SetActive(true);
            playerInput.GetComponent<CharacterController>().enabled = false;
            
            if (playerInput.playerIndex == 0)
            {
                playerInput.gameObject.transform.position = player1SpawnPointT.position;
                DebugX.LogWithColorYellow(playerInput.transform.position + " | "+player1SpawnPointT.position);
            }
            else if (playerInput.playerIndex == 1)
                playerInput.transform.position = player2SpawnPointT.position;

            playerInput.GetComponent<CharacterController>().enabled = true;
            // playerInput.GetComponent<PlayerBase>().ActivatePlayer(playerInput);
            playerConnectionEBus.OnPlayerJoinedGameFinished(playerInput);
                        
        }

        void RefreshActivePlayers()
        {
            int count = 0;
            foreach (var player in PlayerGlobalState.AllPlayersCurrent)
            {
                if (player != null)
                    count = count + 1;
            }

            PlayerGlobalState.activePlayers = count;
        }
        void OnPlayerDeviceDisconnected(PlayerInput playerInput)
        {
            StartCoroutine(UnregisterPlayerOnDeviceDisconnected(playerInput));
        }
        
        void OnPlayerLeftGame(PlayerInput playerInput){
            PlayerGlobalState.AllPlayersCurrent[playerInput.playerIndex] = null;
            RefreshManualJoiningInputAction();
        }

        void RefreshManualJoiningInputAction()
        {
            if (_playerInputManager.playerCount < 2)
                joinInputAction.Enable();
            else
                joinInputAction.Disable();
        }
    }
}
