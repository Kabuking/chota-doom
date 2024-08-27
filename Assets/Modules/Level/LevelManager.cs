using System;
using System.Collections;
using Modules.Common.Abilities.Base.model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.Level
{
    public class LevelManager: MonoBehaviour
    {
        public float levelReloadDelay = 2;

        public string nextLevelToLoad;
        
        private AbilityType _abilityTypeToUnlock;

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

            SceneManager.LoadScene(nextLevelToLoad);
        }
        
        
        IEnumerator ReloadLevel()
        {
            //show some ui
            
            
            yield return new WaitForSeconds(levelReloadDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}