using System;
using System.Collections;
using Characters.Player.Global;
using Modules.Common;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.InputSystem;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityEngine;

namespace Modules.Player.Scripts.Components
{
    public class PlayerDamageSystem: MonoBehaviour
    {
        public float respawnTimerAfterDead = 2f;
        
        //states
        [SerializeField] private float staggerSpeed;
        [SerializeField] private int maxHealth;
        [SerializeField] private float hurtCoolDown;
        [SerializeField] private float hurtDuration;

        [Space(10)]
        [SerializeField] private int currentHealth;
        [SerializeField] private Vector2 lastDamageDirection;

        public Vector2 damageDirection { get; private set; }
        public bool coolDownFinished { get; private set; } = true;

        //Refs
        private PCharacterMovement characterMovement;
        private PlayerController _playerController;
        private PlayerInputMapping _playerInputMapping;
        
        
        [SerializeField] private Renderer playerRender;
        [SerializeField] private Material aliveMaterial;
        [SerializeField] private Material deadMaterial;
        
        public void SetAliveMaterial() => playerRender.material = aliveMaterial;
        public void SetDeadMaterial() => playerRender.material = deadMaterial;
        
        
        private void Awake()
        {
            characterMovement = GetComponent<PCharacterMovement>();
            _playerController = GetComponent<PlayerController>();
            _playerInputMapping = GetComponent<PlayerInputMapping>();
            
            currentHealth = maxHealth;
        }

        private void OnEnable()
        {
            _playerInputMapping.TestTakeSelfDamage += TriggerTakeDamage;
        }

        private void OnDisable()
        {
            _playerInputMapping.TestTakeSelfDamage -= TriggerTakeDamage;
        }

        public bool IsTakingDamage { get; private set; }
        public BulletBase.DamageType lastDamageType;


        void TriggerTakeDamage(Vector2 damageDirection, BulletBase.DamageType damageType)
        {
            if (coolDownFinished)
            {
                // DebugX.LogWithColorYellow("Taking damage");
                IsTakingDamage = true;
                lastDamageDirection = damageDirection;
                lastDamageType = damageType;
            }
        }
        
        // Logic Start
        public void OnDamageEnter()
        {
            
            characterMovement.StopVelocityXYZ();
            
            coolDownFinished = false;
            IsTakingDamage = true;

            switch (lastDamageType)
            {
                case BulletBase.DamageType.Normal:
                    currentHealth = Mathf.Clamp(currentHealth - 1,0,maxHealth );
                    break;
                default:
                    break;
            }

            StartCoroutine(HurtCoolDown());
            
            if (currentHealth <= 0)
            {
                _playerController.TriggerDead();
            }
        }

        public void OnDamageUpdate()
        {
            characterMovement.ApplyXZVelocityWithoutMovement(staggerSpeed, lastDamageDirection);
        }
        
        public void OnDamageExit()
        {
            characterMovement.StopVelocityXYZ();
            IsTakingDamage = false;
        }
        
        //Logic End
        IEnumerator HurtCoolDown()
        {
            yield return new WaitForSeconds(hurtDuration);
            //Get back to normal state
            IsTakingDamage = false;
            yield return new WaitForSeconds(hurtCoolDown);
            coolDownFinished = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagNames.EnemyDamage))
            {
                // DebugX.LogWithColorCyan("Found trigger Enemy Damage");
                if (_playerController.currentStateName != PlayerStateName.Crouch)
                {
                    
                    BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                    Vector3 bulletDirection = bulletBase.rbProjectile.velocity.normalized;
                    Vector3 bulletPosition = other.transform.position;
                    Vector3 damageDirection = (transform.position - bulletPosition).normalized;
                    // DebugX.LogWithColorCyan("Damage taken player "+damageDirection);
                    TriggerTakeDamage(damageDirection, bulletBase.damageType);
                }
            }
        }
    }
}