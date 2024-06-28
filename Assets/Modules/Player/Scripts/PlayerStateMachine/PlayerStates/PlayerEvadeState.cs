using System.Collections;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityEngine;
using UnityHFSM;

namespace Modules.Player.Scripts.PlayerStateMachine.PlayerStates
{
    public class PlayerEvadeState: PlayerStateBase
    {
       public PlayerEvadeState(StateMachine<PlayerStateName, PlayerStateTransitionEvent> playerStateMachine, PlayerController.PlayerController controller) : base(playerStateMachine, controller)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            playerController.StartCoroutine(EvadeTimerCheck());
            // DebugX.LogWithColor("Entered Evade state ",Color.red);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            characterMovement.ApplyEvade();
        }

        public override void OnExit()
        {
            base.OnExit();
            
            characterMovement.StopVelocityXYZ();
            // DebugX.LogWithColor("Entered EXIT state ",Color.red);
        }
        
        IEnumerator EvadeTimerCheck()
        {
            playerGameplayState.DisableAbleToPerformEvade();
            yield return new WaitForSeconds(playerGameplayState.playerStats.evadeLengthInSecs);
            ForceTransitionTo_LocomotionNotAiming();
            yield return new WaitForSeconds(playerGameplayState.playerStats.evadeCoolDown);
            playerGameplayState.EnableAbleToPerformEvade();
        }
        
    }
}