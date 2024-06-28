namespace Modules.Player.Scripts.PlayerStateMachine.model
{
    public enum PlayerStateTransitionEvent
    {
        /*CombatIdle_To_Run,
        CombatIdle_To_Dash,
        CombatIdle_To_AbilityMode,
        
        Run_To_CombatIdle,
        Run_To_Dash,
        Run_To_AbilityMode,
        
        Dash_To_Idle,
        Dash_To_Dash,
        
        AbilityMode_To_Idle,
        
        TakingDamage_To_Idle,
        
        Dying_To_Idle,
        Dying_To_Dead,*/
        
        Transition_To_LocomotionNotAiming,
        Transition_To_LocomotionAiming,
        Transition_To_Evade,
        
        
        Transition_To_Roll,
        Transition_To_CombatIdle,
        Transition_To_Run,
        Transition_To_Sprint,
        Transition_To_PaceControl,
        Transition_To_Dash,
        Transition_To_Reload,
        Transition_To_TakingDamage,
        Transition_To_Test,
        
        Any_To_TakingDamage,
        Any_To_Dying,
        
        
        //TriggerActions
        ActiveItemUsePerform,
        ActiveItemUseCanceled,
        ItemEquip_Start,
        ItemEquip_Finish,
        ItemUnequip_Start,
        ItemUnequip_Finish,
        TriggerItemSwitchStart,
        TriggerItemSwitchFinished,
        TriggerItemPickUp,
        TriggerItemDrop,
        
        TargetSwitchButtonPressed,
        TargetSwitchRSFlick,
        TargetSwitchLR,
        // TriggerWeaponTwo,
        ShootableItemNearby,
        EvadePerform,
        PaceControlPerform,
        PaceControlCanceled,
        
        //InputTriggers
        Input_StartedMovement,
        Input_StopMovement
    }
}
