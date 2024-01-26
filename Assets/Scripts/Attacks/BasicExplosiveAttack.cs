using System.Collections;
using UnityEngine;

namespace Attacks
{
    public class BasicExplosiveAttack : ExplosiveAttack
    {
        public override void Execute()
        {
            base.Execute();
            StartCoroutine(ExplosionWait(_explosionDelay));
        }

        private IEnumerator ExplosionWait(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            Instantiate(_explosionPrefab, transform.position + transform.forward * _deltaPositionForward + Vector3.up * _deltaPositionY, transform.rotation);
        }
    }
}