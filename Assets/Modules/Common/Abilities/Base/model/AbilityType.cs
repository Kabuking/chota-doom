namespace Modules.Common.Abilities.Base.model
{
    public enum AbilityType
    {
        Crouch,
        Dash,
        Skillshot,
        GunJam,
        Stomp,
        Teleport,
        Clone,
        SelfHurt
    }
    
    public enum AbilityProcessingState{ Received, Processing, Empty }
    
    public enum AbilityTriggeredInputType{ Ability1, Ability2, Ability3, Ability4, Ability5, None}
}