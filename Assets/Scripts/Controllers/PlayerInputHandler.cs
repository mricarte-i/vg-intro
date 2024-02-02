using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerInputHandler : InputHandler
    {
        #region InputActions
        private InputAction walking;
        private InputAction jumping;
        private InputAction menu;
        private InputAction neutralAttack;
        private InputAction downAttack;
        private InputAction upperAttack;

        [Space][SerializeField] private InputActionAsset playerControls;
        [SerializeField] private InputUser _user;
        public void SetInputUser(InputUser newUser) => _user = newUser;
        #endregion

        #region Properties and Fields
        
        //private BoxCollider _boxTrigger; //do we really need to keep this variable?
        private CharacterPushInteraction _pushInteract;

        #endregion

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            _pushInteract = GetComponent<CharacterPushInteraction>();

            _moveCommandReceiver = new MoveCommandReceiver();

            setUpJumpVariables();
        }

        private new void Start()
        {
            base.Start();
        }

        public void BindControls(GeneratedPlayerControls controls){
            var movementActionMap = controls.Movement;
            var menuActionMap = controls.Menu;
            var attackActionMap = controls.Attacks;

            walking = movementActionMap.Walking;
            jumping = movementActionMap.Jumping;
            menu = menuActionMap.Pause;
            neutralAttack = attackActionMap.Neutral;
            downAttack = attackActionMap.Down;
            upperAttack = attackActionMap.Upper;

            walking.started += OnWalkingPerformed;
            walking.performed += OnWalkingPerformed;
            walking.canceled += OnWalkingPerformed;

            jumping.started += OnJumpingPerformed;
            jumping.canceled += OnJumpingPerformed;

            menu.started += OnMenuPerformed;

            neutralAttack.started += OnNeutralAttackPerformed;
            neutralAttack.canceled += OnNeutralAttackPerformed;

            downAttack.started += OnDownAttackPerformed;
            downAttack.canceled += OnDownAttackPerformed;

            upperAttack.started += OnUpperAttackPerformed;
            upperAttack.canceled += OnUpperAttackPerformed;

            //should be called OnEnable...
            walking.Enable();
            jumping.Enable();
            menu.Enable();
            neutralAttack.Enable();
            downAttack.Enable();
            upperAttack.Enable();
        }

        private void OnDestroy() {
            _user.UnpairDevices();
        }

        #region Movement Boilerplate

        private void OnWalkingPerformed(InputAction.CallbackContext context) {
            var direction = context.ReadValue<Vector2>();

            Horizontal = direction.x;
            Vertical = direction.y;

        }
        private void OnJumpingPerformed(InputAction.CallbackContext context) {
            IsJumpPressed = context.ReadValueAsButton();
            if(IsJumpPressed){
                //Debug.Log("hear me!");
            }
        }
        private void OnMenuPerformed(InputAction.CallbackContext context) {
            if(_isDummy) return;
            context.ReadValueAsButton();
            //Debug.Log("calling a manager...");
            AppManager.Instance.TogglePause();
        }

        private void OnNeutralAttackPerformed(InputAction.CallbackContext context)
        {
            IsNeutralAttackPressed = context.ReadValueAsButton();
        }

        private void OnDownAttackPerformed(InputAction.CallbackContext context)
        {
            IsDownAttackPressed = context.ReadValueAsButton();
        }

        private void OnUpperAttackPerformed(InputAction.CallbackContext context)
        {
            IsUpperAttackPressed = context.ReadValueAsButton();
        }

        void OnDisable(){
            walking.Disable();
            jumping.Disable();
            menu.Disable();
            neutralAttack.Disable();
            downAttack.Disable();
            upperAttack.Disable();
        }

        public override void EnablePlayerActions()
        {
            walking.Enable();
            jumping.Enable();
            neutralAttack.Enable();
            downAttack.Enable();
            upperAttack.Enable();
        }

        public override void DisablePlayerActions()
        {
            walking.Disable();
            jumping.Disable();
            neutralAttack.Disable();
            downAttack.Disable();
            upperAttack.Disable();
        }

        public override void ResetPlayerActions()
        {
            base.ResetPlayerActions();
        }

        #endregion

        // Update is called once per frame
        void Update()
        {
            //player might not be giving inputs but could be falling, still have speed, etc.
            _currentSpeed = Direction * (movementSpeed * Time.deltaTime);

            if(_isDummy) return;
            _pushInteract.Speed(PushForce);
        
            StartJump = false;
        
            Move(_currentSpeed);
            handleGravity();
            handleJump();
            handleAttack();
            handleAnimations();
        }
    }
}
