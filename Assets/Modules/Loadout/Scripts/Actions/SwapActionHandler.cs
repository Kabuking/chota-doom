using Characters.Player.Global;
using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Manager;
using Modules.Loadout.Scripts.Slot;

namespace Modules.Loadout.Scripts.Actions
{
    public class SwapActionHandler
    {
        public ItemBase DoSwapProcess(
            ItemBase itemToSwap, 
            ItemRack itemRack, 
            PickupItemActionHandler pickupItemActionHandler,
            DropItemHandler dropItemHandler, 
            EquipUnequipHandler equipUnequipHandler)
        {
            switch (itemToSwap.ItemCategory)
            {
                case EnumAllItemType.ItemCategory.Weapon: return ApplyWeaponOrConsumableSwap(itemToSwap, itemRack, pickupItemActionHandler, dropItemHandler, equipUnequipHandler);
                case EnumAllItemType.ItemCategory.Consumables:return ApplyWeaponOrConsumableSwap(itemToSwap, itemRack, pickupItemActionHandler, dropItemHandler, equipUnequipHandler);
                case EnumAllItemType.ItemCategory.Ammunition: return ApplyAmmoSwap(itemToSwap, itemRack, pickupItemActionHandler, dropItemHandler, equipUnequipHandler); //TODO: Complete
                case EnumAllItemType.ItemCategory.Gear: return ApplyGearSwap(itemToSwap, itemRack, pickupItemActionHandler, dropItemHandler, equipUnequipHandler); //TODO: Complete it
                default: 
                    DebugX.LogWithColorCyan("default defaultdefault");
                    return null;
            } 
        }
        
        private ItemBase ApplyWeaponOnlySwap(
            ItemBase itemToSwap, 
            ItemRack itemRack, 
            PickupItemActionHandler pickupItemActionHandler,
            DropItemHandler dropItemHandler, 
            EquipUnequipHandler equipUnequipHandler)
        {
            //Todo
            return itemToSwap;
        }
        
        private ItemBase ApplyConsumableOnlySwap(
            ItemBase itemToSwap, 
            ItemRack itemRack, 
            PickupItemActionHandler pickupItemActionHandler,
            DropItemHandler dropItemHandler, 
            EquipUnequipHandler equipUnequipHandler)
        {
            //Todo
            return itemToSwap;
        }
        
        private ItemBase ApplyAmmoSwap(
            ItemBase itemToSwap, 
            ItemRack itemRack, 
            PickupItemActionHandler pickupItemActionHandler,
            DropItemHandler dropItemHandler, 
            EquipUnequipHandler equipUnequipHandler)
        {
            
            //Todo
            return null;
        }
        
        private ItemBase ApplyGearSwap(
            ItemBase itemToSwap, 
            ItemRack itemRack, 
            PickupItemActionHandler pickupItemActionHandler,
            DropItemHandler dropItemHandler, 
            EquipUnequipHandler equipUnequipHandler)
        {
            
            //TODO logic ?
            return null;
        }

        private ItemBase ApplyWeaponOrConsumableSwap(
            ItemBase itemToSwap, 
            ItemRack itemRack, 
            PickupItemActionHandler pickupItemActionHandler,
            DropItemHandler dropItemHandler, 
            EquipUnequipHandler equipUnequipHandler)
        {

            //drop carrying item
            ItemSlot itemSlotDropped = dropItemHandler.DoDropForwardToPlayer(itemRack);

            //Pick item
            ItemSlot itemSlotWherePickWasApplied = pickupItemActionHandler.DoPickUpProcess(itemToSwap, itemSlotDropped,itemRack);
            
            //Equip item
            equipUnequipHandler.EquipItem(itemSlotWherePickWasApplied, itemRack);
            
            return itemToSwap;
        }
    }
}