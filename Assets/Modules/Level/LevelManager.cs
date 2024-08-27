using System;
using System.Collections;
using System.Collections.Generic;
using Modules.Common.Abilities.Base.model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.Level
{
    public class LevelManager: MonoBehaviour
    {
        [SerializeField] private Transform theBoss;
        public float bossSpawnDelay = 60;
        public float levelReloadDelay = 5;

        public string nextLevelToLoad;
        
        private AbilityType _abilityTypeToUnlock;

        private void Start()
        {
            StartCoroutine(BoosSpawnSpawnAfter());
        }

        private void OnEnable()
        {
            LevelEvents.OnePlayerDead += OnOnePlayerDead;
            LevelEvents.LevelDefeated += OnLevelDefeated;

        }

        private void OnDisable()
        {
            LevelEvents.OnePlayerDead -= OnOnePlayerDead;
            LevelEvents.LevelDefeated -= OnLevelDefeated;

        }

        void OnOnePlayerDead()
        {
            StartCoroutine(ReloadLevel());
        }

        void OnLevelDefeated(AbilityTriggeredInputType abilityTypeToUnlock)
        {
            PlayThroughState.unlockedAbilities[abilityTypeToUnlock] = true;

            StartCoroutine(ReloadLevel());
        }
        
        
        IEnumerator ReloadLevel()
        {
            //show some ui
            
            
            yield return new WaitForSeconds(levelReloadDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        IEnumerator BoosSpawnSpawnAfter()
        {
            yield return new WaitForSeconds(bossSpawnDelay);
            theBoss.gameObject.SetActive(true);
        }
    }
}