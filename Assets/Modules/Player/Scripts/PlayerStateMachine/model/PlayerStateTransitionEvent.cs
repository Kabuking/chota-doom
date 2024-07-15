namespace Modules.Player.Scripts.PlayerStateMachine.model
{
    public enum PlayerStateTransitionEvent
    {
        Transition_To_LocomotionNotAiming,
        Transition_To_LocomotionAiming,
        Transition_To_Evade,
        Transition_To_Crouch,
        Transition_To_Ability,
        Transition_To_Hurt,
        Transition_To_Dead,
        
        
        Transition_To_CombatIdle,
        Transition_To_Run,
        Transition_To_Reload,
        Transition_To_Test,
        
        // Any_To_TakingDamage,
        // Any_To_Dead,
        
        
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
        TriggerCrouchPerform,
        TriggerCrouchCancel,

        TargetSwitchButtonPressed,
        TargetSwitchRSFlick,
        TargetSwitchLR,

    }
}
