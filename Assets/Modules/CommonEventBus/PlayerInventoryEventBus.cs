using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Slot;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Modules.CommonEventBus
{
    public static class PlayerInventoryEventBus
    {
        // public static UnityAction<PlayerInput> InventoryInitializationComplete = delegate { };
        
        public static UnityAction<PlayerInput, ItemRack> OnItemRackInitialized = delegate { };
        
        public static UnityAction<ItemBase>  ShowItemPickupInfo = delegate { };
        // public static UnityAction<PlayerInput, ItemBase>  HideItemPickupInfo = delegate { };
        
        
        public static UnityAction<PlayerInput>  PlayerReadyToShowUI = delegate { };
    }
}