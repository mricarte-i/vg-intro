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
            _markovChain.AddState("idle", StopAction);
            
            _markovChain.AddState("walkingForward", StopAction);
            _markovChain.AddState("walkingForward", OnWalkingForwardPerformed);
            
            _markovChain.AddState("jumping", StopAction);
            _markovChain.AddState("jumping", OnJumpingPerformed);
            
            _markovChain.AddState("neutralAttack", StopAction);
            _markovChain.AddState("neutralAttack", OnNeutralAttackPerformed);
            
            _markovChain.AddState("downAttack", StopAction);
            _markovChain.AddState("downAttack", OnDownAttackPerformed);
            
            _markovChain.AddState("upperAttack", StopAction);
            _markovChain.AddState("upperAttack", OnUpperAttackPerformed);
            
            _markovChain.AddTransition("walkingForward", "idle", 0.3d);
            _markovChain.AddTransition("walkingForward", "jumping", 0.1d);
            _markovChain.AddTransition("walkingForward", "neutralAttack", 0.1d);
            _markovChain.AddTransition("walkingForward", "downAttack", 0.1d);
            _markovChain.AddTransition("walkingForward", "upperAttack", 0.1d);
            
            _markovChain.AddTransition("jumping", "idle", 0.3d);
            _markovChain.AddTransition("jumping", "walkingForward", 0.3d);
            _markovChain.AddTransition("jumping", "neutralAttack", 0.1d);
            _markovChain.AddTransition("jumping", "downAttack", 0.1d);
            _markovChain.AddTransition("jumping", "upperAttack", 0.1d);
            
            _markovChain.AddTransition("idle", "walkingForward", 0.3d);
            _markovChain.AddTransition("idle", "jumping", 0.1d);
            _markovChain.AddTransition("idle", "neutralAttack", 0.1d);
            _markovChain.AddTransition("idle", "downAttack", 0.1d);
            _markovChain.AddTransition("idle", "upperAttack", 0.1d);
            
            _markovChain.AddTransition("neutralAttack", "idle", 0.3d);
            _markovChain.AddTransition("neutralAttack", "walkingForward", 0.3d);
            _markovChain.AddTransition("neutralAttack", "jumping", 0.1d);
            _markovChain.AddTransition("neutralAttack", "downAttack", 0.1d);
            _markovChain.AddTransition("neutralAttack", "upperAttack", 0.1d);
            
            _markovChain.AddTransition("downAttack", "idle", 0.3d);
            _markovChain.AddTransition("downAttack", "walkingForward", 0.3d);
            _markovChain.AddTransition("downAttack", "jumping", 0.1d);
            _markovChain.AddTransition("downAttack", "neutralAttack", 0.1d);
            _markovChain.AddTransition("downAttack", "upperAttack", 0.1d);
            
            _markovChain.AddTransition("upperAttack", "idle", 0.3d);
            _markovChain.AddTransition("upperAttack", "walkingForward", 0.3d);
            _markovChain.AddTransition("upperAttack", "jumping", 0.1d);
            _markovChain.AddTransition("upperAttack", "neutralAttack", 0.1d);
            _markovChain.AddTransition("upperAttack", "downAttack", 0.1d);
            
            _markovChain.SetState("walkingForward");
        }

        private void OnDestroy() {
            //_user.UnpairDevices();
        }

        #region Movement Boilerplate

        private void StopAction()
        {
            StopJumping();
            StopWalking();
            StopNeutralAttack();
            StopDownAttack();
            StopUpperAttack();
        }

        private void OnWalkingForwardPerformed() {
            var direction = transform.forward;

            Horizontal = direction.x;
            Vertical = direction.z;
        }

        private void StopWalking()
        {
            Horizontal = 0;
            Vertical = 0;
        }
        
        private void OnJumpingPerformed()
        {
            IsJumpPressed = true;
        }

        private void StopJumping()
        {
            IsJumpPressed = false;
        }

        private void OnNeutralAttackPerformed()
        {
            IsNeutralAttackPressed = true;
        }

        private void StopNeutralAttack()
        {
            IsNeutralAttackPressed = false;
        }

        private void OnDownAttackPerformed()
        {
            IsDownAttackPressed = true;
        }

        private void StopDownAttack()
        {
            IsDownAttackPressed = false;
        }

        private void OnUpperAttackPerformed()
        {
            IsUpperAttackPressed = true;
        }

        private void StopUpperAttack()
        {
            IsUpperAttackPressed = false;
        }

        private bool _isEnabled = true;

        void OnDisable()
        {
            _isEnabled = false;
        }

        public override void EnablePlayerActions()
        {
            _isEnabled = true;
        }

        public override void DisablePlayerActions()
        {
            _isEnabled = false;
            StopAction();
        }

        public override void ResetPlayerActions()
        {
            EnablePlayerActions();
            _animatorController.TriggerReset();
        }

        #endregion
    
        private float _timeElapsed = 0;

        // Update is called once per frame
        private void Update()
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed > 1)
            {
                if (_isEnabled)
                {
                    _markovChain.NextStep();
                    _markovChain.RunActions();
                }
                _timeElapsed = 0;
                Debug.Log(_markovChain.GetCurrentState());
            }
            
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
