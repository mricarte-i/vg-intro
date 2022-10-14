using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks
{
    public class BowlingThrowAttack : Attack
    {
        [SerializeField] private float _rollingSpeed;
        private Dictionary<string, Vector3> _hitboxesInitialPositions;
        private bool _endingAttack = false;

        protected override void Awake()
        {
            base.Awake();
            _hitboxesInitialPositions = new Dictionary<string, Vector3>();
            foreach (var hitbox in _hitboxes)
            {
                var hitboxGameObject = hitbox.gameObject;
                _hitboxesInitialPositions.Add(hitboxGameObject.name, hitboxGameObject.transform.localPosition);
            }
        }
        
        public override void Execute()
        {
            _isAttacking = true;
            InvokeBeforeAttackingEvents();
            foreach (var hitbox in _hitboxes)
            {
                var hitboxGameObject = hitbox.gameObject;
                hitboxGameObject.SetActive(true);
                hitboxGameObject.transform.Translate(Vector3.forward * (Time.deltaTime * _rollingSpeed));
            }

            StartCoroutine(Wait(Duration));
        }

        private void Update()
        {
            if (_isAttacking && !_endingAttack)
            {
                foreach (var hitbox in _hitboxes)
                {
                    hitbox.gameObject.transform.Translate(Vector3.forward * (Time.deltaTime * _rollingSpeed));
                }
            }
            
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            _endingAttack = true;
            foreach (var hitbox in _hitboxes)
            {
                var hitboxGameObject = hitbox.gameObject;
                hitboxGameObject.SetActive(false);
                hitboxGameObject.transform.localPosition = _hitboxesInitialPositions[hitboxGameObject.name];
            }
            _isAttacking = false;
            _endingAttack = false;
            InvokeAfterAttackingEvents();
        }
    }
}