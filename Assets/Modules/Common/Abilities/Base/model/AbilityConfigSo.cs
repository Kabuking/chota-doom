using UnityEngine;

namespace Modules.Common.Abilities.Base.model
{
    public abstract class AbilityConfigSo: ScriptableObject
    {
        public AbilityType abilityType;
        public float cooldown;
    }
}