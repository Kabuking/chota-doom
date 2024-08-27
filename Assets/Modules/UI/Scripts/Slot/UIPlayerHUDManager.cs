using Modules.CommonEventBus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.UI.Scripts.Slot
{
    public class UIPlayerHUDManager: MonoBehaviour
    {
        [SerializeField] private UIPlayerHUDSlot _playerHudSlot1;
        [SerializeField] private UIPlayerHUDSlot _playerHudSlot2;
        private void OnEnable()
        {
            PlayerInventoryEventBus.PlayerReadyToShowUI += OnPlayerReadyToShowUI;
        }

        private void OnDisable()
        {
            PlayerInventoryEventBus.PlayerReadyToShowUI -= OnPlayerReadyToShowUI;
        }

        void OnPlayerReadyToShowUI(PlayerInput playerInput)
        {
            switch (playerInput.playerIndex)
            {
                case 0:
                    _playerHudSlot1.gameObject.SetActive(true);
                    _playerHudSlot1.Init(playerInput);
                    break;
                case 1:
                    _playerHudSlot2.gameObject.SetActive(true);
                    _playerHudSlot2.Init(playerInput);
                    break;
            }

        }

        void OnPlayerDead()
        {
            _playerHudSlot1.gameObject.SetActive(false);
        }
    }
}