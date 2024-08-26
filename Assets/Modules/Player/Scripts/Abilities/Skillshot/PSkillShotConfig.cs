using Modules.Common.Abilities.Base.model;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Skillshot
{
    [CreateAssetMenu(fileName = "AbilityConfig",menuName = "Ability/Player/SkillShot")]
    public class PSkillShotConfig: AbilityConfigSo
    {
        public GameObject projectleSpawn;
    }
}