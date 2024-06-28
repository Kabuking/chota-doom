using Characters.Player.Global;
using Modules.Loadout.Scripts.Slot;

namespace Modules.Loadout.Scripts.Actions
{
    public class EquipUnequipHandler
    {
        public ItemSlot EquipItem(ItemSlot itemSlotToEquip, ItemRack itemRack)
        {
            itemRack.ActiveItemSlot.DeSelectSlot();
            
            //Select new
            itemSlotToEquip.SelectSlot();
            
            //Notify
            itemRack.UpdateActiveItemSlotHolder(itemSlotToEquip);  
            return itemSlotToEquip;
        }
    }
}
