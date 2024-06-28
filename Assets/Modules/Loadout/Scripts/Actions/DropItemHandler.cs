using Modules.Loadout.Scripts.Slot;
using UnityEngine;

namespace Modules.Loadout.Scripts.Actions
{
    public class DropItemHandler
    {
        public ItemSlot DoDropProcessOnProvidedTransform(ItemSlot itemSlotToDrop, Transform dropLocation, ItemRack itemRack)
        {
            var playerTransform = itemRack.PlayerOwner.transform;

            //Update Transform
            var itemToDropTransform = itemSlotToDrop.Item.transform;
            itemToDropTransform.parent = null;
            itemToDropTransform.position = dropLocation.position;
            itemToDropTransform.rotation = dropLocation.rotation;
            
            //Remove from inventory Carrying
            itemSlotToDrop.UnAssignItem();
            
            //Update State
            // itemSlotToDrop.SetItemOnDropped(gameplayLoadout.Get_OwnerPlayerInput());
            
            return itemSlotToDrop;
        }

        public ItemSlot DoDropForwardToPlayer(ItemRack itemRack)
        {
            var playerTransform = itemRack.PlayerOwner.transform;
            Vector3 dropPosition = playerTransform.position + (playerTransform.forward * 1);
            
            itemRack.ActiveItemSlot.Item.SetItemOnDropped(itemRack.PlayerOwner, dropPosition);

            //Remove from inventory Carrying
            itemRack.ActiveItemSlot.UnAssignItem();
                
                
            //TODO: Rethink here
            //Set active item handling to something else ?
            
            return itemRack.ActiveItemSlot;
        }
    }
}