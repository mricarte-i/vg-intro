using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectController : MonoBehaviour, IListenable
{
    public AudioSource AudioSource => _audioSource;
    private AudioSource _audioSource;


    public void InitAudioSource()
    {
        // Asignar el componente AudioSource
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip) {
        AudioSource.PlayOneShot(clip);
    }

    public void Stop() => AudioSource.Stop();


    void Start()
    {
        InitAudioSource();
        AudioManager.Instance.OnEffectsVolumeChange += OnEffectsVolumeChange;
    }


    #region EVENT_ACTIONS
    private void OnEffectsVolumeChange(float value)
    {
        AudioSource.volume = value;
    }

    #endregion
}
