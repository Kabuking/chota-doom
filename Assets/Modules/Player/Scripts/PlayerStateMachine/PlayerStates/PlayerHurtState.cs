using Modules.Common.Abilities.Base.model;
using Modules.Player.Scripts.Components;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityHFSM;

namespace Modules.Player.Scripts.PlayerStateMachine.PlayerStates
{
    public class PlayerHurtState: PlayerStateBase
    {
        private PlayerDamageSystem _playerDamageSystem;
        
        public PlayerHurtState(StateMachine<PlayerStateName, PlayerStateTransitionEvent> playerStateMachine, PlayerController controller) : base(playerStateMachine, controller)
        {
            _playerDamageSystem = _playerAbilityManager.GetComponentInChildren<PlayerDamageSystem>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _playerDamageSystem.OnDamageEnter();
        }

        public override void OnLogic()
        {
            base.OnLogic();
            _playerDamageSystem.OnDamageUpdate();
            if (ExitConditionFromHurt()) { }
        }

        public override void OnExit()
        {
            base.OnExit();
            _playerDamageSystem.OnDamageExit();
        }
    }
}
