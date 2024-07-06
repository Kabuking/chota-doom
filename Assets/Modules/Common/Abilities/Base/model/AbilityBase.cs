using UnityEngine;

namespace Modules.Common.Abilities.Base.model
{
    public abstract class AbilityBase
    {
        public bool coolDownFinished { get; private set; } = true;
        public bool abilityPerformFinished { get; private set; } = false;

        public AbilityConfigSo abilityConfigSo;
        protected readonly Transform _characterTransform;
        
        public AbilityBase(Transform characterTransform, AbilityConfigSo incomingAbilityConfigSo)
        {
            _characterTransform = characterTransform;
            abilityConfigSo = incomingAbilityConfigSo;
        }

        public virtual void AbilityOnStart()
        {
            coolDownFinished = false;
            abilityPerformFinished = false;
        }
        public virtual void AbilityOnUpdate(){ }

        public virtual void AbilityOnExit()
        {
        }

        public virtual bool AbilityCanBePerformed()
        {
            return (coolDownFinished == true);
        }

        public void SetCoolDownToFinished() => coolDownFinished = true;
        public void SetAbilityPerformFinished() => abilityPerformFinished = true;
    }
}
