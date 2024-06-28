using System.Collections.Generic;
using Modules.Loadout.Scripts.Item;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Modules.Loadout.Scripts.Manager
{
    public abstract class GameplayLoadout: MonoBehaviour
    {
        //Getters
        // public abstract ItemBase Get_ActiveItemHolding();
        // public abstract List<ItemBase> Get_LoadOutItemsCarrying();
        public abstract Transform Get_ItemSocketTransform();
        public abstract PlayerInput Get_OwnerPlayerInput();
        /*public abstract MonoBehaviour Get_OwningLoadOutManager();

        public abstract bool AvailableItemSlots();
        public abstract int GetActiveItemHoldingSlotIndex();

        //Setters
        public abstract ItemBase UpdateActiveItemHolding(ItemBase newItemHolding);
        public abstract ItemBase AddItemInLoadOutCarrying(ItemBase newItemHolding);
        public abstract ItemBase RemoveItemInLoadOutCarrying(ItemBase itemToRemove);
        public abstract ItemBase SwapActiveItemHolding(ItemBase itemToSwapWith);
        */

        public abstract List<ItemBase> GetStarterItemList();
        //TODO
        //public List<Ammonition> GetAmmonitions();


        //Events 
        // public UnityAction<PlayerInput, ItemBase> LoadoutPickedItem = delegate { };
        /*public UnityAction<PlayerInput, ItemBase, ItemBase> Event_OnItemSwapped = delegate { };
        public UnityAction<PlayerInput, ItemBase> Event_OnItemEquipped = delegate { };
        public UnityAction<PlayerInput, ItemBase> Event_OnItemUnEquipped = delegate { };
        public UnityAction<PlayerInput, ItemBase> Event_OnItemSwitched = delegate { };
        public UnityAction<PlayerInput, ItemBase> Event_OnItemDropped = delegate { };
        public UnityAction<PlayerInput, int> Event_OnSlotsUpdated = delegate { };*/

    }
}