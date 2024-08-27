using System;
using System.Collections.Generic;
using Modules.Common.Abilities.Base.model;
using UnityEngine;

namespace Modules.Level
{
    public static class PlayThroughState
    {
        public static Dictionary<AbilityTriggeredInputType, bool> unlockedAbilities = new Dictionary<AbilityTriggeredInputType, bool>();

        // public List<bool> allAbilitiesUnlockState;
    }
}