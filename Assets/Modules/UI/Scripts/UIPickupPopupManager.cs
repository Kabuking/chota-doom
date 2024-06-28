using System.Collections.Generic;
using Characters.Player.Global;
using Modules.CommonEventBus;
using Modules.Loadout.Scripts.Item;
using Modules.UI.Scripts.Pickup;
using UnityEngine;

namespace Modules.UI.Scripts
{
    public class UIPickupPopupManager: MonoBehaviour
    {
        [SerializeField] private PickupUI pickupUIItemPopUpTemplate;
        private Dictionary<ItemBase, PickupUI> _createdPopups = new Dictionary<ItemBase, PickupUI>();
            
        private void OnEnable()
        {
            PlayerInventoryEventBus.ShowItemPickupInfo += OnShowPickPopup;
        }

        private void OnDisable()
        {
            PlayerInventoryEventBus.ShowItemPickupInfo -= OnShowPickPopup;
        }

        void OnShowPickPopup(ItemBase itemBase)
        {
            if (!_createdPopups.ContainsKey(itemBase))
            {
                //Does not exists, create the popup
                // DebugX.LogWithColorCyan($"pop up for {itemBase.name} does not exist, creating one");
                var popup = Instantiate(pickupUIItemPopUpTemplate, transform)
                    .InitializeItemPopup(itemBase);
                _createdPopups[itemBase] = popup;
                _createdPopups[itemBase].RemoveMe += OnHidePopUp;
                popup.gameObject.SetActive(true);
            }
        }

        void OnHidePopUp(ItemBase itemBase)
        {
            _createdPopups[itemBase].RemoveMe -= OnHidePopUp;
            // DebugX.LogWithColorCyan($"pop up for {itemBase.name} will be destroyed");
            Destroy(_createdPopups[itemBase].gameObject);
            _createdPopups.Remove(itemBase);
        }
    }
}