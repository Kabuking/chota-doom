using Modules.Common.Abilities.Base.model;
using Modules.Player.Scripts.Components.TargetAssist;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Modules.Player.Scripts.InputSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputMapping: MonoBehaviour
    {
        [Header("Target Switch or buttons Input Setup")] 
        public ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons buttonTargetSwitchType;
        public ManualSwitchAtEnemyInRange.SortingPriorityOnLeftRightSwitch sortingPriorityLeftRightSwitch = ManualSwitchAtEnemyInRange.SortingPriorityOnLeftRightSwitch.XYZ;
        
        
        [Space(10)]
        public GameInput gameplayInput;
        protected PlayerInput playerInput;
        
        public UnityAction ActiveItemUseEventPerformed = delegate {  };
        public UnityAction ActiveItemUseEventCanceled = delegate {  };
        public UnityAction<int> ItemSwitchSlotPerformed = delegate {  };
        public UnityAction<int> ItemSwitchLeftRightEventPerformed = delegate {  };
        public UnityAction TargetSwitchSingledButtonPressedEvent = delegate {  };
        public UnityAction TargetSwitchLRPressedEvent = delegate {  };

        public UnityAction ItemPickupEventPerformed = delegate {  };
        public UnityAction ItemDropEventPerformed = delegate {  };
        public UnityAction TargetSwitchRSFlickEvent = delegate {  };

        // public UnityAction WeaponSwitchEvent = delegate {  };
        public UnityAction ReloadGunPerformednEvent = delegate {  };
        public UnityAction ReloadGunCanceledEvent = delegate {  };

        public UnityAction CrouchPerformedEvent = delegate {  };
        public UnityAction CrouchCanceledEvent = delegate {  };

        public UnityAction<AbilityTriggeredInputType> AbilityTriggerEventPerformed = delegate {  };
        
        
        //Testing only
        public UnityAction<Vector3, BulletBase.DamageType> TestTakeSelfDamage = delegate {  };


        public Vector2 rollDirection = Vector2.zero;
        [Header("Move Input LS")]
        public Vector2 lastIncomingMovementVector;
        public Vector2 incomingMovementVector;

        [Header("Target Direction RSFlick")] 
        public Vector2 lastRsFlickDirection;
        public Vector2 currentRsFlickDirection;

        [FormerlySerializedAs("targetSwitchDirectionLR")]
        public float lastTargetSwitchDirectionLR = 0;
        
        public bool IsAiming;


        [Header("Item Switching")] 
        [SerializeField] public int lastItemSwitchDirection = 0;
        [SerializeField] public int lastItemSwitchSlot = 0;

        private InputAction _movementInputAction_IA;
        private InputAction _itemUseInputAction_IA;
        private InputAction _itemSwitchInputAction_IA;
        private InputAction _itemSwitchLRInputAction_IA;
        private InputAction _itemPickup_IA;
        private InputAction _itemDrop_IA;
        // private InputAction _evadeInputAction_IA;
        private InputAction _lastTargetSwitchRSFlick_IA;
        private InputAction _crouch_IA;


        private InputActionMap _targetSwitchSingleButton_IAM;
        private InputAction _targetSwitchButton_IA;

        private InputActionMap _targetSwitchLeftRight_IAM;
        private InputAction _targetSwitchLeftRight_IA;
        
        //Abilities
        private InputAction Ability1_IA;
        private InputAction Ability2_IA;
        private InputAction Ability3_IA;
        private InputAction Ability4_IA;
        private InputAction Ability5_IA;
        
        public bool sprintPerforming = false;
        public bool paceControlPerforming = false;
        public bool crouchPerforming = false;

        public bool itemUsePerforming = false;
        public bool performReload = false;

        private bool awakeFinished = false;

        //Testing
        private InputAction _selfDamage_IA;


        [Header("Abilities")]
        public AbilityTriggeredInputType abilityTriggeredInputType = AbilityTriggeredInputType.None;

        
        public enum PlayerControlScheme
        {
            GamePad,
            KBM
        }
        
        private void Awake()
        {
            
            playerInput = GetComponent<PlayerInput>();
            
            _movementInputAction_IA = playerInput.currentActionMap.FindAction(InputActionNames.Movement);
            _itemUseInputAction_IA = playerInput.currentActionMap.FindAction(InputActionNames.ItemUse);
            _itemSwitchInputAction_IA  = playerInput.currentActionMap.FindAction(InputActionNames.ItemSwitch);
            _itemSwitchLRInputAction_IA  = playerInput.currentActionMap.FindAction(InputActionNames.ItemSwitchLR);
            _lastTargetSwitchRSFlick_IA =  playerInput.currentActionMap.FindAction(InputActionNames.TargetSwitchRSFlick);
            _itemPickup_IA =  playerInput.currentActionMap.FindAction(InputActionNames.ItemPickup);
            _itemDrop_IA =  playerInput.currentActionMap.FindAction(InputActionNames.ItemDrop);
            _crouch_IA = playerInput.currentActionMap.FindAction(InputActionNames.Crouch);
            _targetSwitchButton_IA = playerInput.currentActionMap.FindAction(InputActionNames.TargetSwitchButton);
            _targetSwitchLeftRight_IA = playerInput.currentActionMap.FindAction(InputActionNames.TargetSwitchButtonLR);
            
            
            Ability1_IA = playerInput.currentActionMap.FindAction(InputActionNames.Ability1);
            Ability2_IA = playerInput.currentActionMap.FindAction(InputActionNames.Ability2);
            Ability3_IA = playerInput.currentActionMap.FindAction(InputActionNames.Ability3);
            Ability4_IA = playerInput.currentActionMap.FindAction(InputActionNames.Ability4);
            Ability5_IA = playerInput.currentActionMap.FindAction(InputActionNames.Ability5);
            
            
            _selfDamage_IA = playerInput.currentActionMap.FindAction(InputActionNames.TestApplyDamage);
            
            SetTargetSwitchButtonType(buttonTargetSwitchType);

            awakeFinished = true;

        }

        private void OnEnable()
        {
            _movementInputAction_IA.performed += OnRun;
            _movementInputAction_IA.canceled += OnRun;
            _itemUseInputAction_IA.performed += OnActiveItemUsePerformed;
            _itemUseInputAction_IA.canceled += OnActiveItemUseCanceled;
            _itemSwitchInputAction_IA.performed += OnItemSwitchSlotPerform;
            _itemSwitchLRInputAction_IA.performed += OnItemSwitchLeftRightPerform;
            _itemPickup_IA.performed += OnItemPickupIAPerform;
            _itemDrop_IA.performed += OnItemDropIAPerform;
            _targetSwitchButton_IA.performed += OnTargetSwitchButtonIaPressed;
            _lastTargetSwitchRSFlick_IA.performed += OnTargetSwitchRSFlicked;
            _targetSwitchLeftRight_IA.performed += OnTargetSwitchLeftRightIaPressed;

            _crouch_IA.performed += OnCrouchPerformed;
            _crouch_IA.canceled += OnCrouchReleased;

            Ability1_IA.performed += OnAbility1Performed;
            Ability2_IA.performed += OnAbility2Performed;
            Ability3_IA.performed += OnAbility3Performed;
            Ability4_IA.performed += OnAbility4Performed;
            Ability5_IA.performed += OnAbility5Performed;

            _selfDamage_IA.performed += OnDamageSelf;
        }

        private void OnDisable()
        {
            _movementInputAction_IA.performed -= OnRun;
            _movementInputAction_IA.canceled -= OnRun;
            _itemUseInputAction_IA.performed -= OnActiveItemUsePerformed;
            _itemUseInputAction_IA.canceled -= OnActiveItemUseCanceled;
            _itemSwitchInputAction_IA.performed -= OnItemSwitchSlotPerform;
            _itemSwitchLRInputAction_IA.performed -= OnItemSwitchLeftRightPerform;
            _itemPickup_IA.performed -= OnItemPickupIAPerform;
            _itemDrop_IA.performed -= OnItemDropIAPerform;
            _targetSwitchButton_IA.performed -= OnTargetSwitchButtonIaPressed;
            _lastTargetSwitchRSFlick_IA.performed -= OnTargetSwitchRSFlicked;
            _targetSwitchLeftRight_IA.performed -= OnTargetSwitchLeftRightIaPressed;
            
            _crouch_IA.performed -= OnCrouchPerformed;
            _crouch_IA.canceled -= OnCrouchReleased;

            Ability1_IA.performed -= OnAbility1Performed;
            Ability2_IA.performed -= OnAbility2Performed;
            Ability3_IA.performed -= OnAbility3Performed;
            Ability4_IA.performed -= OnAbility4Performed;
            Ability5_IA.performed -= OnAbility5Performed;
            
            _selfDamage_IA.performed -= OnDamageSelf;
            
        }

        private void Update()
        {
            
        }

        
        public void SetTargetSwitchButtonType(ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons incomingTargetSwitchType)
        {
            buttonTargetSwitchType = incomingTargetSwitchType;
            
            //Additional updates for later
            switch (incomingTargetSwitchType)
            {
                case ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons.LeftRightSwitch:
                    _targetSwitchLeftRight_IA.Enable();
                    _targetSwitchButton_IA.Disable();
                    break;
                case ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons.SecondNearestTarget:
                    _targetSwitchButton_IA.Enable();
                    _targetSwitchLeftRight_IA.Disable();
                    break;
                case ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons.CurrentLockedToNextTargetNearest:
                    _targetSwitchButton_IA.Enable();
                    _targetSwitchLeftRight_IA.Disable();
                    break;
            }
        } 


        
        public PlayerControlScheme currentActiveControlScheme;
        public void OnDeviceChanged(PlayerInput playerInput)
        {
            if (playerInput.currentControlScheme == "Gamepad")
                currentActiveControlScheme = PlayerControlScheme.GamePad;
            else
                currentActiveControlScheme = PlayerControlScheme.KBM;
        }
        
        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                incomingMovementVector = context.ReadValue<Vector2>();
                if (incomingMovementVector != Vector2.zero)
                {
                    lastIncomingMovementVector = incomingMovementVector;
                }
            }
            else
            {
                incomingMovementVector = Vector2.zero;

            }
        }


        
        void OnActiveItemUsePerformed(InputAction.CallbackContext context) { itemUsePerforming = true; ActiveItemUseEventPerformed.Invoke(); }
        void OnActiveItemUseCanceled(InputAction.CallbackContext context) { itemUsePerforming = false; ActiveItemUseEventCanceled.Invoke(); }

        void OnItemSwitchSlotPerform(InputAction.CallbackContext context) { lastItemSwitchSlot = (int) context.ReadValue<float>(); ItemSwitchSlotPerformed.Invoke(lastItemSwitchSlot); }
        void OnItemSwitchLeftRightPerform(InputAction.CallbackContext context) { lastItemSwitchDirection = (int) context.ReadValue<float>(); ItemSwitchLeftRightEventPerformed.Invoke(lastItemSwitchDirection); }
        void OnItemPickupIAPerform(InputAction.CallbackContext context) { ItemPickupEventPerformed.Invoke(); }
        void OnItemDropIAPerform(InputAction.CallbackContext context) { ItemDropEventPerformed.Invoke(); }

        void OnTargetSwitchButtonIaPressed(InputAction.CallbackContext context)
        {
            TargetSwitchSingledButtonPressedEvent.Invoke();
        }

        void OnTargetSwitchLeftRightIaPressed(InputAction.CallbackContext context)
        {
            lastTargetSwitchDirectionLR = context.ReadValue<float>();
            TargetSwitchLRPressedEvent.Invoke();
        }
        void OnTargetSwitchRSFlicked(InputAction.CallbackContext context)
        {
            FilterRSInput(context.ReadValue<Vector2>());
        }

        private float maxDeflectionSqr = 0;
        private bool deflectionRecorded = false;

        void FilterRSInput(Vector2 rightStickInput)
        {
            if (rightStickInput.sqrMagnitude > maxDeflectionSqr)
            {
                maxDeflectionSqr = rightStickInput.sqrMagnitude;
                lastRsFlickDirection = rightStickInput;
                deflectionRecorded = true;
            }

            // Check if right stick is moving inward and was previously deflected outward
            if (deflectionRecorded && rightStickInput.sqrMagnitude < maxDeflectionSqr * 0.8f && maxDeflectionSqr > 0.1f)
            {
                // Vector3 flickDirection = new Vector3(lastRSinput.x, 0, lastRSinput.y);
                TargetSwitchRSFlickEvent?.Invoke();

                // Reset max deflection after switching
                maxDeflectionSqr = 0;
                deflectionRecorded = false;
            }
            
        }

        void OnReloadGunPerformed(InputAction.CallbackContext context) { performReload = true; ReloadGunPerformednEvent.Invoke(); }
        void OnReloadGunCanceled(InputAction.CallbackContext context) { performReload = false; ReloadGunCanceledEvent.Invoke(); }
        void OnCrouchPerformed(InputAction.CallbackContext context){ crouchPerforming = true; CrouchPerformedEvent.Invoke(); }
        void OnCrouchReleased(InputAction.CallbackContext context){ crouchPerforming = false; CrouchCanceledEvent.Invoke(); }

        void OnAbility1Performed(InputAction.CallbackContext context) { abilityTriggeredInputType = AbilityTriggeredInputType.Ability1; AbilityTriggerEventPerformed.Invoke(AbilityTriggeredInputType.Ability1); }
        void OnAbility2Performed(InputAction.CallbackContext context) { abilityTriggeredInputType = AbilityTriggeredInputType.Ability2; AbilityTriggerEventPerformed.Invoke(AbilityTriggeredInputType.Ability2); }
        void OnAbility3Performed(InputAction.CallbackContext context) { abilityTriggeredInputType = AbilityTriggeredInputType.Ability3; AbilityTriggerEventPerformed.Invoke(AbilityTriggeredInputType.Ability3); }
        void OnAbility4Performed(InputAction.CallbackContext context) { abilityTriggeredInputType = AbilityTriggeredInputType.Ability4; AbilityTriggerEventPerformed.Invoke(AbilityTriggeredInputType.Ability4); }
        void OnAbility5Performed(InputAction.CallbackContext context) { abilityTriggeredInputType = AbilityTriggeredInputType.Ability5; AbilityTriggerEventPerformed.Invoke(AbilityTriggeredInputType.Ability5); }

        void OnDamageSelf(InputAction.CallbackContext context)
        {
            TestTakeSelfDamage.Invoke(Random.insideUnitCircle.normalized, BulletBase.DamageType.Normal);
        }
        
        //For Live Debugging
        /*private void OnValidate()
        {
            if (awakeFinished)
            {
                SetTargetSwitchButtonType(buttonTargetSwitchType);
                DebugX.LogWithColorYellow("Validated Target switch on inspector change");
            }
            
            List<string> name = new List<string>();
      
            name.Add("Apple");
            name.Add("Manog");
            name.Add("Guava");
            
            foreach (var sname in name)
            {
                Debug.Log($"{sname} in {name.IndexOf(sname)}");
            }
        }*/
    }
}