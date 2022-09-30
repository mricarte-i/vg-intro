using UnityEngine;

public interface IListenable {
    AudioSource AudioSource { get; }

    void InitAudioSource();
    void Play(AudioClip clip);
    void Stop();
}