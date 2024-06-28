using System.Collections;
using Modules.Loadout.Scripts.Slot;
using UnityEngine;

namespace Modules.Loadout.Scripts.Actions
{
    public class SwitchItemHandler
    {
                
        /// <summary>
        /// ITEM SWITCH  => Applying the next item available from itemlist inventory
        /// Called from State Machine
        /// </summary>
        public void PerformItemSwitchLeftRight(
            float itemSwitchDelay, 
            int switchDirection, 
            ItemRack itemRack, 
            EquipUnequipHandler equipUnequipHandler,
            System.Action<ItemSlot> callback)
        {
            itemRack.PlayerOwner
                .StartCoroutine(DoItemSwitch(itemRack, equipUnequipHandler, itemSwitchDelay, switchDirection, callback));
        }

        IEnumerator DoItemSwitch(
            ItemRack itemRack, 
            EquipUnequipHandler equipUnequipHandler,
            float itemSwitchDelay, 
            int switchDirection,
            System.Action<ItemSlot> callback)
        {
            yield return new WaitForSeconds(itemSwitchDelay);
            
            ItemSlot itemSwitchedTo = itemRack.ActiveItemSlot;
            int currentIndex = itemRack.CarryingItemSlot.IndexOf(itemSwitchedTo);

            int nextIndex = (currentIndex + (switchDirection * 1)) % itemRack.CarryingItemSlot.Count;
            if (nextIndex < 0)
            {
                itemSwitchedTo = equipUnequipHandler.EquipItem(itemRack.CarryingItemSlot[^1], itemRack);
            }else if (nextIndex < itemRack.CarryingItemSlot.Count)
            {
                itemSwitchedTo = equipUnequipHandler.EquipItem(itemRack.CarryingItemSlot[nextIndex], itemRack);
            }
            callback(itemSwitchedTo);
        }
        
        
    }
}