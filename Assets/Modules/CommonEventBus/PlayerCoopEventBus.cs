using Characters.Player.Global;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Modules.CommonEventBus
{
    [CreateAssetMenu(fileName = "PlayerCoopEventBus", menuName = "Gameplay/PlayerCoopEventBus")]
    public class PlayerCoopEventBus:UnityEngine.ScriptableObject
    {
        [SerializeField]private int totalPlayerJoined;

        public event UnityAction<PlayerInput>  PlayerJoinedGameEvent = delegate { };
        public event UnityAction<PlayerInput>  PlayerJoinGameSessionFinished = delegate { };
        public event UnityAction<PlayerInput>  PlayerLeftGameEvent = delegate { };

        public event UnityAction<PlayerInput>  PlayerDeviceConnectedEvent = delegate { };
        public event UnityAction<PlayerInput>  PlayerDeviceDisconnectedEvent = delegate { };
        
        public void OnPlayerDeviceConnected(PlayerInput playerInput)
        {
            DebugX.LogWithColor($"PlayerInputStateSo: Player device connected {playerInput.gameObject.name}", Color.green);
            PlayerDeviceConnectedEvent.Invoke(playerInput);
        }
        public void OnPlayerDeviceDisconnected(PlayerInput playerInput)
        {
            DebugX.LogWithColor($"PlayerInputStateSo: Player device disconnected {playerInput.gameObject.name}", Color.red);
            PlayerDeviceDisconnectedEvent.Invoke(playerInput);
        }
        public void OnPlayerJoinedGame(PlayerInput playerInput)
        {
            // DebugX.LogWithColor($"PlayerInputStateSo: Player joined game {playerInput.gameObject.name}", Color.green);
            PlayerJoinedGameEvent.Invoke(playerInput);
        }
        public void OnPlayerJoinedGameFinished(PlayerInput playerInput)
        {
            DebugX.LogWithColor($"PlayerInputStateSo: Player joined game Finished{playerInput.gameObject.name}", Color.green);
            PlayerJoinGameSessionFinished.Invoke(playerInput);
        }
        public void OnPlayerLeftGame(PlayerInput playerInput)
        {
            DebugX.LogWithColor($"PlayerInputStateSo: Player left game {playerInput.gameObject.name}", Color.red);
            PlayerLeftGameEvent.Invoke(playerInput);
        }


        public void OnControlsChanged(PlayerInput playerInput)
        {
            DebugX.LogWithColor($"PlayerInputStateSo: Controls changed {playerInput.gameObject.name}", Color.cyan);
        }
        
        
        public void OnEscapeButton()
        {
            DebugX.LogWithColor("PlayerInputStateSo: ESCAPE button", Color.cyan);
        }
        
        // public void 
    }
}
