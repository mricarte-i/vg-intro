using System.Collections;
using Controllers;
using UnityEngine;

namespace Attacks
{
  public class RaycastAttack : Attack {

        [SerializeField] private GameObject _beamPrefab;
        [SerializeField] private float _range = 6f;
        private GameObject _spawnedBeam;

        private bool _endingAttack = false;

        public override void Execute(){
            _isAttacking = true;
            InvokeBeforeAttackingEvents();
            StartCoroutine(WaitStart(Duration/3));

            StartCoroutine(WaitEnd(Duration));
        }

        private IEnumerator WaitStart(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            _spawnedBeam = Instantiate(_beamPrefab, transform);
            var sfxgo = Instantiate(_sfxPrefab);
            sfxgo.GetComponent<SoundEffectController>().Play(_attackStats.SFXTHROW);
        }

        private void Update(){
            if(_isAttacking && !_endingAttack) {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit, _range)){

                    Debug.Log(hit.transform.name);

                    var lifeControllerHit = hit.transform.GetComponentInChildren<LifeController>();

                    //Debug.Log(lifeControllerHit != null);
                    //Debug.Log(lifeControllerHit != _hurtbox);

                    if(lifeControllerHit != null && lifeControllerHit != _hurtbox){
                        MakeRayDamage(lifeControllerHit);
                    }
                }
            }
        }

        protected void MakeRayDamage(LifeController lifeController)
        {
            if(lifeController != _hurtbox){
                lifeController.GetHit(Damage*Time.deltaTime);
                var sfxgo = Instantiate(_sfxPrefab);
                sfxgo.GetComponent<SoundEffectController>().Play(_attackStats.SFXCONTACT);
            }
        }

        private IEnumerator WaitEnd(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            _endingAttack = true;
            if(_spawnedBeam != null){
                Destroy(_spawnedBeam);
                _spawnedBeam = null;
            }
            _isAttacking = false;
            _endingAttack = false;
            InvokeAfterAttackingEvents();
        }

    }
}
