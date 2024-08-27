using Modules.Common.Abilities.Base.model;
using Modules.CommonEventBus;
using Modules.Level;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.UI.Scripts.Slot
{
    public class UIPlayerHUDManager: MonoBehaviour
    {
        [SerializeField] private UIPlayerHUDSlot _playerHudSlot1;
        [SerializeField] private UIPlayerHUDSlot _playerHudSlot2;
        [SerializeField] private Transform bossDefeatedUI;
        private void OnEnable()
        {
            PlayerInventoryEventBus.PlayerReadyToShowUI += OnPlayerReadyToShowUI;
            LevelEvents.LevelDefeated += EnableBossDefeatedUI;
        }

        private void OnDisable()
        {
            PlayerInventoryEventBus.PlayerReadyToShowUI -= OnPlayerReadyToShowUI;
            LevelEvents.LevelDefeated -= EnableBossDefeatedUI;

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


        void EnableBossDefeatedUI(AbilityTriggeredInputType a)
        {
            bossDefeatedUI.gameObject.SetActive(true);
        }
    }
}