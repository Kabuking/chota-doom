using Modules.Common.Abilities.Base.model;
using UnityEngine.Events;

namespace Modules.Level
{
    public static class LevelEvents
    {
        public static UnityAction OnePlayerDead = delegate {  };
        
        public static UnityAction<AbilityTriggeredInputType> LevelDefeated = delegate {  };

    }
}