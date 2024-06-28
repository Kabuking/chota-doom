using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.Player.Scripts.PlayerStateMachine.model
{
    public enum PlayerStateName
    {
        Locomotion_NotAiming,
        Evade,
        
        Locomotion_Aiming,
        IDLE_JOGGING_SPRINTING,
        CombatIdle,
        Jogging,
        Sprint,
        PaceControl,
        Dash,
        Roll,
        Reload,
        TakingDamage,
        Dying,
        Dead,
        TEST
    }
    
    
    public enum LocomotionNotAiming_Substate
    {
        Standing,
        Jogging,
    }

    public enum LocomotionNotAiming_CombatSubstate
    {
        Idle,
        UsingItem,
        EquippingItem,
        UnequipingItem
    }
    
    public enum PlayerAnimationName
    {
        Idle,
        CombatIdle,
        Jogging,
        Sprinting,
        Roll,
        Reload,
        GettingHurt
    }

    public abstract class StateDependentBehaviourMap: MonoBehaviour
    {
        public Dictionary<PlayerStateName, UnityAction> behaviourMap = new Dictionary<PlayerStateName, UnityAction>();
        protected void InitBehaviourMap()
        {
            behaviourMap.Add(PlayerStateName.CombatIdle, () => OnCombatIdleAiming());
            behaviourMap.Add(PlayerStateName.PaceControl, () => OnCombatIdleAiming());
            behaviourMap.Add(PlayerStateName.Dash, () => OnCombatIdleNotAiming());
            behaviourMap.Add(PlayerStateName.Jogging, () => OnRun());
            behaviourMap.Add(PlayerStateName.Sprint, () => OnSprint());
        }

        protected abstract void OnCombatIdleAiming();
        protected abstract void OnCombatIdleNotAiming();
        protected abstract void OnRun();
        protected abstract void OnSprint();
    }
    
    [Serializable]
    public class PlayerAnimationData
    {
        public PlayerAnimationName _animationName;
        public AnimationClip animationClip;
    }
}
