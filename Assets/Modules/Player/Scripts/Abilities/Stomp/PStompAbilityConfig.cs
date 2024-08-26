using Modules.Common.Abilities.Base.model;
using Unity.VisualScripting;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Stomp
{
    [CreateAssetMenu(fileName = "AbilityConfig",menuName = "Ability/Player/Stomp")]
    public class PStompAbilityConfig: AbilityConfigSo
    {
        public GameObject stompRing;
    }
}