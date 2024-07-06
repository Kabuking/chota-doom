using System.Collections.Generic;
using System.Linq;
using Characters.Player.Global;
using Modules.Common.Abilities.Base.model;
using Modules.Common.Abilities.Base.utils;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Base
{
    public class AbilityStackProcessor
    {
        private PlayerAbilityManager _playerAbilityManager;
        private Transform playerTransform;
        private PlayerAbilityInputProcessor _playerAbilityInputProcessor;
        public List<AbilityBase> abilityImplList { get; private set; }
        public List<AbilityBase> abilityImplListDefault { get; private set; }
        public AbilityBase _currentAbilityPerforming { get; private set; }

        //Initialization
        public AbilityStackProcessor(MonoBehaviour playerAbilityManager)
        {
            _playerAbilityManager = playerAbilityManager as PlayerAbilityManager;
            playerTransform = playerAbilityManager.GetComponent<Transform>(); //Considering PlayerAbilityManager is Monobehaviour

            if (_playerAbilityManager is not null)
            {
                abilityImplList = PlayerAbilityAdder.AddAllAbility(playerTransform, _playerAbilityManager.startingAbilityList);
                abilityImplListDefault = PlayerAbilityAdder.AddDefaultAbility(playerTransform, _playerAbilityManager.defaultAbilities); 
            }
            else
            {
                DebugX.LogWithColorYellow("PlayerAbilityManager not attached to player");
            }

            _playerAbilityInputProcessor = new PlayerAbilityInputProcessor();
        }
        
        
        
        public AbilityBase GetAbilityImplFromAbilityType(AbilityType abilityType) => abilityImplList.FirstOrDefault(a => a.abilityConfigSo.abilityType == abilityType);
        public AbilityBase GetAbilityDefaultFromAbilityType(AbilityType abilityType) => abilityImplListDefault.FirstOrDefault(a => a.abilityConfigSo.abilityType == abilityType);


        public void AddIncomingAbilityInputToStack(AbilityTriggeredInputType abilityInput) =>
            _playerAbilityInputProcessor.AddAbilityToStack(abilityInput);

        public bool CheckIfAbilityCanBePerformed()
        {
            if (_playerAbilityInputProcessor.HasAbilityInStackToPerform())
            {
                int? abilityToPerformIndex = _playerAbilityInputProcessor.RetrieveFirstAbilityInputFromStack();
                if (abilityToPerformIndex != null)
                {
                    if (abilityImplList.Count > abilityToPerformIndex)
                    {
                        AbilityBase abilityBase = abilityImplList[abilityToPerformIndex.Value];
                        return (abilityBase.AbilityCanBePerformed()) ;
                    }
                }
            }
            return false;
        }
        
        public void SetActiveAbility(AbilityBase abilityBase)
        {
            _currentAbilityPerforming = abilityBase;
        }

        public void AbilityPerformOnStart()
        {
            
            int? abilityToPerformIndex = _playerAbilityInputProcessor.RetrieveFirstAbilityInputFromStack();
            if (abilityToPerformIndex != null)
            {
                if (abilityImplList.Count > abilityToPerformIndex)
                {
                    AbilityBase abilityBase = abilityImplList[abilityToPerformIndex.Value];
                    SetActiveAbility(abilityBase);
                    _currentAbilityPerforming.AbilityOnStart();
                }
                else
                {
                    DebugX.LogWithColorYellow("Ability has no implementation");
                }
            }
            else
            {
                DebugX.LogWithColorYellow("No Input Ability yet, This should not happen");
            }

            _playerAbilityInputProcessor.RemoveAnAbilityProcessed();
        }
    }
}
