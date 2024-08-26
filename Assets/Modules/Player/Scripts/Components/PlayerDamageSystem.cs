using System.Collections;
using Modules.Common;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.InputSystem;
using Modules.Player.Scripts.PlayerStateMachine.model;
using UnityEngine;

namespace Modules.Player.Scripts.Components
{
    public class PlayerDamageSystem: ADamageable
    {
        public float respawnTimerAfterDead = 2f;
        
        //states
        [SerializeField] private float staggerSpeed;
        [SerializeField] private int maxHealth;
        [SerializeField] private float hurtCoolDown;
        [SerializeField] private float hurtDuration;

        [Space(10)]
        [SerializeField] private int currentHealth;
        [SerializeField] private Vector3 lastDamageDirection;

        public bool coolDownFinished = true;

        //Refs
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


        void TriggerTakeDamage(Vector3 damageDirection, BulletBase.DamageType damageType)
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
            ApplyStagger();
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
                    staggerDirection = new Vector3(staggerDirection.x, transform.position.y, staggerDirection.z);
                    TriggerTakeDamage(staggerDirection,iamBullet.damageType);
                }
            }
        }

        void ApplyStagger()
        {
            _characterController
                .SimpleMove( lastDamageDirection
                             * staggerSpeed);
        }
    }
}