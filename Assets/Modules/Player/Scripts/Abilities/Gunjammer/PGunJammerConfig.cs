using Modules.Common.Abilities.Base.model;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Gunjammer
{
    [CreateAssetMenu(fileName = "AbilityConfig",menuName = "Ability/Player/PGunJam")]
    public class PGunJammerConfig: AbilityConfigSo
    {
        public float enemyGunJammingDuration;
    }
}