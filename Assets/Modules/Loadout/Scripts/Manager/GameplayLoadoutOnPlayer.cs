using System.Collections;
using System.Collections.Generic;
using Characters.Player.Global;
using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Slot;
using Modules.Player.Scripts.ComponentEventBus;
using Modules.Player.Scripts.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Loadout.Scripts.Manager
{
    public class GameplayLoadoutOnPlayer: GameplayLoadout
    {
        [SerializeField] private float loadOutInitializationDelay = 1f;
        [SerializeField] private int maxNumberOfSlots = 3;
        [SerializeField] private float itemSwitchDelay=1f;

        //This is what player will start will, later it will be loaded through saved progress
        public List<ItemBase> startingItems;
        [SerializeField] private ItemRack itemRack;

        [SerializeField] private Transform sockeTransform;
        [SerializeField] private ItemBase itemTracedForPickup;

        //Refs
        private PlayerComponentEventBus _playerComponentEventBus;
        private Camera _mainCamera;
        private PlayerInput _playerInput;
        private PlayerInputMapping _playerInputMapping;

        public void InitLoadOut()
        {
            DebugX.LogWithColorYellow(startingItems.Count + " << Starting items");
            _playerComponentEventBus = GetComponent<PlayerComponentEventBus>();
            _mainCamera = Camera.main;
            _playerInput = GetComponent<PlayerInput>();
            _playerInputMapping = GetComponent<PlayerInputMapping>();
            
            _playerComponentEventBus.ItemDoPickUpProcess += ReceivePickUpInput;
            _playerComponentEventBus.ItemDropAttempt += ReceiveItemDropInput;
            
            StartCoroutine(LoadOutInitialization(0));

            
        }

        private void OnDisable()
        {
            _playerComponentEventBus.ItemDoPickUpProcess -= ReceivePickUpInput;
            _playerComponentEventBus.ItemDropAttempt -= ReceiveItemDropInput;
        }

        /*
        private void Start()
        {
            StartCoroutine(LoadOutInitialization(loadOutInitializationDelay));
        }
        */

        /*
         * Delay so that other components get notified
         * Delay to add a fade in affect of the UI entry instead of instant
         */
        IEnumerator LoadOutInitialization(float loadInitializationDelay)
        {
            yield return new WaitForSeconds(loadInitializationDelay);
            
            itemRack = new ItemRack(this);
            itemRack.InitializeItemRack(maxNumberOfSlots, startingItems);
        }

        //TODO: Null check remove if possible
        override public void OnItemUse()
        {
            // Debug.Log("Item use call "+itemRack.ActiveItemSlot == null);
            itemRack.ActiveItemSlot?.Item?.OnItemUse();
        }
        override public void OnItemUseStop() { itemRack.ActiveItemSlot?.Item?.OnItemUseStop(); }
        override public void ReceiveItemSwitchLeftRight() => itemRack.ReceiveItemSwitch();
        private void ReceivePickUpInput(ItemBase pickUpItemBase) { if (pickUpItemBase == null) return; itemRack.ReceiveItemPick(pickUpItemBase); }
        private void ReceiveItemDropInput() { itemRack.ReceiveItemDrop(); }

        

        public override  Transform Get_ItemSocketTransform() => sockeTransform;
        public override  PlayerInput Get_OwnerPlayerInput() => _playerInput;
        public override List<ItemBase> GetStarterItemList() => startingItems;
    }
}