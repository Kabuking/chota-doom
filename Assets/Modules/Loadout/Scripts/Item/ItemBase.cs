using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Modules.CommonEventBus;
using Modules.Loadout.Scripts.Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Modules.Loadout.Scripts.Item
{
    public abstract class ItemBase : MonoBehaviour
    {
        [SerializeField] public ItemDataSo itemDataSo;
        [SerializeField] public Transform itemPickupPopUIPoint;
        // [SerializeField] private ItemPickPopUIOnGround UIdetails;
        public EnumAllItemType.ItemCategory ItemCategory;
        [SerializeField] private EnumAllItemType.ItemState itemState = EnumAllItemType.ItemState.OnGround_Idle;
        [SerializeField] protected EnumAllItemType.ItemId itemId;
        [SerializeField] protected EnumAllItemType.ItemParentSocketName socketTagName;

        [SerializeField] private HashSet<PlayerInput> playersOnPickupRange = new HashSet<PlayerInput>();
        public abstract void OnItemUse();
        [SerializeField] private PlayerInput ownerPlayer;

        [SerializeField] private Collider weaponCollider;

        protected CinemachineImpulseSource _cinemachineImpulseSource;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void Start()
        {
            weaponCollider = GetComponent<Collider>();
        }

        private void Update()
        {
            if(itemState == EnumAllItemType.ItemState.OnGround_PickupShowing)
                ScanNearestPlayer();
            OnUpdate();
        }
        
        protected virtual void OnUpdate(){}
        public virtual void OnItemRefill() { }
        public abstract void OnItemUseStop();
        
        // Picked
        public virtual void SetItemOnPicked(PlayerInput playerInput)
        {
            weaponCollider.enabled = false;
            ownerPlayer = playerInput;
            UnregisterAllPlayers();
            SetItemOnUnequipped();
        }

        // Equipped
        public virtual void SetItemOnEquipped()
        {
            UpdateStateTo(EnumAllItemType.ItemState.Equipped);
        }

        // Dropped
        public virtual void SetItemOnDropped(PlayerInput playerWhoDropped, Vector3 dropPosition)
        {
            //Rethink what to do with transform location and rotation
            var transform1 = transform;
            transform1.parent = null;
            transform1.position = dropPosition;
            transform1.rotation = Quaternion.identity;

            SetItemOnGroundIdle();
            RegisterAsNearbyPlayer(playerWhoDropped);
        }


        // UnEquip
        public void SetItemOnUnequipped()
        {
            UpdateStateTo(EnumAllItemType.ItemState.Unequip);
            gameObject.SetActive(false);
        }

        // Ground_Showing popup
        public void SetItemOnGroundPickupShowing()
        {
            UpdateStateTo(EnumAllItemType.ItemState.OnGround_PickupShowing);
            PlayerInventoryEventBus.ShowItemPickupInfo.Invoke(this);
        }
        
        //Ground Idle
        void SetItemOnGroundIdle()
        {
            gameObject.SetActive(true);
            ownerPlayer = null;
            weaponCollider.enabled = true;
            UpdateStateTo(EnumAllItemType.ItemState.OnGround_Idle);
        }
        
        
        public Transform GetParentSocketToSpawn(Transform socketParentTransform)
        {

            foreach (Transform child in socketParentTransform)
            {
                if (child.CompareTag(socketTagName.ToString()))
                {
                    return child;
                }
            }

            return null;
        }

        public EnumAllItemType.ItemState GetItemState => itemState;
        public EnumAllItemType.ItemId GetItemId => itemId;

        void UpdateStateTo(EnumAllItemType.ItemState newItemState)
        {
            itemState = newItemState;
            Event_OnItemStateUpdate.Invoke(itemState, this);
        }
        
        public bool ConditionToBePicked() => itemState == EnumAllItemType.ItemState.OnGround_Idle ||
                                             itemState == EnumAllItemType.ItemState.OnGround_PickupShowing;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnTriggerEnterPlayer(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnTriggerExitPlayer(other);
            }
        }

        public void RegisterAsNearbyPlayer(PlayerInput playerInput)
        {
            if (itemState == EnumAllItemType.ItemState.OnGround_Idle 
               || itemState == EnumAllItemType.ItemState.OnGround_PickupShowing)
            {
                playersOnPickupRange.Add(playerInput);
                SetItemOnGroundPickupShowing();
            }
        }
        
        public void UnregisterAsNearbyPlayer(PlayerInput playerInput)
        {
            if (itemState == EnumAllItemType.ItemState.OnGround_PickupShowing)
            {
                playersOnPickupRange.Remove(playerInput);
                if (playersOnPickupRange.Count == 0)
                    SetItemOnGroundIdle();
            }
        }

        void UnregisterAllPlayers()
        {
            playersOnPickupRange.Clear();
        }
        
        protected virtual void OnTriggerEnterPlayer(Collider player) { }
        protected virtual void OnTriggerExitPlayer(Collider player) { }
        public abstract int GetItemAmount();
        public UnityAction<int> OnAmountUpdated = delegate(int arg0) {  };
        private PlayerInput nearestPlayer;
        public PlayerInput Get_NearestPlayerInPickupRange => nearestPlayer;
        public bool SomePlayerStillInPickupRange => playersOnPickupRange.Count > 0;
        void ScanNearestPlayer()
        {
            if (playersOnPickupRange != null && playersOnPickupRange.Count > 0)
            {
                nearestPlayer = playersOnPickupRange.OrderBy(
                    p => Vector3.Distance(transform.position, p.transform.position)
                ).First();
            }
        }

        public bool AmINearestPlayer(PlayerInput incomingPlayer) => nearestPlayer == incomingPlayer;



        public UnityAction<EnumAllItemType.ItemState, ItemBase> Event_OnItemStateUpdate = delegate {  };
        // public UnityAction Event_OnHidePopUp = delegate {  };
    }

}