using UnityEngine;

namespace Animations.Stages
{
    public class Lightning : MonoBehaviour
    {
        [SerializeField] private GameObject _sfxPrefab;
        [SerializeField] private AudioClip _lightningSound;

        public void BringInTheThunder()
        {
            var sfxgo = Instantiate(_sfxPrefab);
            sfxgo.GetComponent<SoundEffectController>().Play(_lightningSound);
        }
    }
}