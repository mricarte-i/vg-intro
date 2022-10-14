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

        [SerializeField] private float _duration;
        public float Duration => _duration;

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
            foreach (var hitbox in _hitboxes)
            {
                hitbox.SetActive(true);
            }

            StartCoroutine(Wait(_duration));
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            foreach (var hitbox in _hitboxes)
            {
                hitbox.SetActive(false);
            }

            _isAttacking = false;
        }
    }
}