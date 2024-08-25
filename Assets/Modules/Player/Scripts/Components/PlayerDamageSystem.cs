using System;
using System.Collections;
using Characters.Player.Global;
using Modules.Common;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.InputSystem;
using Modules.Player.Scripts.PlayerStateMachine.model;
using Unity.VisualScripting;
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

        public bool coolDownFinished = true;

        //Refs
        private PCharacterMovement characterMovement;
        private PlayerController _playerController;
        private PlayerInputMapping _playerInputMapping;
        private CharacterController _characterController;
        
        [SerializeField] private Renderer playerRender;
        [SerializeField] private Material aliveMaterial;
        [SerializeField] private Material deadMaterial;
        
        public void SetAliveMaterial() => playerRender.material = aliveMaterial;
        public void SetDeadMaterial() => playerRender.material = deadMaterial;
        
        
        private void Awake()
        {
            characterMovement =  transform.root.GetComponent<PCharacterMovement>();
            _playerController =  transform.root.GetComponent<PlayerController>();
            _playerInputMapping = transform.root.GetComponent<PlayerInputMapping>();
            _characterController = transform.root.GetComponent<CharacterController>();
            
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
            
            //Damage stagger
            _characterController.SimpleMove( new Vector3(
                                                 lastDamageDirection.x,
                                                 0, 
                                                 lastDamageDirection.y) 
                                             * staggerSpeed);

            
            if (currentHealth <= 0)
            {
                _playerController.TriggerDead();
            }
        }

        public void OnDamageUpdate()
        {
            //characterMovement.ApplyXZVelocityWithoutMovement(staggerSpeed, lastDamageDirection);
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
                DebugX.LogWithColorCyan("Found trigger Enemy Damage");

                    
                transform.root.GetComponent<CharacterController>()
                    .SimpleMove( new Vector3(
                        other.transform.position.z,
                        0, 
                        other.transform.position.x) 
                                 * staggerSpeed);
                
                
                /*if (_playerController.currentStateName != PlayerStateName.Crouch)
                {
                    
                    BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                    lastDamageDirection = new Vector2(other.transform.position.z, other.transform.position.x);
                    TriggerTakeDamage(lastDamageDirection, bulletBase.damageType);

                }*/
            }
        }
    }
}