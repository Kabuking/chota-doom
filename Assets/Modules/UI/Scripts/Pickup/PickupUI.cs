using System;
using Characters.Player.Global;
using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Modules.UI.Scripts.Pickup
{
    /// <summary>
    /// Script to show pickup pop up when Player is in range
    /// </summary>
    public class PickupUI : MonoBehaviour
    {
        public UnityAction<ItemBase> RemoveMe;
        
        [SerializeField] private float yOffset = 5;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private TextMeshProUGUI amountText;

        [SerializeField] private TextMeshProUGUI playerNumberIndicator;

        [Header("Debugginh")] private ItemBase _popupForItem;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (target != null && _popupForItem.Get_NearestPlayerInPickupRange != null)
            {
                var a = Camera.main.WorldToScreenPoint(target.position);
                Vector3 screenPos = new Vector3(a.x, a.y + yOffset, a.z);
                screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
                screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);
                rectTransform.position = screenPos;
                playerNumberIndicator.SetText($"P{_popupForItem.Get_NearestPlayerInPickupRange.playerIndex + 1}");
            }
        }

        [SerializeField] private Transform target;


        /// <summary>
        /// On Created
        /// </summary>
        /// <param name="player"> Whoever triggered it</param>
        /// <param name="itemBase"> Item Details</param>
        /// <returns></returns>
        public PickupUI InitializeItemPopup(ItemBase itemBase)
        {
            displayName.SetText(itemBase.itemDataSo.itemDisplayName);
            amountText.SetText(itemBase.itemDataSo.itemAmount.ToString());
            _popupForItem = itemBase;
            target = itemBase.itemPickupPopUIPoint;
            _popupForItem.Event_OnItemStateUpdate += OnItemStateUpdate;
            return this;
        }

        void OnItemStateUpdate(EnumAllItemType.ItemState itemStateUpdatedTo, ItemBase item)
        {
            if (itemStateUpdatedTo != EnumAllItemType.ItemState.OnGround_PickupShowing)
            {
                // DebugX.LogWithColorYellow($"Will Hilde popup for item {item.name} On item state update "+itemStateUpdatedTo);
                OnHidePopup();
            }
            else
            {
                // DebugX.LogWithColorYellow($"On item state updated for item {item.name} "+itemStateUpdatedTo);
            }
        }

        void OnHidePopup()
        {
            _popupForItem.Event_OnItemStateUpdate -= OnItemStateUpdate;
            RemoveMe.Invoke(_popupForItem);
        }
        
    }
}
