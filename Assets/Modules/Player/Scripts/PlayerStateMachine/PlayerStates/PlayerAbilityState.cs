using Characters.Player.Global;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityHFSM;

namespace Modules.Player.Scripts.PlayerStateMachine.PlayerStates
{
    public class PlayerAbilityState: PlayerStateBase
    {
        public PlayerAbilityState(StateMachine<PlayerStateName, PlayerStateTransitionEvent> playerStateMachine, PlayerController controller) : base(playerStateMachine, controller)
        {
            // _playerAbilityState = 
        }

       public override void OnEnter()
       {
           base.OnEnter();
           _playerAbilityManager.AbilityApplyOnStart();
           // DebugX.LogWithColorYellow("Ability started "+_playerAbilityManager.GetAbilityToBeProcessed());
       }

       public override void OnLogic()
       {
           base.OnLogic();
           _playerAbilityManager.AbilityOnUpdate();
           
           if(_playerAbilityManager.LastAbilityPerformFinished())
               ForceTransitionTo_LocomotionNotAiming();
       }

       public override void OnExit()
       {
           base.OnExit();
           _playerAbilityManager.AbilityApplyOnExit();
           // DebugX.LogWithColorYellow("Ability finished ");
       }
    }
}
