using Modules.Common.Abilities.Base.model;
using Modules.Loadout.Scripts.Manager;
using Modules.Player.Scripts.Abilities.Base;
using Modules.Player.Scripts.ComponentEventBus;
using Modules.Player.Scripts.Components;
using Modules.Player.Scripts.Components.TargetAssist;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.InputSystem;
using Modules.Player.Scripts.PlayerData;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityEngine;
using UnityHFSM;

namespace Modules.Player.Scripts.PlayerStateMachine.PlayerStates
{
    public abstract class PlayerStateBase: State<PlayerStateName, PlayerStateTransitionEvent>
    {
        protected StateMachine<PlayerStateName, PlayerStateTransitionEvent> _playerStateMachine;
        protected PCharacterMovement characterMovement;
        protected readonly PlayerInputMapping playerInputMapping;
        protected GameplayLoadoutOnPlayer _itemManager;

        protected readonly PlayerComponentEventBus _playerComponentEventBus;
        
        // protected playerGameplayState _playerGameplayState;

        protected PlayerStats _playerStats;
        protected PlayerGameplayStatsState playerGameplayState;
        protected PlayerController playerController;


        protected PlayerAbilityManager _playerAbilityManager;

        private PlayerDamageSystem _playerDamageSystem;
        
        //Might not be initialized
        protected AbilityBase crouchAbility;
        
        public PlayerStateBase(StateMachine<PlayerStateName, PlayerStateTransitionEvent> playerStateMachine, PlayerController controller)
        {
            _playerStateMachine = playerStateMachine;
            characterMovement = controller.GetComponent<PCharacterMovement>();
            playerInputMapping = controller.GetComponent<PlayerInputMapping>();
            _itemManager = controller.GetComponent<GameplayLoadoutOnPlayer>();
            playerGameplayState = controller.GetComponent<PlayerGameplayStatsState>();
            _playerStats = controller.playerStats;
            playerController = controller;
            _playerComponentEventBus = controller.transform.root.GetComponent<PlayerComponentEventBus>();
            _playerAbilityManager = controller.GetComponent<PlayerAbilityManager>();
            _playerDamageSystem = controller.GetComponent<PlayerDamageSystem>();
                
            crouchAbility = _playerAbilityManager.GetDefaultAbility(AbilityType.Crouch);
        }
        public override void OnEnter()
        {
            base.OnEnter();
            playerController.enableTickOnStateLogic = true;
            // CommonOnStateEnterItemBehaviours();
        }

        public override void OnLogic()
        {
            base.OnLogic();
            CheckForTransition_OnLogic();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        
        #region Transitions_Check
        //=============================================TRANSITIONS======================================================
        
        protected virtual void CheckForTransition_OnLogic() { }
        
        protected void TriggerLocalTransition(PlayerStateTransitionEvent playerStateTransitionEvent)
        {

            playerController.enableTickOnStateLogic = false;
            _playerStateMachine.TriggerLocally(playerStateTransitionEvent);
            
        }
        
        protected void CheckAndTriggerTransitionTo_CombatIdle()
        {
            if (playerInputMapping.incomingMovementVector == Vector2.zero
                && !playerInputMapping.sprintPerforming)//TODO: Convert to event
            {
                TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_CombatIdle);
            }
        }

        protected bool TransitionForAbilityCheck()
        {
            if (_playerAbilityManager.CanIPerformAbilityAbility())
            {
                TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_Ability);
                return true;
            }
            else return false;
        } 
        
        protected bool CheckAndTriggerTransitionTo_Hurt()
        {
            if (_playerDamageSystem.IsTakingDamage && _playerDamageSystem.coolDownFinished)//TODO: Convert to event
            {
                TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_Hurt);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        protected bool ExitConditionFromHurt()
        {
            if (_playerDamageSystem.coolDownFinished)//TODO: Convert to event
            {
                TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_LocomotionNotAiming);
                return true;
            }
            return false;
        }
        
