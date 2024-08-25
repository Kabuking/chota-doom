using System.Collections;
using Modules.Common.Abilities.Base.model;
using Modules.Player.Scripts.Components;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Dash
{
    public class PlayerDashAbility: AbilityBase
    {
        private CharacterController _characterController;
        private DashConfig _dashConfig;

        private MonoBehaviour _monoBehaviour;
        private PCharacterMovement _characterMovement;
        
        public PlayerDashAbility(Transform characterTransform, AbilityConfigSo incomingAbilityConfigSo) : base(characterTransform, incomingAbilityConfigSo)
        {
            _characterController = characterTransform.GetComponent<CharacterController>();
            _dashConfig = incomingAbilityConfigSo as DashConfig;
            _monoBehaviour = characterTransform.GetComponent<MonoBehaviour>();
            _characterMovement = characterTransform.GetComponent<PCharacterMovement>();
        }

        public override void AbilityOnStart()
        {
            base.AbilityOnStart();
            _monoBehaviour.StartCoroutine(DashCoolDown());
        }

        public override void AbilityOnUpdate()
        {
            base.AbilityOnUpdate();
            _characterMovement.ApplyEvade(_dashConfig.dashSpeed);
        }

        public override void AbilityOnExit()
        {
            base.AbilityOnExit();
            _characterMovement.StopVelocityXYZ();
        }

        IEnumerator DashCoolDown()
        {
            yield return new WaitForSeconds(_dashConfig.dashDuration);
            SetAbilityPerformFinished();
            yield return new WaitForSeconds(_dashConfig.cooldown);
            SetCoolDownToFinished();
        }
    }
}
