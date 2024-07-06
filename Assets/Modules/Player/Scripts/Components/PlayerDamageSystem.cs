using System;
using System.Collections;
using Characters.Player.Global;
using Modules.Player.Scripts.Controller;
using Modules.Player.Scripts.InputSystem;
using UnityEngine;

namespace Modules.Player.Scripts.Components
{
    public class PlayerDamageSystem: MonoBehaviour
    {
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



        void TriggerTakeDamage(Vector2 damageDirection)
        {
            DebugX.LogWithColorYellow("Taking damage");
            IsTakingDamage = true;
            lastDamageDirection = damageDirection;
        }
        
        // Logic Start
        public void OnDamageEnter()
        {
            coolDownFinished = false;
            IsTakingDamage = true;

            StartCoroutine(HurtCoolDown());
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
        
        //Operations
        void ProcessTakeDamage(int damageAmount, Vector3 damageDirection)
        {
            
        }
    }
}