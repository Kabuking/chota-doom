using Modules.Common.Abilities.Base.model;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Crouch
{
    [CreateAssetMenu(fileName = "AbilityConfig",menuName = "Ability/Player/CrouchConfig")]
    public class CrouchConfigSo: AbilityConfigSo
    {
        public float crouchHeight = 0.5f;
        public float crouchMovementSpeed = 1f;
        public float crouchDuration = 1f;
    }
}