using Characters.Player.Global;
using Modules.CommonEventBus;
using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Manager;
using Modules.Player.Scripts.Abilities.Base;
using Modules.Player.Scripts.ComponentEventBus;
using Modules.Player.Scripts.InputSystem;
using Modules.Player.Scripts.PlayerData;
using Modules.Player.Scripts.PlayerStateMachine.model;
using Modules.Player.Scripts.PlayerStateMachine.PlayerStates;
using UnityEngine;
using UnityHFSM;

namespace Modules.Player.Scripts.Controller
{
    [RequireComponent(typeof(PlayerComponentEventBus))]
    public class PlayerController: MonoBehaviour
    {
        public PlayerStats playerStats;
        private StateMachine<PlayerStateName, PlayerStateTransitionEvent> playerFSM;
        [SerializeField] public PlayerStateName currentStateName;
        public bool enableTickOnStateLogic = true;
        
        //References
        [SerializeField] private PlayerCoopEventBus _playerCoopEventBus;
        
        protected GameplayLoadout gameplayLoadout;
        PlayerInputMapping _playerInputMapping;
        private PlayerGameplayStatsState _playerGameplayStatsState;
        private PlayerComponentEventBus _playerComponentEventBus;
        private PlayerAbilityManager _playerAbilityManager;
        private GameplayLoadoutOnPlayerV2 _gameplayLoadoutOnPlayer;
            
