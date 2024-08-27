using System.Collections;
using Characters.Player.Global;
using Modules.Common.Abilities.Base.model;
using Modules.Player.Scripts.Components;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityEngine;
using UnityHFSM;

namespace Modules.Player.Scripts.PlayerStateMachine.PlayerStates
{
    public class PlayerDeadState: PlayerStateBase
    {
        private PlayerDamageSystem _playerDamageSystem;
        public PlayerDeadState(StateMachine<PlayerStateName, PlayerStateTransitionEvent> playerStateMachine, PlayerController controller) : base(playerStateMachine, controller)
        {
            _playerDamageSystem = _playerAbilityManager.GetComponent<PlayerDamageSystem>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            DebugX.LogWithColorGreen($"Respawning....in {_playerDamageSystem.respawnTimerAfterDead} seconds");
            
            
            _playerDamageSystem.SetDeadMaterial();
            _playerDamageSystem.StartCoroutine(RespawnTimer());
        }

        public override void OnLogic()
        {
            base.OnLogic();
            
            //Apply movement
        }

        public override void OnExit()
        {
            base.OnExit();
            _playerDamageSystem.SetAliveMaterial();

        }

        IEnumerator RespawnTimer()
        {
            yield return new WaitForSeconds(_playerDamageSystem.respawnTimerAfterDead);
            ForceTransitionTo_LocomotionNotAiming();
        }
    }
}
