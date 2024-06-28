using System;
using UnityEngine;

namespace Modules.Player.Scripts.PlayerData
{
    [Serializable]
    public class PlayerGameplayStatsState: MonoBehaviour
    {
        public PlayerStats playerStats;
        public enum NextShotType
        {
            None,
            DirectShot,
            AfterAim
        }

        public NextShotType nextShotType = NextShotType.None;
        //Before Gameplay
        public float paceControlSpeed;
        
        [Header("Walk")]
        public float jogging;

        [Header("Sprinting")]
        public float sprintSpeed;
        public bool sprinting = false;
        public float sprintTimeMax;
        public float sprintTimerDecreasingRate;
        public float sprintTimerDecreasingRateValue;
        public float sprintRecoveryDelay;
        public float sprintIncreasingRecoveryRate;
        public float sprintIncreasingRecoveryRateValue;
        public float currentSprintTimeLeft = 0;
        
        [Header("Dash")]
        public float evadeSpeed;
        public float evadeLengthInSecs;
        public float evadeCoolDown;

        public bool ableToPerformEvade = true;
        public void DisableAbleToPerformEvade() => ableToPerformEvade = false;
        public void EnableAbleToPerformEvade() => ableToPerformEvade = true;
    }
}
