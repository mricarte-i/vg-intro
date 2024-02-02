using System;
using System.Collections.Generic;
using Animations;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class InputHandler : MonoBehaviour
    {
        
        #region Properties and Fields

        [SerializeField] protected RhythmHpModifierData _rhythmValues;

        [Header("Movement variables")]
        //Movement stuff...
        [SerializeField] protected float movementSpeed = 10f;
        protected Vector3 _currentSpeed;

        [Header("Jumping variables")]
        //jump variables...
        [SerializeField] protected float _gravity = -9.8f;
        [SerializeField] protected float _groundedGravity = -0.5f;
        protected float _initialJumpVelocity;
        [SerializeField] protected float _maxJumpHeight = 1.0F;
        [SerializeField] protected float _maxJumpTime = 0.5f;


        [Header("Controller variables")]
        //Controller stuff...
        [SerializeField] protected bool _isDummy = false;
        [SerializeField] protected bool _inverted = false;
        public void Invert(bool isInverted) => _inverted = isInverted;

        protected MoveCommandReceiver _moveCommandReceiver;
        protected List<MoveCommand> commands = new List<MoveCommand>();

        protected CharacterController _characterController;
        //protected BoxCollider _boxTrigger; //do we really need to keep this variable?
        //protected CharacterPushInteraction _pushInteract;

        protected PlayerId _playerId;
        public void SetPlayerId(PlayerId id) => _playerId = id;

        //private int currentCmdNum = 0; memento lmao

        protected DamageSystemHandler _damageSystemHandler;
        public void SetDamageSystemHandler(DamageSystemHandler dsh) => _damageSystemHandler = dsh;

        protected CharacterAnimatorController _animatorController;
        public void SetAnimatorController(CharacterAnimatorController cac) => _animatorController = cac;

        [Header("Rhythm Exclusive variables")]

        [SerializeField] protected float _rythmCooldown = 0.3f;
        protected float _currentRythmCooldown = 0f;

        #endregion
        
        protected void Start()
        {
            _damageSystemHandler.AddBeforeAttackingEvent(_animatorController.TriggerNeutralAttack, DamageSystemHandler.AttackType.Neutral);
            _damageSystemHandler.AddBeforeAttackingEvent(_animatorController.TriggerUpperAttack, DamageSystemHandler.AttackType.Upper);
            _damageSystemHandler.AddBeforeAttackingEvent(_animatorController.TriggerDownAttack, DamageSystemHandler.AttackType.Down);
            
            _damageSystemHandler.GetHurtbox.AddOnHitEvents(DisablePlayerActions);
            _damageSystemHandler.GetHurtbox.AddOnHitEvents(_animatorController.TriggerTakeDamage);
            
            _damageSystemHandler.GetHurtbox.AddAfterHitEvents(EnablePlayerActions);
            
            _damageSystemHandler.GetHurtbox.AddOnLoseEvents(DisablePlayerActions);
            _damageSystemHandler.GetHurtbox.AddOnLoseEvents(_animatorController.TriggerDying);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        #region Movement Properties

        protected bool IsJumpPressed { get; set; }
        protected bool IsJumping { get; set; }
        protected bool StartJump { get; set; }

        //names based on typical controller naming...
        protected float Vertical { get; set; }
        protected float Horizontal { get; set; }
        //name based on me not wanting to change Vertical & Horizontal to something better
        protected float Depth { get; set; } //(y/gravity)

        protected Vector3 Direction {
            get {
                var dir =
                    (transform.forward * (Horizontal * (_inverted ? -1 : 1))) +
                    (transform.up * Depth) +
                    (transform.right * (0.5f * Vertical * (_inverted ? 1 : -1))); //half rotation speed ?
                return dir;
            }
        }

        protected bool IsMoving => Mathf.Abs(_characterController.velocity.x) > 0.1;
    
        protected enum MovingOrientation
        {
            Idle,
            Forwards,
            Backwards
        }
    
        protected MovingOrientation GetMovingOrientation()
        {
            if (!IsMoving) return MovingOrientation.Idle;
            var dotProduct = Vector3.Dot(transform.forward, _characterController.velocity);
            if (dotProduct < 0) return MovingOrientation.Backwards;
            return MovingOrientation.Forwards;
        }
    
        protected Vector3 PushForce => _characterController.velocity.Equals(Vector3.zero) ?
            _currentSpeed :
            _characterController.velocity;

        #endregion

        public void BindControls()
        {
            throw new Exception("Bind controls AI not available");
        }

        public void BindControls(GeneratedPlayerControls controls)
        {
            throw new Exception("Bind controls Player not available");
        }
        
        protected void setUpJumpVariables() {
            float timeToApex = _maxJumpTime / 2;
            _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
        }
        
        protected void handleAnimations()
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

        protected void handleJump(){
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

        protected void handleGravity(){
            if(_characterController.isGrounded){
                Depth = _groundedGravity;
            }else{
                Depth += _gravity * Time.deltaTime;
            }
        }
        
        #region Attacks

        protected bool IsNeutralAttackPressed { get; set; }
        protected bool IsDownAttackPressed { get; set; }
        protected bool IsUpperAttackPressed { get; set; }

        #endregion
        
        protected void Move(Vector3 velForce){
            MoveCommand moveCommand = new MoveCommand(
                _moveCommandReceiver,
                velForce,
                _characterController);
            moveCommand.Execute();
            commands.Add(moveCommand);
            //_currentCmdNum++; memento lmao
        }

        protected void handleAttack()
        {
            // lowering cooldown
            if(_currentRythmCooldown > 0) _currentRythmCooldown -= Time.deltaTime;

            var isAttackPressed = IsNeutralAttackPressed || IsDownAttackPressed || IsUpperAttackPressed;
            if(!isAttackPressed) return;
        
            if(AppManager.Instance.GetGameMode() == GameMode.RHYTHM && _currentRythmCooldown <= 0) {
                _currentRythmCooldown = _rythmCooldown;
                var result = RhythmController.Instance.HitStrum(_playerId);

                var hitState =
                    result.Position >= 0 ?
                        result.Position < RhythmController.Instance.LatencyThreshold ?
                            result.Position == 0 ? RhythmState.Great : RhythmState.Good
                            : result.Position > -RhythmController.Instance.LatencyThreshold ?
                                RhythmState.Good
                                : RhythmState.Bad
                        : RhythmState.Bad;

                if(result.BeatNumber == -1){
                    hitState = RhythmState.Bad;
                }

                switch(hitState){
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

        
        public virtual void EnablePlayerActions() {}

        public virtual void DisablePlayerActions() {}

        public virtual void ResetPlayerActions()
        {
            Debug.Log($"Reseteando before: animacion Idle:{_animatorController.IsIdle()}");
            _animatorController.TriggerReset();
            Debug.Log($"Reseteando after: animacion Idle:{_animatorController.IsIdle()}");
            EnablePlayerActions();
        }
    }
}
