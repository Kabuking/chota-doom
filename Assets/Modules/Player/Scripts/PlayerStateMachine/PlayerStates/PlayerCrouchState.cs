using Modules.Common.Abilities.Base.model;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityHFSM;

namespace Modules.Player.Scripts.PlayerStateMachine.PlayerStates
{
    public class PlayerCrouchState: PlayerStateBase
    {
        // private AbilityBase _abilityBase;
        
        public PlayerCrouchState(StateMachine<PlayerStateName, PlayerStateTransitionEvent> playerStateMachine, PlayerController controller) : base(playerStateMachine, controller)
        {
            // _abilityBase = _playerAbilityManager.GetAbility(AbilityType.Crouch);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            crouchAbility.AbilityOnStart();
        }

        public override void OnLogic()
        {
            base.OnLogic();
            crouchAbility.AbilityOnUpdate();

            if (CheckAndTriggerTransitionTo_Hurt()) { }
            else if (TransitionForAbilityCheck()) { }
            else if(crouchAbility.abilityPerformFinished)
                ForceTransitionTo_LocomotionNotAiming();
        }

        public override void OnExit()
        {
            base.OnExit();
            crouchAbility.AbilityOnExit();
        }
    }
}
