using System.Collections.Generic;
using System.Linq;
using Modules.Common.Abilities.Base.model;

namespace Modules.Player.Scripts.Abilities.Base
{
    public class PlayerAbilityInputProcessor
    {
        private List<AbilityTriggeredInputType> abilityInputListToPerform;
        public int abilityListRemainingCount { get; private set; } = 0;


        public PlayerAbilityInputProcessor()
        {
            abilityInputListToPerform = new List<AbilityTriggeredInputType>();
        }
        
        //Receiving Ability Input
        // Delay check test player input vs adding to list
        //To remove from list
        public void AddAbilityToStack(AbilityTriggeredInputType abilityInput)
        {
            abilityInputListToPerform.Add(abilityInput);
            abilityListRemainingCount = abilityInputListToPerform.Count;
        }

        public void RemoveAnAbilityProcessed()
        {
            if (abilityListRemainingCount > 0)
            {
                abilityInputListToPerform.Remove(0);
                abilityListRemainingCount = abilityInputListToPerform.Count;
            }
        }

        public bool HasAbilityInStackToPerform() => abilityListRemainingCount > 0;
        public int? RetrieveFirstAbilityInputFromStack() => GetAbilityImplIndexForInput(abilityInputListToPerform.FirstOrDefault());
        
        int? GetAbilityImplIndexForInput(AbilityTriggeredInputType abilityTriggeredInputType)
        {
            switch (abilityTriggeredInputType)
            {
                case AbilityTriggeredInputType.Ability1: return 0;
                    break;
                case AbilityTriggeredInputType.Ability2:
                    if (HasAbilityInStackToPerform()) return 1;
                    break;
                case AbilityTriggeredInputType.Ability3:
                    if (HasAbilityInStackToPerform()) return 2;
                    break;
                case AbilityTriggeredInputType.Ability4:
                    if (HasAbilityInStackToPerform()) return 3;
                    break;
                default:
                    return -1;
            }

            return -1;
        }
    }
}