        protected void ForceTransitionTo_LocomotionAiming() => TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_LocomotionAiming);
        protected void ForceTransitionTo_LocomotionNotAiming() => TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_LocomotionNotAiming);
        
        
        // protected void ForceTransitionToLocomotion() => TriggerStateTransitionLocally(PlayerStateTransitionEvent.Transition_To_Locomotion);
        protected void ForceTransitionToCombatIdle() => TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_CombatIdle);
        protected void ForceTransitionToJogging() => TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_Run);
        protected void ForceTransitionToTest() => TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_Test);

        protected bool CheckTransitionTo_Jogging()
        {
            if (CheckIfInput_Jogging())
            {
                TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_Run);
                return true;
            }
            else return false;
        }

        protected bool CheckIfInput_Jogging()
        {
            /*if (playerInputMapping.incomingMovementVector != Vector2.zero
                && !playerInputMapping.paceControlPerforming
                && !playerInputMapping.sprintPerforming) return true;
            else return false;*/
            if (playerInputMapping.incomingMovementVector != Vector2.zero) return true;
            else return false;
        }

        /*protected bool CheckIfInput_Sprint()
        {
            if (playerInputMapping.incomingMovementVector != Vector2.zero
                && !playerInputMapping.paceControlPerforming
                && !playerInputMapping.itemUsePerforming
                && playerInputMapping.sprintPerforming
                && playerGameplayState.currentSprintTimeLeft >= 0.5f) return true;
            else return false;
        }*/
        /*protected bool CheckTransitionTo_Sprint()
        {
            if (CheckIfInput_Sprint())
            {
                TriggerStateTransitionLocally(PlayerStateTransitionEvent.Transition_To_Sprint);
                return true;
            }
            else
            {
                return false;
            }
        }*/
        

        protected bool CheckTransitionTo_Crouch()
        {
            if (playerInputMapping.crouchPerforming && crouchAbility.coolDownFinished)
            {
                // DebugX.LogWithColorCyan("Trigger transition to crouch");
                TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_Crouch);
                return true;
            }
            else return false;
        }

        
        protected bool CheckTransition_To_Aiming()
        {
            if (playerInputMapping.IsAiming)
            {
                TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_LocomotionAiming);
                return true;
            }
            else return false;
        }
        
        protected bool CheckTransition_To_NotAiming()
        {
            if (playerInputMapping.IsAiming == false)
            {
                TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_LocomotionNotAiming);
                return true;
            }
            else return false;
        }
        
        protected bool CheckTransitionTo_Reload()
        {
            if (playerInputMapping.performReload)
            {
                TriggerLocalTransition(PlayerStateTransitionEvent.Transition_To_Reload);
                return true;
            }
            else{
                return false;
            }
        }




        protected bool CanPerformAbility() => _playerAbilityManager.CanIPerformAbilityAbility();
        
        
        #endregion
        

        /*
         * Items Related
         * This actions/events are triggered when player is in some state
         */
        protected  virtual void OnInput_ActiveItemUse() => _itemManager.OnItemUse();
        protected  virtual void OnInput_ItemUseStop()=> _itemManager.OnItemUseStop();
        protected virtual void OnInput_ItemSwitch_Trigger() { _itemManager.ReceiveItemSwitchLeftRight(); }
        protected virtual void OnItemSwitch_Finish(){}
        protected virtual void OnInput_ItemPickup_Trigger() { _playerComponentEventBus.ItemPickUpAttempt.Invoke(); }
        protected virtual void OnInput_ItemDrop_Trigger() { _playerComponentEventBus.ItemDropAttempt.Invoke(); }

        protected virtual void OnItem_EquipStart(){ }
        protected virtual void OnItem_EquipFinish() { }
        protected virtual void OnItem_Unequip_Start() { }
        protected virtual void OnItem_Unequip_Finish() { }
        private PlayerStateBase AddAction_ItemUse_Perform() { AddAction(PlayerStateTransitionEvent.ActiveItemUsePerform, OnInput_ActiveItemUse); return this; }
        private PlayerStateBase AddAction_ItemUse_Cancel() { AddAction(PlayerStateTransitionEvent.ActiveItemUseCanceled, OnInput_ItemUseStop); return this; }
        public PlayerStateBase AddAction_ItemUse_PerformAndCancel() { AddAction_ItemUse_Perform(); AddAction_ItemUse_Cancel(); return this; }
        public PlayerStateBase AddAction_ItemPickUp() { AddAction(PlayerStateTransitionEvent.TriggerItemPickUp, OnInput_ItemPickup_Trigger); return this; }
        public PlayerStateBase AddAction_ItemDrop() { AddAction(PlayerStateTransitionEvent.TriggerItemDrop, OnInput_ItemDrop_Trigger); return this; }

        //TODO: Need to check if required
        public PlayerStateBase AddAction_ItemEquip_UnEquip()
        {
            AddAction(PlayerStateTransitionEvent.ItemEquip_Start, OnItem_EquipStart);
            AddAction(PlayerStateTransitionEvent.ItemEquip_Finish, OnItem_EquipFinish);
            AddAction(PlayerStateTransitionEvent.ItemUnequip_Start, OnItem_Unequip_Start);
            AddAction(PlayerStateTransitionEvent.ItemUnequip_Finish, OnItem_Unequip_Finish);
            return this;
        }

        public PlayerStateBase AddAction_ItemSwitch_PerformAndFinish()
        {
            AddAction(PlayerStateTransitionEvent.TriggerItemSwitchStart, OnInput_ItemSwitch_Trigger); 
            AddAction(PlayerStateTransitionEvent.TriggerItemSwitchFinished, OnItemSwitch_Finish);
            return this;
        }

        
        /*
         * Target Related Triggered
         */ 
        protected  virtual void OnInput_TargetSwitchSingleButtonPressed() => _playerComponentEventBus.DoPerformTargetSwitchOnButtonPressed?.Invoke(playerInputMapping.buttonTargetSwitchType);
        protected  virtual void OnInput_TargetSwitchRSFlicked() => _playerComponentEventBus.DoPerformTargetSwitchOnButtonPressed?.Invoke(ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons.RightStickFlick);
        protected  virtual void OnInput_TargetSwitchLR() => _playerComponentEventBus.DoPerformTargetSwitchOnButtonPressed?.Invoke(ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons.LeftRightSwitch);

        public PlayerStateBase AddAction_TargetSwitchButtonPressed() { AddAction(PlayerStateTransitionEvent.TargetSwitchButtonPressed, OnInput_TargetSwitchSingleButtonPressed); return this; }
        public PlayerStateBase AddAction_TargetSwitchRSFlicked() { AddAction(PlayerStateTransitionEvent.TargetSwitchRSFlick, OnInput_TargetSwitchRSFlicked); return this; }
        public PlayerStateBase AddAction_TargetSwitchLR() { AddAction(PlayerStateTransitionEvent.TargetSwitchLR, OnInput_TargetSwitchLR); return this; }


        // public PlayerStateBase  AddAction_Reload_Perform() { AddAction(PlayerStateTransitionEvent.ReloadGun, OnInput_WeaponReload); return this; }
        

        //Ability Related


        
        /*protected virtual void OnInput_Ability1_Trigger() { _playerComponentEventBus.PerformAbility?.Invoke(PlayerInputMapping.AbilityTriggeredInputType.Ability1); }
        protected virtual void OnInput_Ability2_Trigger() { _playerComponentEventBus.PerformAbility?.Invoke(PlayerInputMapping.AbilityTriggeredInputType.Ability2); }
        protected virtual void OnInput_Ability3_Trigger() {_playerComponentEventBus.PerformAbility?.Invoke(PlayerInputMapping.AbilityTriggeredInputType.Ability3); }
        protected virtual void OnInput_Ability4_Trigger() {_playerComponentEventBus.PerformAbility?.Invoke(PlayerInputMapping.AbilityTriggeredInputType.Ability4); }
        protected virtual void OnInput_Ability5_Trigger() { _playerComponentEventBus.PerformAbility?.Invoke(PlayerInputMapping.AbilityTriggeredInputType.Ability5); }


        public PlayerStateBase AddAction_Ability1() { AddAction(PlayerStateTransitionEvent.Ability1, OnInput_Ability1_Trigger); return this; }
        public PlayerStateBase AddAction_Ability2() { AddAction(PlayerStateTransitionEvent.Ability2, OnInput_Ability2_Trigger); return this; }
        public PlayerStateBase AddAction_Ability3() { AddAction(PlayerStateTransitionEvent.Ability3, OnInput_Ability3_Trigger); return this; }
        public PlayerStateBase AddAction_Ability4() { AddAction(PlayerStateTransitionEvent.Ability4, OnInput_Ability4_Trigger); return this; }
        public PlayerStateBase AddAction_Ability5() { AddAction(PlayerStateTransitionEvent.Ability5, OnInput_Ability5_Trigger); return this; }*/


        
    }
}
