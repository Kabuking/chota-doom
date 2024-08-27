using Modules.Common.Abilities.Base.model;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Skillshot
{
    [CreateAssetMenu(fileName = "AbilityConfig",menuName = "Ability/Player/SkillShot")]
    public class PSkillShotConfig: AbilityConfigSo
    {
        public GameObject projectleSpawn;
        public float damageValue = 20;

        public float laserRadius = 3;
        public float laserLength = 1000;
        public LayerMask enemyLayerMask;

    }
}