using System;
using Attacks;
using UnityEngine;

namespace Controllers
{
    public class DamageSystemHandler : MonoBehaviour
    {
        [SerializeField] private Attack _neutralAttack;
        [SerializeField] private Attack _downAttack;
        [SerializeField] private Attack _upperAttack;
        [SerializeField] private LifeController _hurtbox;

        private void Start()
        {
            _neutralAttack.SetHurtBox(_hurtbox);
            _downAttack.SetHurtBox(_hurtbox);
            _upperAttack.SetHurtBox(_hurtbox);
        }

        public void DoNeutralAttack()
        {
            _neutralAttack.Execute();
        }

        public void DoDownAttack()
        {
            _downAttack.Execute();
        }

        public void DoUpperAttack()
        {
            _upperAttack.Execute();
        }
    }
}