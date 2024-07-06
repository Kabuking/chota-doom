using Modules.Common.Abilities.Base.model;
using Modules.Loadout.Scripts.Item;
using Modules.Player.Scripts.Components.TargetAssist;
using Modules.Player.Scripts.InputSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.Player.Scripts.ComponentEventBus
{
    public class PlayerComponentEventBus: MonoBehaviour
    {
        // public UnityAction<Transform> OnFoundEnemy;
        public UnityAction<ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons> DoPerformTargetSwitchOnButtonPressed;

        //Item_Relate
        public UnityAction<ItemBase> ItemSwitchFinished = delegate {  };
        public UnityAction ItemPickUpAttempt = delegate {  };               //Triggered by State Machine | Received by PickupManager
        public UnityAction ItemDropAttempt = delegate {  };               //Triggered by State Machine | Received by PickupManager
        public UnityAction<ItemBase> ItemDoPickUpProcess = delegate {  };   //Triggered by PickupManager | Received by ItemManager

        
        //Ability Relate
        // public UnityAction<PlayerInputMapping.AbilityTriggeredInput> PerformAbility = delegate {  };              
        // public UnityAction StopActiveAbilityProcessing = delegate {  };
        
        // public UnityAction<AbilityType> ExplicitAbilityPerform = delegate {  };              

    }
}