        private void Awake()
        {
            // _playerWeaponManager = GetComponent<PlayerWeaponManager>();
            // _inventoryManager = GetComponent<InventoryManager>();

            _playerComponentEventBus = GetComponent<PlayerComponentEventBus>();
            _playerInputMapping = GetComponent<PlayerInputMapping>();
            gameplayLoadout = GetComponent<GameplayLoadout>();
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
            _playerInputMapping.CrouchPerformedEvent += Trigger_CrouchPerform;
            _playerInputMapping.CrouchCanceledEvent += Trigger_CrouchCanceled;

            _playerComponentEventBus.ItemSwitchFinished += Trigger_Item_SwitchFinished;
            
            //Target Related
            _playerInputMapping.TargetSwitchSingledButtonPressedEvent += Trigger_OnPlayerTargetSwitchButtonPressed;
            _playerInputMapping.TargetSwitchRSFlickEvent += Trigger_OnPlayerTargetSwitchRSFlick;
            _playerInputMapping.TargetSwitchLRPressedEvent += Trigger_OnPlayerTargetSwitchLR;
            
            // _playerComponentEventBus.Invoke();

            /*_playerInputMapping.Ability1EventPerformed += Trigger_Ability1Perform;
            _playerInputMapping.Ability2EventPerformed += Trigger_Ability2Perform;
            _playerInputMapping.Ability3EventPerformed += Trigger_Ability3Perform;
            _playerInputMapping.Ability4EventPerformed += Trigger_Ability4Perform;
            _playerInputMapping.Ability5EventPerformed += Trigger_Ability5Perform;*/

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
            _playerInputMapping.CrouchPerformedEvent -= Trigger_CrouchPerform;
            _playerInputMapping.CrouchCanceledEvent -= Trigger_CrouchCanceled;
            
            //Target Related
            _playerInputMapping.TargetSwitchSingledButtonPressedEvent -= Trigger_OnPlayerTargetSwitchButtonPressed;
            _playerInputMapping.TargetSwitchRSFlickEvent -= Trigger_OnPlayerTargetSwitchRSFlick;
            _playerInputMapping.TargetSwitchLRPressedEvent -= Trigger_OnPlayerTargetSwitchLR;
            
            /*_playerInputMapping.Ability1EventPerformed -= Trigger_Ability1Perform;
            _playerInputMapping.Ability2EventPerformed -= Trigger_Ability2Perform;
            _playerInputMapping.Ability3EventPerformed -= Trigger_Ability3Perform;
            _playerInputMapping.Ability4EventPerformed -= Trigger_Ability4Perform;
            _playerInputMapping.Ability5EventPerformed -= Trigger_Ability5Perform;*/

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
            _playerAbilityManager = GetComponent<PlayerAbilityManager>();
            _gameplayLoadoutOnPlayer = GetComponent<GameplayLoadoutOnPlayerV2>();
            // _gameplayLoadoutOnPlayer.InitLoadOut();
            
            _playerAbilityManager.InitializeAbilityManager();
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

            /*//Evade_State
            PlayerEvadeState playerEvadeState = new PlayerEvadeState(playerFSM, this);
            playerFSM.AddState(
                PlayerStateName.Evade, 
                playerEvadeState
                    .AddAction_TargetSwitchButtonPressed()
                    .AddAction_TargetSwitchRSFlicked()
            );*/
            
            //Crouch State
            PlayerCrouchState playerCrouchState = new PlayerCrouchState(playerFSM, this);
            playerFSM.AddState(
                PlayerStateName.Crouch, 
                playerCrouchState
                    .AddAction_TargetSwitchButtonPressed()
                    .AddAction_TargetSwitchRSFlicked()
                    .AddAction_ItemUse_PerformAndCancel()
            );
            
            //Ability State
            PlayerAbilityState playerAbilityState = new PlayerAbilityState(playerFSM, this);
            playerFSM.AddState(
                PlayerStateName.AbilityPerforming,
                playerAbilityState
            );

            //Hurt State
            PlayerHurtState playerHurtState = new PlayerHurtState(playerFSM, this);
            playerFSM.AddState(
                PlayerStateName.Hurt,
                playerHurtState
            );
            
            
            PlayerDeadState playerDeadState = new PlayerDeadState(playerFSM, this);
            playerFSM.AddState(
                PlayerStateName.Dead,
                playerDeadState
            );
            
            
            playerFSM.AddTriggerTransitionFromAny(PlayerStateTransitionEvent.Transition_To_Hurt, PlayerStateName.Hurt);
            playerFSM.AddTriggerTransitionFromAny(PlayerStateTransitionEvent.Transition_To_Dead, PlayerStateName.Dead);

            
            
            playerFSM.AddTriggerTransitionFromAny(PlayerStateTransitionEvent.Transition_To_LocomotionNotAiming, PlayerStateName.Locomotion_NotAiming);
            playerFSM.AddTriggerTransitionFromAny(PlayerStateTransitionEvent.Transition_To_Evade, PlayerStateName.Evade);
            playerFSM.AddTriggerTransitionFromAny(PlayerStateTransitionEvent.Transition_To_Crouch, PlayerStateName.Crouch);
            playerFSM.AddTriggerTransitionFromAny(PlayerStateTransitionEvent.Transition_To_Ability, PlayerStateName.AbilityPerforming);

            
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
        void Trigger_CrouchPerform() => playerFSM.OnAction(PlayerStateTransitionEvent.TriggerCrouchPerform);
        void Trigger_CrouchCanceled() => playerFSM.OnAction(PlayerStateTransitionEvent.TriggerCrouchCancel);


        //Ability Triggers
        /*void Trigger_Ability1Perform() => playerFSM.OnAction(PlayerStateTransitionEvent.Ability1);
        void Trigger_Ability2Perform() => playerFSM.OnAction(PlayerStateTransitionEvent.Ability2);
        void Trigger_Ability3Perform() => playerFSM.OnAction(PlayerStateTransitionEvent.Ability3);
        void Trigger_Ability4Perform() => playerFSM.OnAction(PlayerStateTransitionEvent.Ability4);
        void Trigger_Ability5Perform() => playerFSM.OnAction(PlayerStateTransitionEvent.Ability5);*/

        
        
        //Target Related
        void Trigger_OnPlayerTargetSwitchButtonPressed()
        {
            // DebugX.LogWithColorCyan("Switching with button single");
            
            playerFSM.OnAction(PlayerStateTransitionEvent.TargetSwitchButtonPressed);
        }

        void Trigger_OnPlayerTargetSwitchRSFlick() => playerFSM.OnAction(PlayerStateTransitionEvent.TargetSwitchRSFlick);
        void Trigger_OnPlayerTargetSwitchLR() => playerFSM.OnAction(PlayerStateTransitionEvent.TargetSwitchLR);
        
        
        //External Trigger
        /*public void TriggerTakeDamage(Vector2 damageDirection)
        {
            playerFSM.OnAction(PlayerStateTransitionEvent.Transition_To_Hurt);
        }*/
        
        public void TriggerDead()
        {
            // DebugX.LogWithColorCyan("Transition dead");
            playerFSM.TriggerLocally(PlayerStateTransitionEvent.Transition_To_Dead);
        }
    }
}
