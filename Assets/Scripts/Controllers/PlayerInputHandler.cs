using System;
using System.Collections.Generic;
using Animations;
using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
[RequireComponent(typeof(CharacterController))]
public class PlayerInputHandler : MonoBehaviour
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

    [SerializeField] private RhythmHpModifierData _rhythmValues;

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
    [SerializeField] private bool _inverted = false;
    public void Invert(bool isInverted) => _inverted = isInverted;

    private MoveCommandReceiver _moveCommandReceiver;
    private List<MoveCommand> commands = new List<MoveCommand>();

    private CharacterController _characterController;
    //private BoxCollider _boxTrigger; //do we really need to keep this variable?
    private CharacterPushInteraction _pushInteract;

    //private int currentCmdNum = 0; memento lmao

    private DamageSystemHandler _damageSystemHandler;
    public void SetDamageSystemHandler(DamageSystemHandler dsh) => _damageSystemHandler = dsh;

    private CharacterAnimatorController _animatorController;
    public void SetAnimatorController(CharacterAnimatorController cac) => _animatorController = cac;

    [Header("Rhythm Exclusive variables")]

    [SerializeField] private float _rythmCooldown = 0.3f;
    private float _currentRythmCooldown = 0f;

    #endregion

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _pushInteract = GetComponent<CharacterPushInteraction>();

        _moveCommandReceiver = new MoveCommandReceiver();

        setUpJumpVariables();
    }

    private void Start()
    {
        _damageSystemHandler.AddBeforeAttackingEvent(_animatorController.TriggerNeutralAttack, DamageSystemHandler.AttackType.Neutral);
        _damageSystemHandler.AddBeforeAttackingEvent(_animatorController.TriggerUpperAttack, DamageSystemHandler.AttackType.Upper);
        _damageSystemHandler.AddBeforeAttackingEvent(_animatorController.TriggerDownAttack, DamageSystemHandler.AttackType.Down);
        
        _damageSystemHandler.GetHurtbox.AddOnHitEvents(_animatorController.TriggerTakeDamage);
        _damageSystemHandler.GetHurtbox.AddOnLoseEvents(_animatorController.TriggerDying);
        _damageSystemHandler.GetHurtbox.AddOnLoseEvents(DisablePlayerActions);
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

    public void EnablePlayerActions()
    {
        walking.Enable();
        jumping.Enable();
        neutralAttack.Enable();
        downAttack.Enable();
        upperAttack.Enable();
    }

    public void DisablePlayerActions()
    {
        walking.Disable();
        jumping.Disable();
        neutralAttack.Disable();
        downAttack.Disable();
        upperAttack.Disable();
    }

    public void ResetPlayerActions()
    {
        EnablePlayerActions();
        Debug.Log($"Reseteando before: animacion Idle:{_animatorController.IsIdle()}");
        _animatorController.TriggerReset();
        Debug.Log($"Reseteando after: animacion Idle:{_animatorController.IsIdle()}");
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

    private void handleAnimations()
    {
        if (_damageSystemHandler.IsCurrentlyAttacking)
        {
            return;
        }
        
        if (StartJump)
        {
            _animatorController.TriggerJump();
            StartJump = false;
            return;
        }
        
        if (IsJumping && !_characterController.isGrounded) return;

        var movingOrientation = GetMovingOrientation();
        switch (movingOrientation)
        {
            case MovingOrientation.Forwards:
                _animatorController.TriggerWalkingForwards();
                return;
            case MovingOrientation.Backwards:
                _animatorController.TriggerWalkingBackwards();
                return;
        }
        
        _animatorController.TriggerIdle();
    }

    private void handleJump(){
        if(!IsJumping && _characterController.isGrounded && IsJumpPressed){
            //Debug.Log("Jump!");
            IsJumping = true;
            Depth = _initialJumpVelocity;
            StartJump = true;
        }
        else if(IsJumping && _characterController.isGrounded){
            IsJumping = IsJumpPressed;
        }
    }

    private void handleGravity(){
        if(_characterController.isGrounded){
            Depth = _groundedGravity;
        }else{
            Depth += _gravity * Time.deltaTime;
        }
    }

    private void handleAttack()
    {
        // lowering cooldown
        if(_currentRythmCooldown > 0) _currentRythmCooldown -= Time.deltaTime;

        var isAttackPressed = IsNeutralAttackPressed || IsDownAttackPressed || IsUpperAttackPressed;
        if(!isAttackPressed) return;
        
        if(AppManager.Instance.GetGameMode() == GameMode.RHYTHM && _currentRythmCooldown <= 0) {
            _currentRythmCooldown = _rythmCooldown;
            var result = RhythmController.Instance.GetBeat();
            switch(result){
                case RhythmState.Bad:
                    _damageSystemHandler.GetHurtbox.GetHit(-_rhythmValues.BadHpModifier);
                    return;
                case RhythmState.Good:
                    _damageSystemHandler.GetHurtbox.GetHit(-_rhythmValues.GoodHpModifier);
                    break;
                case RhythmState.Great:
                    _damageSystemHandler.GetHurtbox.GetHit(-_rhythmValues.GreatHpModifier);
                    break;
                default:
                    //Debug.Log("unexpected state");
                    break;
            }
        }
        if (IsNeutralAttackPressed)
        {
            _damageSystemHandler.DoNeutralAttack();
        }

        if (IsDownAttackPressed)
        {
            _damageSystemHandler.DoDownAttack();
        }

        if (IsUpperAttackPressed)
        {
            _damageSystemHandler.DoUpperAttack();
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
    private bool StartJump { get; set; }

    //names based on typical controller naming...
    private float Vertical { get; set; }
    private float Horizontal { get; set; }
    //name based on me not wanting to change Vertical & Horizontal to something better
    private float Depth { get; set; } //(y/gravity)

    private Vector3 Direction {
        get {
            var dir =
                (transform.forward * (Horizontal * (_inverted ? -1 : 1))) +
                (transform.up * Depth) +
                (transform.right * (0.5f * Vertical * (_inverted ? 1 : -1))); //half rotation speed ?
            return dir;
        }
    }

    private bool IsMoving => Mathf.Abs(_characterController.velocity.x) > 0.1;
    
    private enum MovingOrientation
    {
        Idle,
        Forwards,
        Backwards
    }
    
    private MovingOrientation GetMovingOrientation()
    {
        if (!IsMoving) return MovingOrientation.Idle;
        var dotProduct = Vector3.Dot(transform.forward, _characterController.velocity);
        if (dotProduct < 0) return MovingOrientation.Backwards;
        return MovingOrientation.Forwards;
    }
    
    private Vector3 PushForce => _characterController.velocity.Equals(Vector3.zero) ?
        _currentSpeed :
        _characterController.velocity;

    #endregion

    #region Attacks

    private bool IsNeutralAttackPressed { get; set; }
    private bool IsDownAttackPressed { get; set; }
    private bool IsUpperAttackPressed { get; set; }

    #endregion
}
