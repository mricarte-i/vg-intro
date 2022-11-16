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

        private enum AnimationType
        {
            Idle,
            WalkingForwards,
            WalkingBackwards,
            Jump
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _currentAnimation = AnimationType.Idle;
        }

        private static readonly string TRIGGER_IDLE = "TriggerIdle";
        private static readonly string TRIGGER_WALKING_FORWARDS = "TriggerWalkingForwards";
        private static readonly string TRIGGER_WALKING_BACKWARDS = "TriggerWalkingBackwards";
        private static readonly string TRIGGER_JUMP = "TriggerJump";

        private static readonly Dictionary<AnimationType, string> TRIGGER_NAME = new Dictionary<AnimationType, string>
        {
            { AnimationType.Idle, TRIGGER_IDLE },
            { AnimationType.WalkingForwards, TRIGGER_WALKING_FORWARDS },
            { AnimationType.WalkingBackwards, TRIGGER_WALKING_BACKWARDS },
            { AnimationType.Jump, TRIGGER_JUMP }
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
            if (IsJumping()) return;
            _currentAnimation = AnimationType.Jump;
            SetAnimationParameters(AnimationType.Jump);
        }

        #endregion
    }
}