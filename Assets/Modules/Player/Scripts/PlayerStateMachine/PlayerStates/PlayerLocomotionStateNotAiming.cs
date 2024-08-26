using Characters.Player.Global;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityHFSM;

namespace Modules.Player.Scripts.PlayerStateMachine.PlayerStates
{
    public class PlayerLocomotionStateNotAiming: PlayerStateBase
    {
        private LocomotionNotAiming_Substate _locomotionState;
        private LocomotionNotAiming_CombatSubstate _combatState;
        
        public PlayerLocomotionStateNotAiming(StateMachine<PlayerStateName, PlayerStateTransitionEvent> playerStateMachine, PlayerController controller) : base(playerStateMachine, controller)
        {
            _locomotionState = LocomotionNotAiming_Substate.Standing;
            _combatState = LocomotionNotAiming_CombatSubstate.Idle;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnLogic()
        {
            base.OnLogic();
            
            HandleMovement(_playerStats.jogging_OnNotAiming);
            
            if (CheckAndTriggerTransitionTo_Hurt()) { }
            else if (TransitionForAbilityCheck()) {}
            else if (CheckTransitionTo_Crouch()) { }
        }

        void HandleMovement(float speed)
        {

            
            if (CheckIfInput_Jogging())
            {
                characterMovement.ApplyXZVelocity(speed);
                UpdateLocomotionStateTo(LocomotionNotAiming_Substate.Jogging);
            }/*else if (CheckIfInput_Sprint())
            {
                // ApplyXZVelocity(playerGameplayState.sprintSpeed);
            }*/
            else
            {
                UpdateLocomotionStateTo(LocomotionNotAiming_Substate.Standing);
                // StopVelocityXYZ();
            }
            
            characterMovement.ApplyLookTowardEnemy();
        }

        /// <summary>
        /// Below Functions to update current substate identifiers
        /// </summary>
        //Combat State State
        protected override void OnInput_ActiveItemUse()
        {
            base.OnInput_ActiveItemUse();
            UpdateCombatStateTo(LocomotionNotAiming_CombatSubstate.UsingItem);
        }

        protected override void OnInput_ItemUseStop()
        {
            base.OnInput_ItemUseStop();
            UpdateCombatStateTo(LocomotionNotAiming_CombatSubstate.Idle);
        }

        //Locomotion State Update
        void UpdateCombatStateTo(LocomotionNotAiming_CombatSubstate newCombatState)
        {
            _combatState = newCombatState;
        }

        void UpdateLocomotionStateTo(LocomotionNotAiming_Substate newLocomotionState)
        {
            _locomotionState = newLocomotionState;
        }


        /// <summary>
        /// Below functions are events triggered from external components and below implementation is how the state will
        /// respond to those events
        /// </summary>
        protected override void OnItemSwitch_Finish()
        {
            base.OnItemSwitch_Finish();
        }

        protected override void OnInput_ItemSwitch_Trigger()
        {
            base.OnInput_ItemSwitch_Trigger();
        }
    }
}