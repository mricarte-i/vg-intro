using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Entities;
using Flyweight;
using Strategy;
using UnityEngine;

namespace Attacks
{
  public class Attack : MonoBehaviour, IAttack
    {
        [SerializeField] protected AttackStats _attackStats;

        [SerializeField] protected List<Hitbox> _hitboxes = new List<Hitbox>();

        [SerializeField] protected GameObject _sfxPrefab;

        #region Hitter HurtBox

        protected LifeController _hurtbox;
        public void SetHurtBox(LifeController h) => _hurtbox = h;

        #endregion

        public List<Hitbox> Hitboxes => _hitboxes;
        public int Damage => _attackStats.Damage;
        public bool IsAttacking => _isAttacking;

        protected bool _isAttacking = false;

        public float Duration => _attackStats.Duration;

        private event Action BeforeAttackingEvents;
        private event Action AfterAttackingEvents;

        protected void InvokeBeforeAttackingEvents() => BeforeAttackingEvents?.Invoke();
        protected void InvokeAfterAttackingEvents() => AfterAttackingEvents?.Invoke();

        public void AddBeforeAttackingEvent(Action beforeAttackingAction) =>
            BeforeAttackingEvents += beforeAttackingAction;

        public void AddAfterAttackingEvent(Action afterAttackingAction) =>
            AfterAttackingEvents += afterAttackingAction;

        protected virtual void Awake()
        {
            foreach (var hitbox in _hitboxes)
            {
                hitbox.AddActionOnHit(MakeDamage);
            }
        }

        protected void MakeDamage(LifeController lifeController)
        {
            if(lifeController != _hurtbox){
                lifeController.GetHit(Damage);
                var sfxgo = Instantiate(_sfxPrefab);
                sfxgo.GetComponent<SoundEffectController>().Play(_attackStats.SFXCONTACT);
            }
        }

        public virtual void Execute()
        {
            _isAttacking = true;
            InvokeBeforeAttackingEvents();
            foreach (var hitbox in _hitboxes)
            {
                hitbox.gameObject.SetActive(true);
                var sfxgo = Instantiate(_sfxPrefab);
                sfxgo.GetComponent<SoundEffectController>().Play(_attackStats.SFXTHROW);
            }

            StartCoroutine(Wait(Duration));
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            foreach (var hitbox in _hitboxes)
            {
                hitbox.gameObject.SetActive(false);
            }

            _isAttacking = false;
            InvokeAfterAttackingEvents();
        }
    }
}