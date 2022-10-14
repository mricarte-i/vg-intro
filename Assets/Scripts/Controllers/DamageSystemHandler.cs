using System;
using Attacks;
using Strategy;
using UnityEngine;

namespace Controllers
{
  public class DamageSystemHandler : MonoBehaviour
    {
        [SerializeField] private Attack _neutralAttack;
        [SerializeField] private Attack _downAttack;
        [SerializeField] private Attack _upperAttack;
        [SerializeField] private LifeController _hurtbox;
        public LifeController GetHurtbox => _hurtbox;

        public bool IsCurrentlyAttacking => _isCurrentlyAttacking;
        private bool _isCurrentlyAttacking = false;

        private void SetAsNotAttacking() => _isCurrentlyAttacking = false;
        private void SetAsAttacking() => _isCurrentlyAttacking = true;

        private void Start()
        {
            InitAttack(_neutralAttack);
            InitAttack(_downAttack);
            InitAttack(_upperAttack);
        }

        private void InitAttack(Attack attack)
        {
            attack.SetHurtBox(_hurtbox);
            attack.AddBeforeAttackingEvent(SetAsAttacking);
            attack.AddAfterAttackingEvent(SetAsNotAttacking);
        }

        #region Attack

        private void DoAttack(Attack attack)
        {
            if (!_isCurrentlyAttacking)
            {
                attack.Execute();
            }
        }

        public void DoNeutralAttack() => DoAttack(_neutralAttack);

        public void DoDownAttack() => DoAttack(_downAttack);

        public void DoUpperAttack() => DoAttack(_upperAttack);

        #endregion


        #region OnAttack Events

        public void AddBeforeAttackingEvent(Action beforeAttackingAction)
        {
            _neutralAttack.AddBeforeAttackingEvent(beforeAttackingAction);
            _downAttack.AddBeforeAttackingEvent(beforeAttackingAction);
            _upperAttack.AddBeforeAttackingEvent(beforeAttackingAction);
        }

        public void AddAfterAttackingEvent(Action afterAttackingAction)
        {
            _neutralAttack.AddAfterAttackingEvent(afterAttackingAction);
            _downAttack.AddAfterAttackingEvent(afterAttackingAction);
            _upperAttack.AddAfterAttackingEvent(afterAttackingAction);
        }

        #endregion
    }
}