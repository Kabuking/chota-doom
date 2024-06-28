using Modules.CommonEventBus;
using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Manager;
using Modules.Player.Scripts.ComponentEventBus;
using Modules.Player.Scripts.InputSystem;
using Modules.Player.Scripts.PlayerData;
using Modules.Player.Scripts.PlayerStateMachine.model;
using Modules.Player.Scripts.PlayerStateMachine.PlayerStates;
using UnityEngine;
using UnityHFSM;

namespace Modules.Player.Scripts.PlayerController
{
    [RequireComponent(typeof(PlayerComponentEventBus))]
    public class PlayerController: MonoBehaviour
    {
        public PlayerStats playerStats;
        private StateMachine<PlayerStateName, PlayerStateTransitionEvent> playerFSM;
        [SerializeField] private PlayerStateName currentStateName;
        public bool enableTickOnStateLogic = true;
        
        //References
        [SerializeField] private PlayerCoopEventBus _playerCoopEventBus;
        protected GameplayLoadoutOnPlayer _itemManager;
        PlayerInputMapping _playerInputMapping;
        private PlayerGameplayStatsState _playerGameplayStatsState;
        private PlayerComponentEventBus _playerComponentEventBus;

        private void Awake()
        {
            // _playerWeaponManager = GetComponent<PlayerWeaponManager>();
            // _inventoryManager = GetComponent<InventoryManager>();

            _playerComponentEventBus = GetComponent<PlayerComponentEventBus>();
            _playerInputMapping = GetComponent<PlayerInputMapping>();
            _itemManager = GetComponent<GameplayLoadoutOnPlayer>();
            // _itemManager.InitItemManager(initialStateName);
            
            InitializePlayerGameplayState();
            InitializeFsmStates();
            
        }

        private void OnEnable()
        {
            //Item Related
            _playerInputMapping.ActiveItemUseEventPerformed += Trigger_OnPlayerInput_ItemUsePerform;
            _playerInputMapping.ActiveItemUseEventCanceled += Trigger_OnPlayerInput_ItemUseCanceled;
            _playerInputMapping.ItemSwitchLeftRightEventPerformed += Trigger_OnPlayerInput_ItemSwitchPerformLR;
            _playerInputMapping.ItemPickupEventPerformed += Trigger_Item_PickUp;
            _playerInputMapping.ItemDropEventPerformed += Trigger_Item_Drop;
            _playerComponentEventBus.ItemSwitchFinished += Trigger_Item_SwitchFinished;
            
            //Target Related
            _playerInputMapping.TargetSwitchSingledButtonPressedEvent += Trigger_OnPlayerTargetSwitchButtonPressed;
            _playerInputMapping.TargetSwitchRSFlickEvent += Trigger_OnPlayerTargetSwitchRSFlick;
            _playerInputMapping.TargetSwitchLRPressedEvent += Trigger_OnPlayerTargetSwitchLR;
            
            // _playerComponentEventBus.Invoke();

        }

        private void OnDisable()
        {
            //Item Related
            _playerInputMapping.ActiveItemUseEventPerformed -= Trigger_OnPlayerInput_ItemUsePerform;
            _playerInputMapping.ActiveItemUseEventCanceled -= Trigger_OnPlayerInput_ItemUseCanceled;
            _playerInputMapping.ItemSwitchLeftRightEventPerformed -= Trigger_OnPlayerInput_ItemSwitchPerformLR;
            _playerInputMapping.ItemPickupEventPerformed -= Trigger_Item_PickUp;
            _playerInputMapping.ItemDropEventPerformed -= Trigger_Item_Drop;
            _playerComponentEventBus.ItemSwitchFinished -= Trigger_Item_SwitchFinished;
            
            //Target Related
            _playerInputMapping.TargetSwitchSingledButtonPressedEvent -= Trigger_OnPlayerTargetSwitchButtonPressed;
            _playerInputMapping.TargetSwitchRSFlickEvent -= Trigger_OnPlayerTargetSwitchRSFlick;
            _playerInputMapping.TargetSwitchLRPressedEvent -= Trigger_OnPlayerTargetSwitchLR;

        }

        private void Update()
        {
            if (enableTickOnStateLogic)
            {
                playerFSM.OnLogic();
            }
            currentStateName = playerFSM.ActiveStateName;

        }
        
        void InitializePlayerGameplayState()
        {
            _playerGameplayStatsState = GetComponent<PlayerGameplayStatsState>();
        }
        
        void InitializeFsmStates()
        {            
            playerFSM = new StateMachine<PlayerStateName, PlayerStateTransitionEvent>();
            
            //Locomotion_NotAiming
            PlayerLocomotionStateNotAiming playerLocomotionNotAimingState = new PlayerLocomotionStateNotAiming(playerFSM, this);
            playerFSM.AddState(
                PlayerStateName.Locomotion_NotAiming,
                playerLocomotionNotAimingState
                    .AddAction_ItemUse_PerformAndCancel()
                    .AddAction_TargetSwitchButtonPressed()
                    .AddAction_TargetSwitchRSFlicked()
                    .AddAction_TargetSwitchLR()
                    .AddAction_ItemSwitch_PerformAndFinish()
                    .AddAction_ItemPickUp()
                    .AddAction_ItemDrop()
                );

            //Evade_State
            PlayerEvadeState playerEvadeState = new PlayerEvadeState(playerFSM, this);
            playerFSM.AddState(
                PlayerStateName.Evade, 
                playerEvadeState
                    .AddAction_TargetSwitchButtonPressed()
                    .AddAction_TargetSwitchRSFlicked()
            );
            
            
            playerFSM.AddTriggerTransitionFromAny(PlayerStateTransitionEvent.Transition_To_LocomotionNotAiming, PlayerStateName.Locomotion_NotAiming);
            playerFSM.AddTriggerTransitionFromAny(PlayerStateTransitionEvent.Transition_To_Evade, PlayerStateName.Evade);
            
            playerFSM.SetStartState(PlayerStateName.Locomotion_NotAiming);
            playerFSM.Init();
            
        }

        //Item Related
        void Trigger_OnPlayerInput_ItemUsePerform() => playerFSM.OnAction(PlayerStateTransitionEvent.ActiveItemUsePerform);
        void Trigger_OnPlayerInput_ItemUseCanceled() => playerFSM.OnAction(PlayerStateTransitionEvent.ActiveItemUseCanceled);
        void Trigger_OnPlayerInput_ItemSwitchPerformLR(int lrDirection) => playerFSM.OnAction(PlayerStateTransitionEvent.TriggerItemSwitchStart);
        void Trigger_Item_SwitchFinished(ItemBase itemSwitchedTo) => playerFSM.OnAction(PlayerStateTransitionEvent.TriggerItemSwitchFinished);
        void Trigger_Item_PickUp() => playerFSM.OnAction(PlayerStateTransitionEvent.TriggerItemPickUp);
        void Trigger_Item_Drop() => playerFSM.OnAction(PlayerStateTransitionEvent.TriggerItemDrop);
        
        //Target Related
        void Trigger_OnPlayerTargetSwitchButtonPressed()
        {
            // DebugX.LogWithColorCyan("Switching with button single");
            
            playerFSM.OnAction(PlayerStateTransitionEvent.TargetSwitchButtonPressed);
        }

        void Trigger_OnPlayerTargetSwitchRSFlick() => playerFSM.OnAction(PlayerStateTransitionEvent.TargetSwitchRSFlick);
        void Trigger_OnPlayerTargetSwitchLR() => playerFSM.OnAction(PlayerStateTransitionEvent.TargetSwitchLR);

    }
}
