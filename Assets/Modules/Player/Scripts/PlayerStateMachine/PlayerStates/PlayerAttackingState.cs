using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityHFSM;

namespace Modules.Player.Scripts.PlayerStateMachine.PlayerStates
{
    public class PlayerAttackingState:PlayerStateBase
    {
        public PlayerAttackingState(StateMachine<PlayerStateName, PlayerStateTransitionEvent> playerStateMachine, PlayerController controller) : 
            base(playerStateMachine, controller)
        {
        }
    }
}