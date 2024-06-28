using Modules.CommonEventBus;
using Modules.Loadout.Scripts.Slot;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.UI.Scripts.Slot
{
    /// <summary>
    /// Script which created the LoadoutUI for player
    /// </summary>
    public class UILoadOutManager: MonoBehaviour
    {
        [Header("Required")] 
        [SerializeField] private PlayerCoopEventBus playerCoopEventBus;
        [SerializeField] private ItemRackUI  itemRackUILoadOutTemplate;
        
        [Header("Debugging only/ Do not add manually")]
        [SerializeField] private ItemRackUI  itemRackUILoadOutForPlayer1;
        [SerializeField] private ItemRackUI  itemRackUILoadOutForPlayer2;

        
        private void OnEnable()
        {
            
            PlayerInventoryEventBus.OnItemRackInitialized += OnPlayerItemRackInitialized;
            playerCoopEventBus.PlayerLeftGameEvent += OnPlayerLeftGameSession;
        }

        private void OnDisable()
        {
            PlayerInventoryEventBus.OnItemRackInitialized  -= OnPlayerItemRackInitialized;
            playerCoopEventBus.PlayerLeftGameEvent -= OnPlayerLeftGameSession;
        }

        void OnPlayerItemRackInitialized(PlayerInput playerInput, ItemRack itemRack)
        {
            if (playerInput.playerIndex == 0)
            {
                itemRackUILoadOutForPlayer1 = Instantiate(itemRackUILoadOutTemplate, transform);
                itemRackUILoadOutForPlayer1.InitiateLoadOut(playerInput, itemRack);
            }
            else
            {
                itemRackUILoadOutForPlayer2 = Instantiate(itemRackUILoadOutTemplate, transform);
                itemRackUILoadOutForPlayer2.InitiateLoadOut(playerInput, itemRack);
            }
        }

        void OnPlayerLeftGameSession(PlayerInput playerInput)
        {
            if (playerInput.playerIndex == 0)
            {
                itemRackUILoadOutForPlayer1.OnPlayerLeftGameSession(playerInput);
                Destroy(itemRackUILoadOutForPlayer1);
            }
            else
            {
                itemRackUILoadOutForPlayer2.OnPlayerLeftGameSession(playerInput);
                Destroy(itemRackUILoadOutForPlayer2);
            }
        }
    }
    
    /*
     * Checks
     * Player coop join leave all condition
     * Player Dead , Respawn
     */
}

