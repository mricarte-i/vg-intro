using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private float _musicVolume, _effectsVolume, _masterVolume;
    private float _optionsMusicVolume, _optionsEffectsVolume, _optionsMasterVolume;

    public float MusicVolume => _musicVolume;
    public float EffectsVolume => _effectsVolume;
    public float MasterVolume => _masterVolume;


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

    public event Action<float> OnMasterVolumeChange;
    public void ChangeMasterVolume(float value){
        _optionsMasterVolume = value;
        _masterVolume = CalculateVolume(value);
        if(OnMasterVolumeChange != null) OnMasterVolumeChange(_masterVolume);
        //update other values...
        ChangeEffectsVolume(_optionsEffectsVolume);
        ChangeMusicVolume(_optionsMusicVolume);
    }

    public event Action<float> OnMusicVolumeChange;
    public void ChangeMusicVolume(float value){
        _optionsMusicVolume = value;
        _musicVolume = CalculateVolume(value) * MasterVolume;
        //if(_musicSource != null) _musicSource.volume = value;
        if(OnMusicVolumeChange != null) OnMusicVolumeChange(_musicVolume);
    }

    public event Action<float> OnEffectsVolumeChange;
    public void ChangeEffectsVolume(float value){
        _optionsEffectsVolume = value;
        _effectsVolume = CalculateVolume(value) * MasterVolume;
        if(OnEffectsVolumeChange != null) OnEffectsVolumeChange(_effectsVolume);
    }

    private float CalculateVolume(float volume){
        if(volume > 0f){
            return Mathf.Log10(volume) * 20f;
        }
        return -80f;
    }


}
