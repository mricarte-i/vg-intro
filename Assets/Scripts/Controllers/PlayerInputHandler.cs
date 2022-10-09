using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
[RequireComponent(typeof(CharacterController), typeof(BoxCollider))]
public class PlayerInputHandler : MonoBehaviour
{
    #region InputActions
    private InputAction walking;
    private InputAction jumping;
    private InputAction menu;
    [Space][SerializeField] private InputActionAsset playerControls;
    [SerializeField] private InputUser _user;
    public void SetInputUser(InputUser newUser) => _user = newUser;
    #endregion

    #region Properties and Fields

    [Header("Movement variables")]
    //Movement stuff...
    [SerializeField] private float movementSpeed = 10f;
    private Vector3 _currentSpeed;

    [Header("Jumping variables")]
    //jump variables...
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _groundedGravity = -0.5f;
    private float _initialJumpVelocity;
    [SerializeField] private float _maxJumpHeight = 1.0F;
    [SerializeField] private float _maxJumpTime = 0.5f;


    [Header("Controller variables")]
    //Controller stuff...
    [SerializeField] private bool _isDummy = false;
    [SerializeField] private int _playerId = 0;
    [SerializeField] private bool _inverted = false;
    public void Invert(bool isInverted) => _inverted = isInverted;

    [SerializeField] private bool _inputKeyboard = false;
    [SerializeField] private int _controllerId = 0;

    private MoveCommandReceiver _moveCommandReceiver;
    private List<MoveCommand> commands = new List<MoveCommand>();

    private CharacterController _characterController;
    private BoxCollider _boxTrigger; //do we really need to keep this variable?
    private CharacterPushInteraction _pushInteract;

    //private int currentCmdNum = 0; memento lmao

    #endregion

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _boxTrigger = GetComponent<BoxCollider>();
        _boxTrigger.isTrigger = true;

        _pushInteract = GetComponent<CharacterPushInteraction>();

        _moveCommandReceiver = new MoveCommandReceiver();

        setUpJumpVariables();
    }

    public void BindControls(GeneratedPlayerControls controls){
        var movementActionMap = controls.Movement;
        var menuActionMap = controls.Menu;

        walking = movementActionMap.Walking;
        jumping = movementActionMap.Jumping;
        menu = menuActionMap.Pause;


        walking.started += OnWalkingPerformed;
        walking.performed += OnWalkingPerformed;
        walking.canceled += OnWalkingPerformed;

        jumping.started += OnJumpingPerformed;
        jumping.canceled += OnJumpingPerformed;

        menu.started += OnMenuPerformed;

        //should be called OnEnable...
        walking.Enable();
        jumping.Enable();
        menu.Enable();

    }

    private void OnDestroy() {
        _user.UnpairDevices();
    }

    private void setUpJumpVariables() {
        float timeToApex = _maxJumpTime / 2;
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
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
            Debug.Log("hear me!");
        }
    }
    private void OnMenuPerformed(InputAction.CallbackContext context) {
        if(_isDummy) return;
        context.ReadValueAsButton();
        Debug.Log("calling a manager...");
        AppManager.Instance.TogglePause();
    }


    void OnDisable(){
        walking.Disable();
        jumping.Disable();
        menu.Disable();
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        //player might not be giving inputs but could be falling, still have speed, etc.
        _currentSpeed = Direction * movementSpeed * Time.deltaTime;

        if(_isDummy) return;
        _pushInteract.Speed(PushForce);


        Move(_currentSpeed);
        handleGravity();
        handleJump();
    }

    private void handleJump(){
        if(!IsJumping && _characterController.isGrounded && IsJumpPressed){
            Debug.Log("Jump!");
            IsJumping = true;
            Depth = _initialJumpVelocity;
        }else if(!IsJumpPressed && IsJumping && _characterController.isGrounded){
            IsJumping = false;
        }
    }

    private void handleGravity(){
        if(_characterController.isGrounded){
            Depth = _groundedGravity;
        }else{
            Depth += _gravity * Time.deltaTime;
        }
    }

    private void Move(Vector3 velForce){
        MoveCommand moveCommand = new MoveCommand(
            _moveCommandReceiver,
            velForce,
            _characterController);
        moveCommand.Execute();
        commands.Add(moveCommand);
        //_currentCmdNum++; memento lmao
    }

    #region Movement Properties

    private bool IsJumpPressed { get; set; }
    private bool IsJumping { get; set; }

    //names based on typical controller naming...
    private float Vertical { get; set; }
    private float Horizontal { get; set; }
    //name based on me not wanting to change Vertical & Horizontal to something better
    private float Depth { get; set; } //(y/gravity)

    private Vector3 Direction {
        get {
            var dir =
                (transform.forward * Horizontal * (_inverted ? -1 : 1)) +
                (transform.up * Depth) +
                (transform.right * 0.5f * Vertical * (_inverted ? 1 : -1)); //half rotation speed ?
            return dir;
        }
    }

    private bool IsMoving => Direction != Vector3.zero;
    private Vector3 PushForce => _characterController.velocity.Equals(Vector3.zero) ?
        _currentSpeed :
        _characterController.velocity;

    #endregion
}
