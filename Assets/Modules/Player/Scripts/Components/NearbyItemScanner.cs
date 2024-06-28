using System.Collections.Generic;
using System.Linq;
using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Manager;
using Modules.Player.Scripts.ComponentEventBus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Player.Scripts.Components
{
    public class NearbyItemScanner: MonoBehaviour
    {
        private PlayerComponentEventBus _playerComponentEventBus;
        
        //Do not serialize or make publix, editor bug, does not get updated in real time and throws object not found
        [SerializeField] private List<ItemBase> _itemsNear = new List<ItemBase>();
        
        private ItemBase _nearestItem;
        private PlayerInput _playerInput;
        
        private void Awake()
        {
            _playerInput = transform.root.GetComponent<PlayerInput>();
            _playerComponentEventBus = transform.root.GetComponent<PlayerComponentEventBus>();
        }

        private void OnEnable()
        {
            _playerComponentEventBus.ItemPickUpAttempt += OnInputItemPickUPAttempt;
        }

        private void OnDisable()
        {
            _playerComponentEventBus.ItemPickUpAttempt -= OnInputItemPickUPAttempt;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.TryGetComponent<ItemBase>(out ItemBase item) && item.ConditionToBePicked())
            {
                OnItemFoundOnRange(item);
            }
            else
            {
            }
        }

        //Keep in mind on destroy of item or pickup of item - this is not getting triggered
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<ItemBase>(out ItemBase item))
            {
                OnItemGoneFromRange(item);
            }
        }

        void OnItemFoundOnRange(ItemBase itemBase)
        { 
            if (!_itemsNear.Contains(itemBase))
            {
                // itemBase.Event_OnItemStateUpdate += OnItemStateUpdate;
                _itemsNear.Add(itemBase);
            }

            UpdateNearestItem();
            itemBase.RegisterAsNearbyPlayer(_playerInput);
        }
        
        void OnItemGoneFromRange(ItemBase itemBase)
        {
            if(_itemsNear.Contains(itemBase))
                _itemsNear.Remove(itemBase);
            itemBase.UnregisterAsNearbyPlayer(_playerInput);
            // itemBase.Event_OnItemStateUpdate -= OnItemStateUpdate;
            UpdateNearestItem();
        }
        
        private void UpdateNearestItem()
        {
            if (_itemsNear.Count == 0)
            {
                _nearestItem = null;
                return;
            }
            else
            {
                _itemsNear.Sort((item1, item2) =>
                    Vector3.Distance(transform.position, item1.transform.position)
                        .CompareTo(Vector3.Distance(transform.position, item2.transform.position))
                );
                _nearestItem = _itemsNear.Last();
            }
        }


        
        /// <summary>
        /// Exposed => Called by state machine
        /// </summary>
        public void OnInputItemPickUPAttempt()
        {
            if(_nearestItem != null)
                _playerComponentEventBus.ItemDoPickUpProcess.Invoke(_nearestItem);
        }
        
        /*void OnItemStateUpdate(EnumAllItemType.ItemState itemState, ItemBase itemBase)
        {
            //Item was picked
            if(itemState != EnumAllItemType.ItemState.OnGround_Idle && itemState != EnumAllItemType.ItemState.OnGround_PickupShowing)
                OnItemGoneFromRange(itemBase);
            
            //Item was dropped
            if(itemState == EnumAllItemType.ItemState.OnGround_Idle)
                OnItemFoundOnRange(itemBase);
        }*/
        
    }
}