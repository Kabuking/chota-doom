using System.Collections.Generic;
using Modules.Common.Abilities.Base.model;
using Modules.Player.Scripts.Abilities.Crouch;
using Modules.Player.Scripts.Abilities.Dash;
using Modules.Player.Scripts.Abilities.Gunjammer;
using Modules.Player.Scripts.Abilities.Skillshot;
using Modules.Player.Scripts.Abilities.Stomp;
using UnityEngine;

namespace Modules.Common.Abilities.Base.utils
{
    public static class PlayerAbilityAdder
    {
        public static List<AbilityBase> AddDefaultAbility(Transform pTransform, List<AbilityConfigSo> activeAbilityConfig)
        {
            List<AbilityBase> allAbilityListAdded = new List<AbilityBase>();
            
            foreach (var abilityConfigSo in activeAbilityConfig)
            {
                switch (abilityConfigSo.abilityType)
                {
                    case AbilityType.Crouch:
                        allAbilityListAdded.Add(new PlayerCrouchAbility(pTransform, abilityConfigSo));
                        break;
                    default:
                        break;
                }
            }

            return allAbilityListAdded;
        }
        
        public static List<AbilityBase> AddAllAbility(Transform pTransform, List<AbilityConfigSo> activeAbilityConfig)
        {
            List<AbilityBase> allAbilityListAdded = new List<AbilityBase>();
            
            foreach (var abilityConfigSo in activeAbilityConfig)
            {
                switch (abilityConfigSo.abilityType)
                {
                    case AbilityType.Dash:
                        allAbilityListAdded.Add(new PlayerDashAbility(pTransform, abilityConfigSo));
                        break;
                    case AbilityType.Skillshot:
                        allAbilityListAdded.Add(new PSkillShotAbility(pTransform, abilityConfigSo));
                        break;
                    case AbilityType.GunJam:
                        allAbilityListAdded.Add(new PGunJammer(pTransform, abilityConfigSo));
                        break;
                    case AbilityType.Stomp:
                        allAbilityListAdded.Add(new PStompAbility(pTransform, abilityConfigSo));
                        break;
                    default:
                        break;
                }
            }

            return allAbilityListAdded;
        }
    }
}
