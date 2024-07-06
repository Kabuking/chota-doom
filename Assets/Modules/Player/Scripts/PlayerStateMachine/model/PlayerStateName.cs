using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.Player.Scripts.PlayerStateMachine.model
{
    public enum PlayerStateName
    {
        AbilityPerforming,
        Locomotion_NotAiming,
        Evade,
        Hurt,
        
        Locomotion_Aiming,
        IDLE_JOGGING_SPRINTING,
        Crouch,
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

    [Serializable]
    public class PlayerAnimationData
    {
        public PlayerAnimationName _animationName;
        public AnimationClip animationClip;
    }
}
