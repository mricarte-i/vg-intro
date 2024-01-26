using System.Collections;
using Strategy;
using UnityEngine;

namespace Attacks
{
    public class MultipleExplosiveAttack : ExplosiveAttack
    {
        [SerializeField] private int _count;
        [SerializeField] private float _distanceBetween;
        
        public override void Execute()
        {
            base.Execute();
            StartCoroutine(ExplosionWait(_explosionDelay));
        }

        private IEnumerator ExplosionWait(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            for (var i = 0; i < _count; i++)
            {
                Instantiate(_explosionPrefab, transform.position + transform.forward * (_deltaPositionForward + i * _distanceBetween) + Vector3.up * _deltaPositionY, transform.rotation);
            }
        }
    }
}