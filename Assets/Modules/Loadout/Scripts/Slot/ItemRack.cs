using System.Collections.Generic;
using System.Linq;
using Characters.Player.Global;
using Modules.CommonEventBus;
using Modules.Loadout.Scripts.Actions;
using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Manager;
using Modules.Player.Scripts.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Modules.Loadout.Scripts.Slot
{
    [System.Serializable]
    public class ItemRack
    {
        public int testValu;
        //Item Rack state
        public List<ItemSlot> CarryingItemSlot { get; private set ;}
        public ItemSlot ActiveItemSlot { get; private set; }
        public int MAXSlot { get; private set; }
        
        //Refs which will not change
        public Transform ParentSocketTransform { get; private set; }
        public PlayerInput PlayerOwner { get; }
        private PlayerInputMapping _playerInputMapping;
        private readonly GameplayLoadout _gameplayLoadout;
        
        //Handlers
        private EquipUnequipHandler _equipUnequipHandler;
        private PickupItemActionHandler _pickupItemActionHandler;
        private SwapActionHandler _swapActionHandler;
        private SwitchItemHandler _switchItemHandler;
        private DropItemHandler _itemDropHandler;

        public ItemRack(GameplayLoadout gameplayLoadout)
        {
            //Fixed References
            _gameplayLoadout = gameplayLoadout;
            ParentSocketTransform = gameplayLoadout.Get_ItemSocketTransform();
            PlayerOwner = gameplayLoadout.Get_OwnerPlayerInput();
            _playerInputMapping = PlayerOwner.transform.GetComponent<PlayerInputMapping>();
            
            _equipUnequipHandler = new EquipUnequipHandler();
            _pickupItemActionHandler = new PickupItemActionHandler();
            _swapActionHandler = new SwapActionHandler();
            _switchItemHandler = new SwitchItemHandler();
            _itemDropHandler = new DropItemHandler();

            CarryingItemSlot = new List<ItemSlot>();
        }

        public void InitializeItemRack(int maxSlots, List<ItemBase> starterItems) 
        {
            //Create max slots, All slots are empty by default
            MAXSlot = maxSlots;
            for (int i = 0; i < maxSlots; i++)
            {
                CarryingItemSlot.Add(new ItemSlot());
            }

            //Manifest Items and perform pickup operation
            for (int i = 0; i < starterItems.Count; i++)
            {
                ItemBase createdItem = (Object.Instantiate(starterItems[i]));
                _pickupItemActionHandler.DoPickUpProcess(createdItem, CarryingItemSlot[i], this);
            }

            //Notify ItemRack is created
            PlayerInventoryEventBus.OnItemRackInitialized.Invoke(PlayerOwner, this);
            
            //Make first item as default equiped
            UpdateActiveItemSlotHolder(CarryingItemSlot[0].SelectSlot());
        }

        public void UpdateActiveItemSlotHolder(ItemSlot newItemSlotActive) { ActiveItemSlot = newItemSlotActive; }
        public bool AllItemSlotFull() => CarryingItemSlot.FindAll(slot => slot.FillState == ItemSlot.SlotFillState.Empty).Count == 0;
        
        public void ReceiveItemPick(ItemBase newItemToPick)
        {
            if (AllItemSlotFull())
            {
                ForwardItemSwap(newItemToPick);   
            }
            else
            {            
                ItemSlot foundEmptySlot = CarryingItemSlot.First(itemslot => itemslot.FillState == ItemSlot.SlotFillState.Empty);
                _pickupItemActionHandler.DoPickUpProcess(newItemToPick, foundEmptySlot, this);
            }
            
        }
        public void ReceiveItemSwitch()
        {
            ItemSlot previousItemSlot = ActiveItemSlot;
            _switchItemHandler.PerformItemSwitchLeftRight(
                0.1f, 
                _playerInputMapping.lastItemSwitchDirection, 
                this, 
                _equipUnequipHandler,
                (itemSwitchTo)=>
                {
                    /*if (itemSwitchTo == previousItemSlot)
                    {
                        DebugX.LogWithColorYellow($"Item did not switched, item slot {itemSwitchTo.Item.name} {itemSwitchTo.FillState} {itemSwitchTo.SelectionState}");
                    }
                    else
                    {
                        DebugX.LogWithColorYellow($"Item switched to , item slot {itemSwitchTo} {itemSwitchTo.FillState} {itemSwitchTo.SelectionState}");
                    }*/
                });
        }
        
        public void ReceiveItemDrop()
        {
            if(ActiveItemSlot.FillState == ItemSlot.SlotFillState.Hasitem)
                _itemDropHandler.DoDropForwardToPlayer(this);
        }
        
        private void ForwardItemSwap(ItemBase newItemToSwapWith)
        {
            DebugX.LogWithColorYellow("Forwarded swap");
            _swapActionHandler.DoSwapProcess(newItemToSwapWith, this,_pickupItemActionHandler, _itemDropHandler, _equipUnequipHandler);
        }
    }
}
