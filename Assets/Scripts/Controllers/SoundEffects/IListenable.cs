using UnityEngine;

public interface IListenable {
    AudioType AudioType { get; }
    AudioSource AudioSource { get; }
    AudioClip AudioClip { get; }

    void InitAudioSource();
    void Play(AudioClip clip);
    void Stop();
}