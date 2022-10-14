using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectController : MonoBehaviour, IListenable
{
    public AudioSource AudioSource => _audioSource;
    private AudioSource _audioSource;

    public AudioClip AudioClip => _audioClip;
    private AudioClip _audioClip;

    public AudioType AudioType => _audioType;
    [SerializeField] private AudioType _audioType = AudioType.EFFECTS;


    public void InitAudioSource()
    {
        // Asignar el componente AudioSource
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip) {
        AudioSource.PlayOneShot(clip);
    }

    public void Play() => AudioSource.PlayOneShot(AudioClip);

    public void Stop() => AudioSource.Stop();


    void Start()
    {
        InitAudioSource();

        if(AudioType == AudioType.EFFECTS) AudioManager.Instance.OnEffectsVolumeChange += OnEffectsVolumeChange;

        if(AudioType == AudioType.MUSIC) AudioManager.Instance.OnMusicVolumeChange += OnMusicVolumeChange;

        AudioManager.Instance.OnMasterVolumeChange += OnMasterVolumeChange;
    }


    #region EVENT_ACTIONS
    private void OnEffectsVolumeChange(float value)
    {
        AudioSource.volume = value;
    }

    private void OnMusicVolumeChange(float value)
    {
        AudioSource.volume = value;
    }

    private void OnMasterVolumeChange(float value)
    {
        //maybe this should scale based on effects?
        AudioSource.volume = value * (AudioType == AudioType.EFFECTS ? AudioManager.Instance.EffectsVolume : AudioManager.Instance.MusicVolume);
    }

    #endregion
}
