using System;
using System.Collections.Generic;
using Modules.Common.Abilities.Base.model;
using Modules.Level;
using Modules.Player.Scripts.InputSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Player.Scripts.Abilities.Base
{
    public class PlayerAbilityManager: MonoBehaviour
    {
        [SerializeField] private bool debugMode;
        
        //Currently as Ability 1,2,3..
        [Tooltip("Unlockable Abilities")]
        [SerializeField] private List<AbilityTriggeredInputType> unlockedAbilities;
        
        [Tooltip("All possible Abilities can perform in cach")]
        public List<AbilityConfigSo> abilityCache;
        
        
        [Tooltip("Abilities which will be there by default")]
        public List<AbilityConfigSo> defaultAbilities;
        
        //Refs
        private PlayerInputMapping _playerInputMapping;
        
        //Processors
        private AbilityStackProcessor _abilityStackProcessor;


        //Call first before state machine init
        public void InitializeAbilityManager()
        {
            if (!debugMode)
            {
                //According to play
                foreach (var (abilityType, abilityStatus) in PlayThroughState.unlockedAbilities)
                {
                    if (abilityStatus && !unlockedAbilities.Contains(abilityType))
                    {
                        unlockedAbilities.Add(abilityType);
                    }
                }
            }

            _playerInputMapping = GetComponent<PlayerInputMapping>();
            // _playerComponentEventBus = transform.GetComponent<PlayerComponentEventBus>();
            _abilityStackProcessor = new AbilityStackProcessor(this);
            
            
            _playerInputMapping.AbilityTriggerEventPerformed += AddAbilityToStack;
        }

        /*private void OnEnable()
        {
            _playerInputMapping.AbilityTriggerEventPerformed += AddAbilityToStack;
        }*/

        private void OnDisable()
        {
            _playerInputMapping.AbilityTriggerEventPerformed -= AddAbilityToStack;
        }

        //Receive input
        //TODO To not take everything
        void AddAbilityToStack(AbilityTriggeredInputType inputAbility)
        {
            
            //Todo: To perform check before adding
            // inputAbility.
            if (unlockedAbilities.Contains(inputAbility))
            {
                _abilityStackProcessor.AddIncomingAbilityInputToStack(inputAbility);
            }
        }

        public bool CanIPerformAbilityAbility() => _abilityStackProcessor.CheckIfAbilityCanBePerformed();
        public void AbilityApplyOnStart() => _abilityStackProcessor.AbilityPerformOnStart();
        public void AbilityOnUpdate() =>_abilityStackProcessor._currentAbilityPerforming.AbilityOnUpdate();
        public void AbilityApplyOnExit() =>_abilityStackProcessor._currentAbilityPerforming.AbilityOnExit();

        public AbilityBase GetAbility(AbilityType abilityType) => _abilityStackProcessor.GetAbilityImplFromAbilityType(abilityType);
        public AbilityBase GetDefaultAbility(AbilityType abilityType) => _abilityStackProcessor.GetAbilityDefaultFromAbilityType(abilityType);

        public bool LastAbilityPerformFinished() => _abilityStackProcessor._currentAbilityPerforming.abilityPerformFinished;
        
        
        //Debugging
        public AbilityType GetAbilityToBeProcessed() => _abilityStackProcessor._currentAbilityPerforming.abilityConfigSo.abilityType;


        [SerializeField] private int abilitiesToPerformInStack;
        private void Update()
        {
            abilitiesToPerformInStack = _abilityStackProcessor._playerAbilityInputProcessor.abilityInputListToPerform.Count;
        }
    }
}
