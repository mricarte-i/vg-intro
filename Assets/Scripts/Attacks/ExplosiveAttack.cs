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
        
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private float _deltaPositionForward;
        [SerializeField] private float _deltaPositionY;
        [SerializeField] private float _explosionDelay;

        public override void Execute()
        {
            base.Execute();
            StartCoroutine(Wait(_explosionDelay));
        }
        
        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            Instantiate(_explosionPrefab, transform.position + transform.forward * _deltaPositionForward + Vector3.up * _deltaPositionY, transform.rotation);
        }
    }
}