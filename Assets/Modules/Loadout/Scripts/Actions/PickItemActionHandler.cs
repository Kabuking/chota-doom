using Characters.Player.Global;
using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Manager;
using Modules.Loadout.Scripts.Slot;
using UnityEngine;

namespace Modules.Loadout.Scripts.Actions
{
    public class PickupItemActionHandler
    {
        private ItemSlot PickUpItem(ItemBase itemToPick, ItemSlot itemSlot, ItemRack itemRack)
        {   
            //Item related Operations
            itemToPick.gameObject.SetActive(false);
            var transform1 = itemToPick.transform;
            Transform parentSocket = itemToPick.GetParentSocketToSpawn(itemRack.ParentSocketTransform);
            itemToPick.transform.SetParent(parentSocket);
            transform1.localPosition = Vector3.zero;
            transform1.localRotation = Quaternion.identity;
            transform1.localScale = Vector3.one;
            itemToPick.SetItemOnPicked(itemRack.PlayerOwner);

            //ItemSlot related operations
            itemSlot.AssignItem(itemToPick);
            
            //ItemRack relation operations
            if (itemSlot == itemRack.ActiveItemSlot)
            {
                itemRack.UpdateActiveItemSlotHolder(itemSlot.SelectSlot());
            }

            return itemSlot;
        }

        
        public ItemSlot DoPickUpProcess(ItemBase itemToPickup, ItemSlot slotToStore, ItemRack itemRack)
        {
            switch (itemToPickup.ItemCategory)
            {
                case EnumAllItemType.ItemCategory.Weapon: return ApplyWeaponPickUp(itemToPickup, slotToStore, itemRack);
                case EnumAllItemType.ItemCategory.Ammunition: return ApplyAmmoPickUp(itemToPickup, slotToStore, itemRack); //TODO: Complete
                case EnumAllItemType.ItemCategory.Consumables:return ApplyConsumablePickUp(itemToPickup, slotToStore, itemRack);
                case EnumAllItemType.ItemCategory.Gear: return ApplyGearPickUp(itemToPickup,slotToStore,  itemRack); //TODO: Complete it
                default: 
                    DebugX.LogWithColorCyan("default defaultdefault");
                    return null;
            }
        }


        ItemSlot ApplyWeaponPickUp(ItemBase weaponItem, ItemSlot slotToStore, ItemRack itemRack)
        {
            // DebugX.LogWithColorCyan($"ApplyWeaponPickUp {weaponItem.name}");

            return PickUpItem(weaponItem, slotToStore, itemRack);
        }
        
        ItemSlot ApplyConsumablePickUp(ItemBase weaponItem, ItemSlot slotToStore, ItemRack itemRack)
        {
            // DebugX.LogWithColorCyan("ApplyConsumablePickUp");

            return PickUpItem(weaponItem,  slotToStore, itemRack);
        }
        
        ItemSlot ApplyGearPickUp(ItemBase weaponItem, ItemSlot slotToStore, ItemRack itemRack)
        {
            // DebugX.LogWithColorCyan("ApplyGearPickUp");

            return null;
        }
        
        ItemSlot ApplyAmmoPickUp(ItemBase weaponItem, ItemSlot slotToStore, ItemRack itemRack)
        {
            // DebugX.LogWithColorCyan("ApplyAmmoPickUp");

            return null;
        }

    }
}