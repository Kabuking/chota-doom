using System.Collections;
using Modules.Common.Abilities.Base.model;
using Modules.Loadout.Scripts.Guns;
using Modules.Player.Scripts.Abilities.Dash;
using Modules.Player.Scripts.Components;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Gunjammer
{
    public class PGunJammer: AbilityBase
    {
        private CharacterController _characterController;
        private PGunJammerConfig gunJammerConfig;

        private MonoBehaviour _monoBehaviour;
        private PCharacterMovement _characterMovement;
        
        public PGunJammer(Transform characterTransform, AbilityConfigSo incomingAbilityConfigSo) : base(characterTransform, incomingAbilityConfigSo)
        {
            _characterController = characterTransform.GetComponent<CharacterController>();
            gunJammerConfig = incomingAbilityConfigSo as PGunJammerConfig;
            _monoBehaviour = characterTransform.GetComponent<MonoBehaviour>();
            _characterMovement = characterTransform.GetComponent<PCharacterMovement>();
        }

        public override void AbilityOnStart()
        {
            base.AbilityOnStart();
            _monoBehaviour.StartCoroutine(GunJamCoolDown());
            Debug.Log("Jammer ability triggered");
        }

        public override void AbilityOnExit()
        {
            base.AbilityOnExit();
            _characterMovement.StopVelocityXYZ();
        }

        IEnumerator GunJamCoolDown()
        {
            
            var allGuns = GameObject.FindObjectsOfType<SlowProjectile>();

            foreach (var enemyGun in allGuns)
            {
                Debug.Log("Jamming ");
                enemyGun.enabled = false;
            }
            yield return new WaitForSeconds(gunJammerConfig.abilityExitDuration);
            SetAbilityPerformFinished();

            yield return new WaitForSeconds(gunJammerConfig.enemyGunJammingDuration);
            foreach (var enemyGun in allGuns)
            {
                if(enemyGun!=null)
                    enemyGun.enabled = true;
            }

            yield return new WaitForSeconds(gunJammerConfig.cooldown);
            SetCoolDownToFinished();
            Debug.Log("Player= Gun Jam cooldown finished");
        }

    }
}
