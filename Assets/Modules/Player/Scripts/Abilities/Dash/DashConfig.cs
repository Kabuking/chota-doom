using Modules.Common.Abilities.Base.model;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Dash
{
    [CreateAssetMenu(fileName = "AbilityConfig",menuName = "Ability/Player/DashConfig")]
    public class DashConfig: AbilityConfigSo
    {
        public float dashDuration;
        public float dashSpeed;
    }
}