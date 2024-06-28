using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Player.Scripts.PlayerData
{
        [CreateAssetMenu(fileName = "PlayerStats", menuName = "Gameplay/PlayerStats")]
        public class PlayerStats: ScriptableObject
        {
                [FormerlySerializedAs("jogging")] [FormerlySerializedAs("walk")] [Header("Movement")] 
                public float jogging_OnNotAiming = 10f;
                public float jogging_OnAiming = 5f;
                public float paceControlSpeed = 1f;
                
                [Header("Sprint")]
                public float sprintSpeed = 20f;
                public float sprintTimeMax = 3f;
                public float sprintRecoveryDelay = 2f;
                public float sprintTimerDecreasingRate = 0.5f;
                public float sprintTimerDecreasingRateValue = 1f;
                public float sprintTimerIncreasingRate = 0.3f;
                public float sprintTimerIncreasingRateValue = 1f;
                
                [FormerlySerializedAs("dashSpeed")] [Header("Rolling")]
                public float rollSpeed = 30f;
                public float dashLength = .5f;
                public float dashCoyoteCheckLength = .3f;
                public float dashCoolDown = .5f;
                public float startItemUseDelayFromSprint = 0.1f;
                public float iFrameDuration_AfterRoll = 0.3f;

                [Header("LookAround And Aiming")] 
                public float controllerDeadZone = 0.1f;
                public float rotateSmoothing = 10;
                public float directShotDelayFromNotAiming = 0.2f;
                public float stayInAimAfterShotDurationDelay = 0.3f;
                public float showLineOfSightDelayFromAim = 0.2f;
                public float showLineOfSightDelayAfterShooting = 0.2f;
                
                
                [Header("Health")]
                public float recoveryTimeFromTakingDamage = 0.8f;
                public float takingDamageInBetweenDuration = 0.02f;
                public float damageForce = 4f;
                public int maxPlayerHealth = 100;
                public bool shouldFaceEnemy = false;

                [Header("Evade")] 
                public float evadeVelocity;
                public float evadeLengthInSecs;
                public float evadeCoolDown;
        }
}
