﻿using System.Collections;
using Modules.Common.Abilities.Base.model;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Skillshot
{
    public class PSkillShotAbility: AbilityBase
    {
        private Transform spawnPosition;
        private PSkillShotConfig _skillShotConfig;
        private MonoBehaviour _monoBehaviour;
        
        public PSkillShotAbility(Transform characterTransform, AbilityConfigSo incomingAbilityConfigSo) : base(characterTransform, incomingAbilityConfigSo)
        {
            spawnPosition = characterTransform.Find("PlayerAbilities").Find("SkillShotPosition");
            _skillShotConfig = incomingAbilityConfigSo as PSkillShotConfig;
            _monoBehaviour = characterTransform.root.GetComponent<MonoBehaviour>();
        }
        
        public override void AbilityOnStart()
        {
            base.AbilityOnStart();
            Debug.Log("Skill shot ability triggered "+spawnPosition.name);
            Object.Instantiate(_skillShotConfig.projectleSpawn, spawnPosition.position, spawnPosition.rotation, spawnPosition);
            
            
            
            _monoBehaviour.StartCoroutine(SkillShotCoolDown());
        }

        
        IEnumerator SkillShotCoolDown()
        {
            yield return new WaitForSeconds(_skillShotConfig.rampUp);

            SphereCastLaser();
            
            yield return new WaitForSeconds(_skillShotConfig.abilityExitDuration);
            SetAbilityPerformFinished();
            yield return new WaitForSeconds(_skillShotConfig.cooldown);
            SetCoolDownToFinished();
        }
        
        
        void SphereCastLaser()
        {
            Physics.queriesHitTriggers = true;
            Vector3 startPosition = spawnPosition.position;
            Vector3 direction = spawnPosition.forward;
            RaycastHit hitInfo;
            
            if (Physics.SphereCast(startPosition, _skillShotConfig.laserRadius, direction, out hitInfo, _skillShotConfig.laserLength, _skillShotConfig.enemyLayerMask))
            {
                EnemyHealth eH = hitInfo.collider.gameObject.GetComponent<EnemyHealth>();
                eH.TakeDamage(_skillShotConfig.damageValue);
                eH.Stagger(-(spawnPosition.position - eH.transform.position).normalized);
            }
        }
    }
}