﻿using System.Collections.Generic;
using Modules.Common.Abilities.Base.model;
using Modules.Player.Scripts.InputSystem;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Base
{
    public class PlayerAbilityManager: MonoBehaviour
    {
        public List<AbilityConfigSo> startingAbilityList;
        
        public List<AbilityConfigSo> defaultAbilities;
        
        //Refs
        private PlayerInputMapping _playerInputMapping;
        
        //Processors
        private AbilityStackProcessor _abilityStackProcessor;
        
        private void Awake()
        {
            _playerInputMapping = GetComponent<PlayerInputMapping>();
            // _playerComponentEventBus = transform.GetComponent<PlayerComponentEventBus>();
            _abilityStackProcessor = new AbilityStackProcessor(this);
        }

        private void OnEnable()
        {
            _playerInputMapping.AbilityTriggerEventPerformed += AddAbilityToStack;
        }

        private void OnDisable()
        {
            _playerInputMapping.AbilityTriggerEventPerformed -= AddAbilityToStack;
        }

        //Receive input
        //TODO To not take everything
        void AddAbilityToStack(AbilityTriggeredInputType inputAbility) => _abilityStackProcessor.AddIncomingAbilityInputToStack(inputAbility);

        public bool CanIPerformAbilityAbility() => _abilityStackProcessor.CheckIfAbilityCanBePerformed();
        public void AbilityApplyOnStart() => _abilityStackProcessor.AbilityPerformOnStart();
        public void AbilityOnUpdate() =>_abilityStackProcessor._currentAbilityPerforming.AbilityOnUpdate();
        public void AbilityApplyOnExit() =>_abilityStackProcessor._currentAbilityPerforming.AbilityOnExit();

        public AbilityBase GetAbility(AbilityType abilityType) => _abilityStackProcessor.GetAbilityImplFromAbilityType(abilityType);
        public AbilityBase GetDefaultAbility(AbilityType abilityType) => _abilityStackProcessor.GetAbilityDefaultFromAbilityType(abilityType);

        public bool LastAbilityPerformFinished() => _abilityStackProcessor._currentAbilityPerforming.abilityPerformFinished;
        
        
        //Debugging
        public AbilityType GetAbilityToBeProcessed() => _abilityStackProcessor._currentAbilityPerforming.abilityConfigSo.abilityType;

    }
}
