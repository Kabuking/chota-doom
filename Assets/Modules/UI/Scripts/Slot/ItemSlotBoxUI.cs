using Modules.Loadout.Scripts.Slot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts.Slot
{
    public class ItemSlotBoxUI: MonoBehaviour
    {
        [SerializeField] private float equippedItemHighlightHeight = 10;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI amountText;

        public ItemSlot LinkedItemSlot { get; private set; }
        // public LoadOutSingleState CurrentLoadOutState { get; private set; }

        [Header("Empty LoadOutState")]
        [SerializeField] private Sprite defaultLoadOutEmptyIcon;

        private RectTransform _rectTransform;
        private float _defaultHeightRT;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _defaultHeightRT = _rectTransform.rect.height;
        }

        /// <summary>
        /// Entry Point or Initialization, Item is still null
        /// </summary>
        public ItemSlotBoxUI InitialAssignItemSlot(ItemSlot incomingItemSlot)
        {
            OnAssignItemSlot(incomingItemSlot);
            return this;
        }
        
        public void InitialEmptySlotSetup()
        {
            ClearUIInfo();
            gameObject.SetActive(true);
        }
        
        void ClearUIInfo()
        {
            itemIcon.sprite = defaultLoadOutEmptyIcon;
            itemIcon.gameObject.SetActive(false);
            amountText.SetText("");
        }


        //Case of drop
        void OnUnAssigned()
        {
            ClearUIInfo();
            
            //Item specific listeners
            LinkedItemSlot.Item.OnAmountUpdated -= OnAmountChange;
        }

        //Item incoming
        void OnAssignItemSlot(ItemSlot incomingItemSlot)
        {
            
            LinkedItemSlot = incomingItemSlot;
            LinkedItemSlot.EventOnAssigned += OnAssignItemSlot;
            LinkedItemSlot.EventOnUnassigned += OnUnAssigned; 
            LinkedItemSlot.EventOnSelect += OnSelect; 
            LinkedItemSlot.EventOnDeselect += OnDeselect;

            if (LinkedItemSlot.FillState == ItemSlot.SlotFillState.Hasitem)
            {
                LinkedItemSlot.Item.OnAmountUpdated += OnAmountChange;
                itemIcon.sprite = LinkedItemSlot.Item.itemDataSo.itemDisplayIcon;
                amountText.SetText(LinkedItemSlot.Item.GetItemAmount().ToString());
            }
            itemIcon.gameObject.SetActive(true);
        }

        void ListeningSlotEvents()
        {

        }

        void OnSelect()
        {
            SlotSizeOnSelect();
        }
        
        void OnDeselect()
        {
            SlotSizeOnDeselect();
        }

        void OnAmountChange(int newAmount)
        {
            amountText.SetText(newAmount.ToString());
        }

        
        void SlotSizeOnSelect()
        {
            _rectTransform.sizeDelta = new Vector2(
                _rectTransform.sizeDelta.x, 
                _defaultHeightRT + equippedItemHighlightHeight);
        }

        void SlotSizeOnDeselect()
        {
            _rectTransform.sizeDelta = new Vector2(
                _rectTransform.sizeDelta.x, 
                _defaultHeightRT);
        }
    }
}
