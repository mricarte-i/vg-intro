using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class AIInputHandler : InputHandler
    {
        #region InputActions
        //private InputAction walking;
        //private InputAction jumping;
        //private InputAction menu;
        //private InputAction neutralAttack;
        //private InputAction downAttack;
        //private InputAction upperAttack;

        //[Space][SerializeField] private InputActionAsset playerControls;
        //[SerializeField] private InputUser _user;
        //public void SetInputUser(InputUser newUser) => _user = newUser;
        #endregion

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            //_pushInteract = GetComponent<CharacterPushInteraction>();

            _moveCommandReceiver = new MoveCommandReceiver();

            setUpJumpVariables();
        }

        private new void Start()
        {
            base.Start();
        }

        private MarkovChain _markovChain = new MarkovChain();

        public new void BindControls(){
            //walking = movementActionMap.Walking;
            //jumping = movementActionMap.Jumping;
            //neutralAttack = attackActionMap.Neutral;
            //downAttack = attackActionMap.Down;
            //upperAttack = attackActionMap.Upper;
            _markovChain.AddState("walkingForward", OnWalkingForwardPerformed);
            _markovChain.AddState("jumping", OnJumpingPerformed);
            _markovChain.AddState("neutralAttack", OnNeutralAttackPerformed);
            _markovChain.AddState("downAttack", OnDownAttackPerformed);
            _markovChain.AddState("upperAttack", OnUpperAttackPerformed);
            _markovChain.AddTransition("walkingForward", "jumping", 0.15d);
            _markovChain.AddTransition("jumping", "walkingForward", 0.99d);
            _markovChain.SetState("walkingForward");
            /*
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
        */
        }

        private void OnDestroy() {
            //_user.UnpairDevices();
        }

        #region Movement Boilerplate

        private void OnWalkingForwardPerformed() {
            var direction = transform.forward;

            Horizontal = direction.x;
            Vertical = direction.z;

        }
        private void OnJumpingPerformed()
        {
            IsJumpPressed = true;
        }

        private void OnNeutralAttackPerformed()
        {
            IsNeutralAttackPressed = true;
        }

        private void OnDownAttackPerformed()
        {
            IsDownAttackPressed = true;
        }

        private void OnUpperAttackPerformed()
        {
            IsUpperAttackPressed = true;
        }

        private bool _isEnabled = true;

        void OnDisable()
        {
            _isEnabled = false;
        }

        public new void EnablePlayerActions()
        {
            _isEnabled = true;
        }

        public new void DisablePlayerActions()
        {
            _isEnabled = true;
        }

        #endregion
    
        private float _timeElapsed = 0;

        // Update is called once per frame
        private void Update()
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed > 1)
            {
                _markovChain.NextStep();
            }
            _markovChain.RunActions();
            //player might not be giving inputs but could be falling, still have speed, etc.
            _currentSpeed = Direction * (movementSpeed * Time.deltaTime);

            if(_isDummy) return;
            //_pushInteract.Speed(PushForce);
        
            StartJump = false;
        
            Move(_currentSpeed);
            handleGravity();
            handleJump();
            handleAttack();
            handleAnimations();
        }
    }
}
