using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Entities;
using Flyweight;
using JetBrains.Annotations;
using Strategy;
using UnityEngine;

namespace Attacks
{
    public class Attack : MonoBehaviour, IAttack
    {
        [SerializeField] private AttackStats _attackStats;

        [SerializeField] private List<GameObject> _hitboxes = new List<GameObject>();

        #region Hitter HurtBox

        private LifeController _hurtbox;
        public void SetHurtBox(LifeController h) => _hurtbox = h;

        #endregion

        public List<GameObject> Hitboxes => _hitboxes;
        public int Damage => _attackStats.Damage;
        public bool IsAttacking => _isAttacking;

        private bool _isAttacking = false;
        
        public float Duration => _attackStats.Duration;

        private event Action BeforeAttackingEvents;
        private event Action AfterAttackingEvents;

        public void AddBeforeAttackingEvent(Action beforeAttackingAction) =>
            BeforeAttackingEvents += beforeAttackingAction;
        
        public void AddAfterAttackingEvent(Action afterAttackingAction) =>
            AfterAttackingEvents += afterAttackingAction;

        private void Awake()
        {
            foreach (var hitboxGo in _hitboxes)
            {
                var hitbox = hitboxGo.GetComponent<Hitbox>();
                hitbox.AddActionOnHit(MakeDamage);
            }
        }

        private void MakeDamage(LifeController lifeController)
        {
            if(lifeController != _hurtbox) lifeController.GetHit(Damage);
        }

        public void Execute()
        {
            _isAttacking = true;
            BeforeAttackingEvents?.Invoke();
            foreach (var hitbox in _hitboxes)
            {
                hitbox.SetActive(true);
            }

            StartCoroutine(Wait(Duration));
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            foreach (var hitbox in _hitboxes)
            {
                hitbox.SetActive(false);
            }

            _isAttacking = false;
            AfterAttackingEvents?.Invoke();
        }
    }
}