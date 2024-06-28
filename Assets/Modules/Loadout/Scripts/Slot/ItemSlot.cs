using Modules.Loadout.Scripts.Item;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Modules.Loadout.Scripts.Slot
{
    public class ItemSlot
    {
        public enum SlotFillState { Empty, Hasitem }
        public enum SlotSelectionState { Selected, Deselected }

        public SlotFillState FillState{ get; private set; }
        public SlotSelectionState SelectionState{ get; private set; }
        
        public ItemBase Item { get; private set; }

        public UnityAction<ItemSlot> EventOnAssigned = delegate { };
        public UnityAction EventOnUnassigned = delegate { };
        public UnityAction EventOnSelect = delegate { };
        public UnityAction EventOnDeselect = delegate { };

        public ItemSlot()
        {
            Item = null;
            SetFillStateToEmpty();
            SetSelectionDeselected();
        }

        private void SetFillStateToEmpty() { FillState = SlotFillState.Empty; }
        private void SetFillStateToHasItem() => FillState = SlotFillState.Hasitem;

        private void SetSelectionSelected() { SelectionState = SlotSelectionState.Selected; EventOnSelect.Invoke(); }
        private void SetSelectionDeselected() { SelectionState = SlotSelectionState.Deselected; EventOnDeselect.Invoke(); }

        public void AssignItem(ItemBase incomingItem)
        {
            Item = incomingItem;
            SetFillStateToHasItem();
            EventOnAssigned.Invoke(this);
        }

        public void UnAssignItem()
        {
            EventOnUnassigned.Invoke();
            Item = null;
            SetFillStateToEmpty();
        }

        public ItemSlot SelectSlot()
        {
            if(FillState == SlotFillState.Hasitem)
                Item.gameObject.SetActive(true);
            SetSelectionSelected();
            return this;
        }

        public ItemSlot DeSelectSlot()
        {
            if(FillState == SlotFillState.Hasitem)
                Item.gameObject.SetActive(false);
            SetSelectionDeselected();
            return this;
        }
    }
}
