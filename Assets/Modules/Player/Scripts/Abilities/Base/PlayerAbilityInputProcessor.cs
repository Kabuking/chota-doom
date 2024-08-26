using System.Collections.Generic;
using System.Linq;
using Modules.Common.Abilities.Base.model;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Base
{
    public class PlayerAbilityInputProcessor
    {
        // public List<AbilityTriggeredInputType> abilityInputListToPerform{ get; private set; }
        public Dictionary<AbilityTriggeredInputType, bool> abilityInputListToPerform{ get; private set; } = new Dictionary<AbilityTriggeredInputType, bool>();
        public int abilityListRemainingCount { get; private set; } = 0;

        
        //Receiving Ability Input
        // Delay check test player input vs adding to list
        //To remove from list
        public void AddAbilityToStack(AbilityTriggeredInputType abilityInput)
        {
            abilityInputListToPerform[abilityInput] = true;
            // abilityInputListToPerform.Add(abilityInput);
            UpdateAbilitiesRemaining();
        }

        void UpdateAbilitiesRemaining() =>  abilityListRemainingCount = abilityInputListToPerform.Values.Count(val => val);

        public void RemoveAnAbilityProcessed(int abilityIndexToRemove)
        {
            if (abilityListRemainingCount > 0)
            {
                AbilityTriggeredInputType? abilityTriggeredInputType =  GetAbilityImplFromIndex(abilityIndexToRemove);
                if (abilityTriggeredInputType.HasValue)
                {
                    abilityInputListToPerform[abilityTriggeredInputType.Value] = false;

                }
                // abilityInputListToPerform.Remove(0);
                UpdateAbilitiesRemaining();
                // Debug.Log("Ability removed to process ");
            }
        }

        public bool HasAbilityInStackToPerform() => abilityListRemainingCount > 0;
        // public int? RetrieveFirstAbilityInputFromStack() => GetAbilityImplIndexForInput(abilityInputListToPerform.FirstOrDefault());
        
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
        
        AbilityTriggeredInputType? GetAbilityImplFromIndex(int index)
        {
            switch (index)
            {
                case 0: return AbilityTriggeredInputType.Ability1;
                    break;
                case 1: return AbilityTriggeredInputType.Ability2;
                    break;
                case 2: return AbilityTriggeredInputType.Ability3;
                    break;
                case 3: return AbilityTriggeredInputType.Ability4;
                    break;
                default:
                    return null;
            }
        }

        public int? RetrieveFirstAbilityInputFromStack()
        {
            AbilityTriggeredInputType firstTrueKey = abilityInputListToPerform.FirstOrDefault(x => x.Value).Key;
            return GetAbilityImplIndexForInput(firstTrueKey);
        } 
 
    }
}
