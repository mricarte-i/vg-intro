using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private float _musicVolume, _effectsVolume;


    void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip){
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void ChangeMasterVolume(float value){
        AudioListener.volume = value;
    }

    public void ChangeMusicVolume(float value){
        _musicVolume = value;
        _musicSource.volume = value;
    }

    public event Action<float> OnEffectsVolumeChange;
    public void ChangeEffectsVolume(float value){
        _effectsVolume = value;
        OnEffectsVolumeChange(value);
    }


}
