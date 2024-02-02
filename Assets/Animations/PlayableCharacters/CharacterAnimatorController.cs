using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Strategy;
using UnityEngine;

namespace Animations
{
    public class CharacterAnimatorController : MonoBehaviour, ICharaterAnimatorController
    {
        private Animator _animator;
        private AnimationType _currentAnimation;
        private bool _deathBed = false;

        private enum AnimationType
        {
            Idle,
            WalkingForwards,
            WalkingBackwards,
            Jump,
            
            TakeDamage,
            Dying,
            Reset,
            
            UpperAttack,
            NeutralAttack,
            DownAttack
        }

        private void Awake()
        {
            _currentAnimation = AnimationType.Idle;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private static readonly string TRIGGER_IDLE = "TriggerIdle";
        private static readonly string TRIGGER_WALKING_FORWARDS = "TriggerWalkingForwards";
        private static readonly string TRIGGER_WALKING_BACKWARDS = "TriggerWalkingBackwards";
        private static readonly string TRIGGER_JUMP = "TriggerJump";

        private static readonly string TRIGGER_TAKE_DAMAGE = "TriggerTakeDamage";
        private static readonly string TRIGGER_DYING = "TriggerDying";
        private static readonly string TRIGGER_RESET = "TriggerReset";
        
        private static readonly string TRIGGER_UPPER_ATTACK = "TriggerUpperAttack";
        private static readonly string TRIGGER_NEUTRAL_ATTACK = "TriggerNeutralAttack";
        private static readonly string TRIGGER_DOWN_ATTACK = "TriggerDownAttack";

        private static readonly Dictionary<AnimationType, string> TRIGGER_NAME = new Dictionary<AnimationType, string>
        {
            { AnimationType.Idle, TRIGGER_IDLE },
            { AnimationType.WalkingForwards, TRIGGER_WALKING_FORWARDS },
            { AnimationType.WalkingBackwards, TRIGGER_WALKING_BACKWARDS },
            { AnimationType.Jump, TRIGGER_JUMP },

            { AnimationType.TakeDamage, TRIGGER_TAKE_DAMAGE },
            { AnimationType.Dying, TRIGGER_DYING },
            { AnimationType.Reset, TRIGGER_RESET},

            { AnimationType.UpperAttack, TRIGGER_UPPER_ATTACK },
            { AnimationType.NeutralAttack, TRIGGER_NEUTRAL_ATTACK },
            { AnimationType.DownAttack, TRIGGER_DOWN_ATTACK }
        };

        private void SetAnimationParameters(AnimationType animationType)
        {
            _animator.SetTrigger(TRIGGER_NAME[animationType]);
        }

        #region Idle

        public bool IsIdle()
        {
            return _currentAnimation == AnimationType.Idle;
        }

        public void TriggerIdle()
        {
            if (_deathBed)
            {
                if (IsDying()) return;
                TriggerDying();
            }
            if (IsIdle()) return;
            _currentAnimation = AnimationType.Idle;
            SetAnimationParameters(AnimationType.Idle);
        }

        #endregion
        
        #region Walking Forwards

        public bool IsWalkingForwards()
        {
            return _currentAnimation == AnimationType.WalkingForwards;
        }

        public void TriggerWalkingForwards()
        {
            if (_deathBed)
            {
                if (IsDying()) return;
                TriggerDying();
            }
            if (IsWalkingForwards()) return;
            _currentAnimation = AnimationType.WalkingForwards;
            SetAnimationParameters(AnimationType.WalkingForwards);
        }

        #endregion
        
        #region Walking Backwards

        public bool IsWalkingBackwards()
        {
            return _currentAnimation == AnimationType.WalkingBackwards;
        }

        public void TriggerWalkingBackwards()
        {
            if (_deathBed)
            {
                if (IsDying()) return;
                TriggerDying();
            }
            if (IsWalkingBackwards()) return;
            _currentAnimation = AnimationType.WalkingBackwards;
            SetAnimationParameters(AnimationType.WalkingBackwards);
        }

        #endregion

        #region Jump

        public bool IsJumping()
        {
            return _currentAnimation == AnimationType.Jump;
        }

        public void TriggerJump()
        {
            if (_deathBed)
            {
                if (IsDying()) return;
                TriggerDying();
            }
            if (IsJumping()) return;
            _currentAnimation = AnimationType.Jump;
            SetAnimationParameters(AnimationType.Jump);
        }

        #endregion

        #region Damage

        public bool IsTakeDamage()
        {
            return _currentAnimation == AnimationType.TakeDamage;
        }

        public void TriggerTakeDamage()
        {
            if (_deathBed)
            {
                if (IsDying()) return;
                TriggerDying();
            }
            if (IsTakeDamage()) return;
            _currentAnimation = AnimationType.TakeDamage;
            SetAnimationParameters(AnimationType.TakeDamage);
        }
        
        public bool IsDying()
        {
            return _currentAnimation == AnimationType.Dying;
        }

        public void TriggerDying()
        {
            _deathBed = true;
            if (IsDying()) return;
            _currentAnimation = AnimationType.Dying;
            SetAnimationParameters(AnimationType.Dying);
        }
        
        public bool IsReseting()
        {
            return _currentAnimation == AnimationType.Reset;
        }

        public void TriggerReset()
        {
            _deathBed = false;
            if (IsDying()) return;
            _currentAnimation = AnimationType.Idle;
            SetAnimationParameters(AnimationType.Reset);
        }

        #endregion

        #region Attacks

        public bool IsUpperAttack()
        {
            return _currentAnimation == AnimationType.UpperAttack;
        }

        public void TriggerUpperAttack()
        {
            if (_deathBed)
            {
                if (IsDying()) return;
                TriggerDying();
            }
            if (IsUpperAttack()) return;
            _currentAnimation = AnimationType.UpperAttack;
            SetAnimationParameters(AnimationType.UpperAttack);
        }
        
        public bool IsNeutralAttack()
        {
            return _currentAnimation == AnimationType.NeutralAttack;
        }

        public void TriggerNeutralAttack()
        {
            if (_deathBed)
            {
                if (IsDying()) return;
                TriggerDying();
            }
            if (IsNeutralAttack()) return;
            _currentAnimation = AnimationType.NeutralAttack;
            SetAnimationParameters(AnimationType.NeutralAttack);
        }
        
        public bool IsDownAttack()
        {
            return _currentAnimation == AnimationType.DownAttack;
        }

        public void TriggerDownAttack()
        {
            if (_deathBed)
            {
                if (IsDying()) return;
                TriggerDying();
            }
            if (IsDownAttack()) return;
            _currentAnimation = AnimationType.DownAttack;
            SetAnimationParameters(AnimationType.DownAttack);
        }

        #endregion

        private void Update()
        {
            if (!_deathBed) return;
            if (_currentAnimation == AnimationType.Reset) return;
            if (IsDying()) return;
            TriggerDying();
        }
    }
}