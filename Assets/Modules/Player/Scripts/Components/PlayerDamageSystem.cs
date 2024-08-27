using System.Collections;
using Characters.Player.Global;
using Modules.Common;
using Modules.Level;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.InputSystem;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.Player.Scripts.Components
{
    public class PlayerDamageSystem: ADamageable
    {
        public float respawnTimerAfterDead = 2f;
        
        //states
        [SerializeField] private float staggerSpeed;
        [SerializeField] private float staggerOnLaserMultiplier = 1.2f;

        [SerializeField] private int maxHealth;
        [SerializeField] private float hurtCoolDown;
        [SerializeField] private float hurtDuration;

        [Space(10)]
        [SerializeField] private int currentHealth;
        [SerializeField] private Vector3 lastDamageDirection;
        [SerializeField] private int lastDamageValue;

        public bool coolDownFinished = true;

        [SerializeField] private GameObject playerCollapseMesh;

        //Refs
        private PlayerController _playerController;
        private PlayerInputMapping _playerInputMapping;
        private CharacterController _characterController;
        
        [SerializeField] private Renderer playerRender;
        [SerializeField] private Material aliveMaterial;
        [SerializeField] private Material deadMaterial;
        
        public void SetAliveMaterial() => playerRender.material = aliveMaterial;
        public void SetDeadMaterial() => playerRender.material = deadMaterial;


        public UnityAction<int> OnHealthNumberUpdate;
        
        private void Awake()
        {
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

        public bool IsTakingDamage = false;
        public BulletBase.DamageType lastDamageType;


        void TriggerTakeDamage(Vector3 damageDirection, BulletBase.DamageType damageType, int damageValue)
        {
            if (coolDownFinished)
            {
                // DebugX.LogWithColorYellow("Actia;lll Taking damage "+damageValue);
                IsTakingDamage = true;
                lastDamageDirection = damageDirection;
                lastDamageType = damageType;
                lastDamageValue = damageValue;
            }
        }
        
        // Logic Start
        public void OnDamageEnter()
        {
            coolDownFinished = false;
            IsTakingDamage = true;
            switch (lastDamageType)
            {
                case BulletBase.DamageType.Normal:
                    currentHealth = Mathf.Clamp(currentHealth - lastDamageValue,0,maxHealth );
                    ApplyStagger();
                    OnHealthNumberUpdate.Invoke(currentHealth);
                    break;
                case BulletBase.DamageType.Laser:
                    currentHealth = Mathf.Clamp(currentHealth - lastDamageValue,0,maxHealth );
                    ApplyStagger(staggerOnLaserMultiplier);
                    OnHealthNumberUpdate?.Invoke(currentHealth);
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

        public void OnDamageUpdate() { }
        public void OnDamageExit(){ IsTakingDamage = false; }
        
        //Logic End
        IEnumerator HurtCoolDown()
        {
            yield return new WaitForSeconds(hurtDuration);
            //Get back to normal state
            IsTakingDamage = false;
            yield return new WaitForSeconds(hurtCoolDown);
            coolDownFinished = true;
        }
        
        public override void TakeBulletDamage(BulletBase iamBullet)
        {
            base.TakeBulletDamage(iamBullet);
            if (iamBullet.gameObject.CompareTag(TagNames.EnemyDamage))
            {
                if (_playerController.currentStateName != PlayerStateName.Crouch)
                {
                    Vector3 staggerDirection = (transform.position - iamBullet.transform.position).normalized;
                    //staggerDirection = new Vector3(staggerDirection.x, transform.position.y, staggerDirection.z);
                    staggerDirection = - transform.forward;

                    TriggerTakeDamage(staggerDirection,iamBullet.damageType, iamBullet.damage);
                }
            }
        }

        public override void TakeLaserDamage(BulletBase.DamageType damageType, int damageValue)
        {
            base.TakeLaserDamage(damageType, damageValue);
            
            // Debug.Log("Player taking laser");
            TriggerTakeDamage(- transform.forward, damageType, damageValue);
            
            
            /*if (iamBullet.gameObject.CompareTag(TagNames.EnemyDamage))
            {
                if (_playerController.currentStateName != PlayerStateName.Crouch)
                {
                    Vector3 staggerDirection = (transform.position - iamBullet.transform.position).normalized;
                    //staggerDirection = new Vector3(staggerDirection.x, transform.position.y, staggerDirection.z);
                    staggerDirection = - transform.forward;

                    TriggerTakeDamage(staggerDirection,damageType, damageValue);
                }
            }*/
        }
        
        void ApplyStagger(float staggerSpeedExtraMultipler = 1)
        {
            _characterController
                .SimpleMove( lastDamageDirection
                             * staggerSpeed * staggerSpeedExtraMultipler);
        }

        public void SpawnPlayerCollapse()
        {
            Instantiate(playerCollapseMesh, transform.position, transform.rotation);
            gameObject.SetActive(false);
            
            LevelEvents.OnePlayerDead.Invoke();
        }
    }
}