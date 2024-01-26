using System.Collections;
using Strategy;
using UnityEngine;

namespace Attacks
{
    public class ExplosiveAttack : BasicAttack, IExplosiveAttack
    {
        public GameObject ExplosionPrefab => _explosionPrefab;
        public float DeltaPositionForward => _deltaPositionForward;
        public float DeltaPositionY=> _deltaPositionY;
        public float ExplosionDelay => _explosionDelay;
        
        [SerializeField] protected GameObject _explosionPrefab;
        [SerializeField] protected float _deltaPositionForward;
        [SerializeField] protected float _deltaPositionY;
        [SerializeField] protected float _explosionDelay;

        public override void Execute()
        {
            base.Execute();
        }
        
        private IEnumerator ExplosionWait(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            Instantiate(_explosionPrefab, transform.position + transform.forward * _deltaPositionForward + Vector3.up * _deltaPositionY, transform.rotation);
        }
    }
}