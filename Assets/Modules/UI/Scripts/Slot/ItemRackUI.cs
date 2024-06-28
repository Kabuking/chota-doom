using System.Collections.Generic;
using System.Linq;
using Modules.Loadout.Scripts.Slot;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Modules.UI.Scripts.Slot
{
    /// <summary>
    /// LoadUI which gets created is managed by it
    /// </summary>
    public class ItemRackUI: MonoBehaviour
    {
        [SerializeField] private int loadOutSizeBox = 55;
        [SerializeField] private ItemSlotBoxUI itemSlotBoxUIItemTemplate;
        [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
        
        [Header("Do not edit in play mode, just debugging")]
        [SerializeField] private List<ItemSlotBoxUI> cachedItemSlotUIList;
        [Range(1,5)]
        private int _maxItemSlot;
        private ItemSlotBoxUI _equippedSlotBox;
        private ItemRack _itemRack;

        private PlayerInput _playerInput;
        private RectTransform _hlgRectTransform;
        
        public void InitiateLoadOut(PlayerInput playerInput, ItemRack itemRack)
        {
            _playerInput = playerInput;
            _itemRack = itemRack;
            _hlgRectTransform = horizontalLayoutGroup.GetComponent<RectTransform>();
            SetupLoadOutSlot(playerInput, itemRack);
        }
        
        //TODO: Slot Upgrade
        void SetMaxSlotValue(PlayerInput playerInput, int newMaxItemSlot)
        {
            _maxItemSlot = newMaxItemSlot;
            
            //Create another Empty Slot if new found - in case of upgrade
            //Delete slot - should not happen as there is no downgrade

        }

        void SetupLoadOutSlot(PlayerInput playerInput, ItemRack itemRack)
        {
            SetMaxSlotValue(playerInput, itemRack.MAXSlot);

            if (playerInput.playerIndex == 0)
                SetLoadoutSizeForPlayer1();
            else
                SetLoadoutSizeForPlayer2();
            
            
            var itemSlotUIChildList = horizontalLayoutGroup.GetComponentsInChildren<ItemSlotBoxUI>();
            cachedItemSlotUIList = new List<ItemSlotBoxUI>();

            //Hiding all slots initially
            foreach (var itemSlotUI in itemSlotUIChildList)
            {
                itemSlotUI.gameObject.SetActive(false);
            }
            
            //Cache according to max item slot
            for (int i = 0; i < itemRack.CarryingItemSlot.Count; i++)
            {
                cachedItemSlotUIList.Add(itemSlotUIChildList[i]);
            }
            
            //Initiate each slots as empty initially, also make it visible
            foreach (var itemSlotUI in cachedItemSlotUIList)
            {
                itemSlotUI.InitialEmptySlotSetup();
            }

            //One to One mapping with itemslot <-> itemSlotContainerUI
            for (var i = 0; i < _itemRack.CarryingItemSlot.Count(); i++)
            {
                cachedItemSlotUIList[i].InitialAssignItemSlot(_itemRack.CarryingItemSlot[i]);
            }
        }
        
        void SetLoadoutSizeForPlayer1()
        {
            horizontalLayoutGroup.padding.left = loadOutSizeBox;
            horizontalLayoutGroup.padding.bottom = loadOutSizeBox;
            horizontalLayoutGroup.spacing = loadOutSizeBox % 10;
            horizontalLayoutGroup.childAlignment = TextAnchor.LowerLeft;
        }

        void SetLoadoutSizeForPlayer2()
        {
            horizontalLayoutGroup.padding.right = loadOutSizeBox;
            horizontalLayoutGroup.padding.bottom = loadOutSizeBox;
            horizontalLayoutGroup.spacing = loadOutSizeBox % 10;
            horizontalLayoutGroup.childAlignment = TextAnchor.LowerRight;
        }

        public void OnPlayerLeftGameSession(PlayerInput playerInput)
        {
            
        }
    }
}