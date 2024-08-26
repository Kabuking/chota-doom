using System.Collections;
using Modules.Common.Abilities.Base.model;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Stomp
{
    public class PStompAbility: AbilityBase
    {
        private Transform spawnPosition;
        private PStompAbilityConfig _stompAbilityConfig;
        private MonoBehaviour _monoBehaviour;
        
        public PStompAbility(Transform characterTransform, AbilityConfigSo incomingAbilityConfigSo) : base(characterTransform, incomingAbilityConfigSo)
        {
            spawnPosition = characterTransform.Find("PlayerAbilities").Find("StompPosition");
            _stompAbilityConfig = incomingAbilityConfigSo as PStompAbilityConfig;
            _monoBehaviour = characterTransform.root.GetComponent<MonoBehaviour>();
        }
        
        public override void AbilityOnStart()
        {
            base.AbilityOnStart();
            Debug.Log("Spawn grass whoosh on "+spawnPosition.position);
            Object.Instantiate(_stompAbilityConfig.stompRing, spawnPosition.position, spawnPosition.rotation);
            _monoBehaviour.StartCoroutine(StompAbilityCoolDown());
            // Debug.Log("Stomp ability triggered");
        }

        public override void AbilityOnExit()
        {
            // Debug.Log("Stomp ability triggered finished"+spawnPosition.name);
            base.AbilityOnExit();
        }
        
        IEnumerator StompAbilityCoolDown()
        {
            yield return new WaitForSeconds(_stompAbilityConfig.abilityExitDuration);
            SetAbilityPerformFinished();
            yield return new WaitForSeconds(_stompAbilityConfig.cooldown);
            SetCoolDownToFinished();
        }
    }
}