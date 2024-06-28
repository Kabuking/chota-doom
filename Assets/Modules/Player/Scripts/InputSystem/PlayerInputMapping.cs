using Characters.Player.Global;
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
        
        public UnityAction AimingEventPerformed = delegate {  };
        public UnityAction AimingEventCanceled = delegate {  };
        
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
        public UnityAction SprintPerformingEvent = delegate {  };
        public UnityAction SprintCanceledEvent = delegate {  };
        public UnityAction ReloadGunPerformednEvent = delegate {  };
        public UnityAction ReloadGunCanceledEvent = delegate {  };
        public UnityAction PaceControlPerformEvent = delegate {  };
        public UnityAction PaceControlCanceledEvent = delegate {  };
        public UnityAction EvadeControlPerformEvent = delegate {  };
        public UnityAction EvadeControlCanceledEvent = delegate {  };


        public Vector2 rollDirection = Vector2.zero;
        [Header("Move Input LS")]
        // private InputAction _moveInputAction;
        public Vector2 lastIncomingMovementVector;
        public Vector2 incomingMovementVector;
        // public float incomingHorizontal;
        // public float incomingVertical;

        [Header("Target Direction RSFlick")] 
        public Vector2 lastRsFlickDirection;
        public Vector2 currentRsFlickDirection;

        [FormerlySerializedAs("targetSwitchDirectionLR")]
        public float lastTargetSwitchDirectionLR = 0;
        
        public bool IsAiming;


        [Header("Item Switching")] 
        [SerializeField] public int lastItemSwitchDirection = 0;
        [SerializeField] public int lastItemSwitchSlot = 0;
        
        // public float incomingHorizontalLookAround;
        // public float incomingVerticalLookAround;

        private InputAction _movementInputAction_IA;
        private InputAction _itemUseInputAction_IA;
        private InputAction _itemSwitchInputAction_IA;
        private InputAction _itemSwitchLRInputAction_IA;
        private InputAction _itemPickup_IA;
        private InputAction _itemDrop_IA;
        private InputAction _evadeInputAction_IA;
        private InputAction _lastTargetSwitchRSFlick_IA;

        private InputActionMap _targetSwitchSingleButton_IAM;
        private InputAction _targetSwitchButton_IA;

        private InputActionMap _targetSwitchLeftRight_IAM;
        private InputAction _targetSwitchLeftRight_IA;
        
        
        // private InputAction _shootingGunOneInputAction;
        // private InputAction _shootingGunTwoInputAction;
        /*private InputAction _weaponSwitchInputAction;
        
        private InputAction _reloadGunInputAction;

        private InputAction _sprintInputAction;
        private InputAction _paceControlInputAction;
        private InputAction _dashInputAction;*/
        
        public bool sprintPerforming = false;
        public bool paceControlPerforming = false;
        public bool itemUsePerforming = false;
        public bool performEvade = false;
        public bool performReload = false;
        
        public Plane playerPlane;

        private bool awakeFinished = false;
        
        public enum PlayerControlScheme
        {
            GamePad,
            KBM
        }
        
        private void Awake()
        {
            
            playerInput = GetComponent<PlayerInput>();
            
            // playerInput.actions.
            // gameplayInput = new GameInput();
            // _lookAroundInputAction = playerInput.currentActionMap.FindAction(InputActionNames.LookAround);
            _movementInputAction_IA = playerInput.currentActionMap.FindAction(InputActionNames.Movement);
            _itemUseInputAction_IA = playerInput.currentActionMap.FindAction(InputActionNames.ItemUse);
            _itemSwitchInputAction_IA  = playerInput.currentActionMap.FindAction(InputActionNames.ItemSwitch);
            _itemSwitchLRInputAction_IA  = playerInput.currentActionMap.FindAction(InputActionNames.ItemSwitchLR);
            _evadeInputAction_IA = playerInput.currentActionMap.FindAction(InputActionNames.Evade);
            _lastTargetSwitchRSFlick_IA =  playerInput.currentActionMap.FindAction(InputActionNames.TargetSwitchRSFlick);
            _itemPickup_IA =  playerInput.currentActionMap.FindAction(InputActionNames.ItemPickup);
            _itemDrop_IA =  playerInput.currentActionMap.FindAction(InputActionNames.ItemDrop);
            
            // _targetSwitchSingleButton_IAM
            _targetSwitchButton_IA = playerInput.currentActionMap.FindAction(InputActionNames.TargetSwitchButton);
            _targetSwitchLeftRight_IA = playerInput.currentActionMap.FindAction(InputActionNames.TargetSwitchButtonLR);
            
            // _shootingGunOneInputAction = playerInput.currentActionMap.FindAction(InputActionNames.ActiveItemUse);
            // _reloadGunInputAction = playerInput.currentActionMap.FindAction(InputActionNames.ReloadGun);
            // _shootingGunTwoInputAction = playerInput.currentActionMap.FindAction(InputActionNames.WeaponTwo);
            // _weaponSwitchInputAction = playerInput.currentActionMap.FindAction(InputActionNames.WeaponSwitch);
            // _sprintInputAction = playerInput.currentActionMap.FindAction(InputActionNames.Sprint);
            // _paceControlInputAction = playerInput.currentActionMap.FindAction(InputActionNames.PaceControl);
            // _dashInputAction = playerInput.currentActionMap.FindAction(InputActionNames.Dash);

            playerPlane = new Plane(Vector3.up, transform.position);
            
            SetTargetSwitchButtonType(buttonTargetSwitchType);

            awakeFinished = true;

        }

        private void OnEnable()
        {
            // gameplayInput.Gameplay.LookAround.performed += OnLookAround;
            // gameplayInput.Gameplay.LookAround.canceled += OnLookAround;
            _movementInputAction_IA.performed += OnRun;
            _movementInputAction_IA.canceled += OnRun;
            _itemUseInputAction_IA.performed += OnActiveItemUsePerformed;
            _itemUseInputAction_IA.canceled += OnActiveItemUseCanceled;
            _itemSwitchInputAction_IA.performed += OnItemSwitchSlotPerform;
            _itemSwitchLRInputAction_IA.performed += OnItemSwitchLeftRightPerform;
            _itemPickup_IA.performed += OnItemPickupIAPerform;
            _itemDrop_IA.performed += OnItemDropIAPerform;
            _evadeInputAction_IA.performed += OnEvadeControlPerformed;
            _evadeInputAction_IA.canceled += OnEvadeControlCanceled;
            _targetSwitchButton_IA.performed += OnTargetSwitchButtonIaPressed;
            _lastTargetSwitchRSFlick_IA.performed += OnTargetSwitchRSFlicked;
            _targetSwitchLeftRight_IA.performed += OnTargetSwitchLeftRightIaPressed;
            // _shootingGunOneInputAction.performed += OnActiveItemUseInput;
            // _shootingGunOneInputAction.canceled += OnWeaponOneInputCanceled;
            // _shootingGunTwoInputAction.performed += OnWeaponTwoInput;
            /*_weaponSwitchInputAction.performed += OnWeaponSwitchInput;
            _sprintInputAction.performed += OnSprintInputPerforming;
            _sprintInputAction.canceled += OnSprintInputCanceled;
            _reloadGunInputAction.performed += OnReloadGunPerformed;
            _reloadGunInputAction.canceled += OnReloadGunCanceled;
            _paceControlInputAction.performed += OnPaceControlPerformed;
            _paceControlInputAction.canceled += OnPaceControlCanceled;*/
            // _dashInputAction.performed += OnDashControlPerformed;
            // _dashInputAction.canceled += OnDashControlCanceled;

            /*
            gameplayInput.Gameplay.Movement.performed += OnRun;
            gameplayInput.Gameplay.Movement.canceled += OnRun;
            
            gameplayInput.Gameplay.Evade.performed += OnEvadeControlPerformed;
            gameplayInput.Gameplay.Evade.canceled += OnEvadeControlCanceled;

            gameplayInput.Gameplay.ActiveItemUse.performed += OnActiveItemUsePerformed;
            gameplayInput.Gameplay.ActiveItemUse.canceled += OnActiveItemUseCanceled;

            gameplayInput.Gameplay.Aim.performed += OnAimingPerformed;
            gameplayInput.Gameplay.Aim.canceled += OnAimingCanceled;
            
            gameplayInput.Enable();
            */

        }

        private void OnDisable()
        {
            // gameplayInput.Gameplay.LookAround.performed-= OnLookAround;
            // gameplayInput.Gameplay.LookAround.canceled -= OnLookAround;
            _movementInputAction_IA.performed -= OnRun;
            _movementInputAction_IA.canceled -= OnRun;
            _itemUseInputAction_IA.performed -= OnActiveItemUsePerformed;
            _itemUseInputAction_IA.canceled -= OnActiveItemUseCanceled;
            _itemSwitchInputAction_IA.performed -= OnItemSwitchSlotPerform;
            _itemSwitchLRInputAction_IA.performed -= OnItemSwitchLeftRightPerform;
            _itemPickup_IA.performed -= OnItemPickupIAPerform;
            _itemDrop_IA.performed -= OnItemDropIAPerform;
            _evadeInputAction_IA.performed -= OnEvadeControlPerformed;
            _evadeInputAction_IA.canceled -= OnEvadeControlCanceled;
            _targetSwitchButton_IA.performed -= OnTargetSwitchButtonIaPressed;
            _lastTargetSwitchRSFlick_IA.performed -= OnTargetSwitchRSFlicked;
            _targetSwitchLeftRight_IA.performed -= OnTargetSwitchLeftRightIaPressed;
            
            
            // _shootingGunOneInputAction.performed -= OnActiveItemUseInput;
            // _shootingGunTwoInputAction.performed -= OnWeaponTwoInput;
            /*_weaponSwitchInputAction.performed -= OnWeaponSwitchInput;
            _sprintInputAction.performed -= OnSprintInputPerforming;
            _sprintInputAction.canceled -= OnSprintInputCanceled;
            _reloadGunInputAction.performed -= OnReloadGunPerformed;
            _paceControlInputAction.performed -= OnPaceControlPerformed;
            _paceControlInputAction.canceled -= OnPaceControlCanceled;
            _dashInputAction.performed -= OnDashControlPerformed;
            _dashInputAction.canceled -= OnDashControlCanceled;*/
            
            // gameplayInput.Gameplay.Dash.performed -= OnDashControlPerformed;
            // gameplayInput.Gameplay.Dash.canceled -= OnDashControlCanceled;
            
            /*gameplayInput.Gameplay.Movement.performed -= OnRun;
            gameplayInput.Gameplay.Movement.canceled -= OnRun;
            
            gameplayInput.Gameplay.ActiveItemUse.performed -= OnActiveItemUsePerformed;
            gameplayInput.Gameplay.ActiveItemUse.canceled -= OnActiveItemUseCanceled;
            
            
            gameplayInput.Gameplay.Evade.performed -= OnEvadeControlPerformed;
            gameplayInput.Gameplay.Evade.canceled -= OnEvadeControlCanceled;
            
            gameplayInput.Gameplay.Aim.performed -= OnAimingPerformed;
            gameplayInput.Gameplay.Aim.canceled -= OnAimingCanceled;
            
            gameplayInput.Disable();*/
        }

        private void Update()
        {
            // incomingMovementVector = gameplayInput.Gameplay.Movement.ReadValue<Vector2>();
            /*incomingMovementVector = playerInput.currentActionMap.FindAction(InputActionNames.Movement).ReadValue<Vector2>();
            if (incomingMovementVector != Vector2.zero)
            {
                lastIncomingMovementVector = incomingMovementVector;
            }*/

            // currentIncomingLookAround = gameplayInput.Gameplay.LookAround.ReadValue<Vector2>();
            
            /*if ((currentIncomingLookAround.x != 0 || currentIncomingLookAround.y != 0))
                lastIncomingLookAround = currentIncomingLookAround;
            
            
            playerPlane.distance = -Vector3.Dot(playerPlane.normal, transform.position);*/
            

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

        /*void OnActiveItemUse(InputAction.CallbackContext context)
        {
            if (context.performed) itemUsePerforming = true;
            else itemUsePerforming = false;
        }*/
        public void OnRun(InputAction.CallbackContext context)
        {
            // DebugX.LogWithColorYellow("--- "+context.phase);
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
            // incomingHorizontal = incomingMovementVector.x;
            // incomingVertical = incomingMovementVector.y;
        }

        void OnAimingPerformed(InputAction.CallbackContext context) { IsAiming = true; AimingEventPerformed.Invoke(); }
        void OnAimingCanceled(InputAction.CallbackContext context) { IsAiming = false; AimingEventCanceled.Invoke(); }
        
        void OnActiveItemUsePerformed(InputAction.CallbackContext context) { itemUsePerforming = true; ActiveItemUseEventPerformed.Invoke(); }
        void OnActiveItemUseCanceled(InputAction.CallbackContext context) { itemUsePerforming = false; ActiveItemUseEventCanceled.Invoke(); }

        void OnItemSwitchSlotPerform(InputAction.CallbackContext context) { lastItemSwitchSlot = (int) context.ReadValue<float>(); ItemSwitchSlotPerformed.Invoke(lastItemSwitchSlot); }
        void OnItemSwitchLeftRightPerform(InputAction.CallbackContext context) { lastItemSwitchDirection = (int) context.ReadValue<float>(); ItemSwitchLeftRightEventPerformed.Invoke(lastItemSwitchDirection); }
        void OnItemPickupIAPerform(InputAction.CallbackContext context) { ItemPickupEventPerformed.Invoke(); }
        void OnItemDropIAPerform(InputAction.CallbackContext context) { ItemDropEventPerformed.Invoke(); }

        void OnTargetSwitchButtonIaPressed(InputAction.CallbackContext context)
        {
            /*if(buttonTargetSwitchType == ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons.SecondNearestTarget 
               || buttonTargetSwitchType == ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons.CurrentLockedToNextTargetNearest)
                TargetSwitchButtonPressedEvent.Invoke();*/
            
            TargetSwitchSingledButtonPressedEvent.Invoke();
            // DebugX.LogWithColorCyan("Single button t switch pressed");
        }

        void OnTargetSwitchLeftRightIaPressed(InputAction.CallbackContext context)
        {
            /*if (buttonTargetSwitchType == ManualSwitchAtEnemyInRange.TargetSwitchTypeForButtons.LeftRightSwitch)
            {
                lastTargetSwitchDirectionLR = context.ReadValue<float>();
                DebugX.LogWithColorCyan("Input LR Switch");
                TargetSwitchLRPressedEvent.Invoke();
            }*/
            lastTargetSwitchDirectionLR = context.ReadValue<float>();
            // DebugX.LogWithColorCyan("Input LR Switch");
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
        // void OnWeaponTwoInput(InputAction.CallbackContext context) {WeaponTwoShootEvent.Invoke();}
        // void OnWeaponSwitchInput(InputAction.CallbackContext context) {WeaponSwitchEvent.Invoke();}
        void OnSprintInputPerforming(InputAction.CallbackContext context) { sprintPerforming = true; SprintPerformingEvent.Invoke(); }
        void OnSprintInputCanceled(InputAction.CallbackContext context) { sprintPerforming = false; SprintCanceledEvent.Invoke(); }
        void OnReloadGunPerformed(InputAction.CallbackContext context) { performReload = true; ReloadGunPerformednEvent.Invoke(); }
        void OnReloadGunCanceled(InputAction.CallbackContext context) { performReload = false; ReloadGunCanceledEvent.Invoke(); }
        void OnPaceControlPerformed(InputAction.CallbackContext context) { paceControlPerforming = true; PaceControlPerformEvent.Invoke(); }
        void OnPaceControlCanceled(InputAction.CallbackContext context) { paceControlPerforming = false; PaceControlCanceledEvent.Invoke(); }


        void OnEvadeControlPerformed(InputAction.CallbackContext context)
        {
            performEvade = true; 
            EvadeControlPerformEvent.Invoke();
        }
        void OnEvadeControlCanceled(InputAction.CallbackContext context) { performEvade = false; EvadeControlCanceledEvent.Invoke(); }


        //For Live Debugging
        private void OnValidate()
        {
            if (awakeFinished)
            {
                SetTargetSwitchButtonType(buttonTargetSwitchType);
                DebugX.LogWithColorYellow("Validated Target switch on inspector change");
            }
        }
    }
}