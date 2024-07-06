using System.Collections;
using Characters.Player.Global;
using Modules.Common.Abilities.Base.model;
using Modules.Player.Scripts.Components;
using Modules.Player.Scripts.InputSystem;
using UnityEngine;

namespace Modules.Player.Scripts.Abilities.Crouch
{
    public class PlayerCrouchAbility: AbilityBase
    {
        private CrouchConfigSo _crouchConfig;
        private CharacterController _characterController;
        private float initialCrouchHeight;

        private MonoBehaviour _monoBehaviour;

        private PCharacterMovement _characterMovement;
        private PlayerInputMapping _playerInputMapping;
        
        void SetCrouchHeight()
        {
            _characterController.height = _crouchConfig.crouchHeight;
            standingMesh.gameObject.SetActive(false);
            crouchMesh.gameObject.SetActive(true);
        }

        void ResetCrouchHeight()
        {
            _characterController.height = initialCrouchHeight;
            crouchMesh.gameObject.SetActive(false);
            standingMesh.gameObject.SetActive(true);
        }


        //Prototype
        //Carefull it is hard coded
        private Transform standingMesh;
        private Transform crouchMesh;
        
        
        public PlayerCrouchAbility(Transform characterTransform, AbilityConfigSo incomingAbilityConfigSo) : base(characterTransform, incomingAbilityConfigSo)
        {
            
            _crouchConfig = incomingAbilityConfigSo as CrouchConfigSo;
            _characterController = characterTransform.GetComponent<CharacterController>();
            initialCrouchHeight = _characterController.height;
            _monoBehaviour = characterTransform.GetComponent<MonoBehaviour>();
            _characterMovement = characterTransform.GetComponent<PCharacterMovement>();
            _playerInputMapping = characterTransform.GetComponent<PlayerInputMapping>();

            standingMesh = characterTransform.GetChild(0).GetChild(0);
            crouchMesh = characterTransform.GetChild(0).GetChild(1);
        }

        public override void AbilityOnStart()
        {
            base.AbilityOnStart();
            SetCrouchHeight();

            _monoBehaviour.StartCoroutine(CoolDownCrouch());
            
            // DebugX.LogWithColorYellow("Crouch ability Started");
        }

        public override void AbilityOnExit()
        {
            base.AbilityOnExit();
            ResetCrouchHeight();
            // DebugX.LogWithColorYellow("Crouch ability exiting");
        }

        public override void AbilityOnUpdate()
        {
            base.AbilityOnUpdate();

            HandleMovement(_crouchConfig.crouchMovementSpeed);
        }

        IEnumerator CoolDownCrouch()
        {
            // DebugX.LogWithColorYellow("Crouch Cooldown started");
            yield return new WaitForSeconds(_crouchConfig.crouchDuration);
            SetAbilityPerformFinished();
            yield return new WaitForSeconds(_crouchConfig.cooldown);
            // DebugX.LogWithColorYellow("Crouch Cooldown finished");
            SetCoolDownToFinished();
        }
        
                
        void HandleMovement(float speed)
        {
            if (CheckIfInput_Jogging())
            {
                _characterMovement.ApplyXZVelocity(speed);
            }
            else
            {
                _characterMovement.StopVelocityXYZ();
            }
            _characterMovement.ApplyLookTowardEnemy();
        }
        
        protected bool CheckIfInput_Jogging()
        {
            if (_playerInputMapping.incomingMovementVector != Vector2.zero) return true;
            else return false;
        }
    }
}