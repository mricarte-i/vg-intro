using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RythmController : MonoBehaviour
{
    public static RythmController Instance;
    
    #region Properties and Fields

    [Header("Audio related")]
    [SerializeField] private BgmData bgmData;
    private AudioSource bgm;
    private float songPosition = 0;
    private float songPosInBeats = 0;
    private float secPerBeat;
    private float dsptimesong;
    private float isPlaying;
    private float pauseStartDsptime = 0f;

    [Header("Rhythm related")]
    [SerializeField] private float _greatTimeframeRatio = 0.2f;
    [SerializeField] private float _goodTimeframeRatio = 0.5f;
    [SerializeField] private AudioClip tickClip;
    private AudioSource tick;
    private boolean onBeat = false;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        // bgm related
        bgm = audioSources[0];
        secPerBeat = 60f / bgmData.Bpm;
        dsptimesong = (float) AudioSettings.dspTime;
        bgmData.Bgm.Play();

        // beat marker related
        tick = audioSources[1];
        QueueNextTick();

        // volume change
        AudioManager.Instance.OnEffectsVolumeChange += OnEffectsVolumeChange;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlaying) return;

        //calculate the position in seconds
        songPosition = (float) (AudioSettings.dspTime - dsptimesong);

        //calculate the position in beats & check if changed
        if ((int) songPosInBeats < (int)(songPosition / secPerBeat)) {
            onBeat = true;
            QueueNextTick();
        }
        songPosInBeats = songPosition / secPerBeat;
    }
    
    void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    void Play()
    {
        if(isPlaying) return;
        
        isPlaying = true;
        dsptimesong += (AudioSettings.dspTime - pauseStartDsptime);
        pauseStartDsptime = 0f;
        bgm.Play();

        QueueNextTick();
    }

    void Pause()
    {
        if(!isPlaying) return;

        isPlaying = false;
        pauseStartDsptime = AudioSettings.dspTime;
        bgm.Pause();
    }

    float GetPrevBeatPosition()
    {
        return dsptimesong + ((int) songPosInBeats) * secPerBeat;
    }
    
    float GetNextBeatPosition()
    {
        return dsptimesong + ((int) songPosInBeats + 1) * secPerBeat;
    }

    void QueueNextTick()
    {
        // dsptime to queue next tick
        float nextTickTime = GetNextBeatPosition();
        tick.PlayScheduled(nextTickTime);
    }

    RythmState getBeat()
    {
        // works with data updated from update

        float timeToPrev = GetPrevBeatPosition() - songPosition;
        float timeToNext = GetNextBeatPosition() - songPosition;

        // always work with next if lastBeat is prev
        float closestTime = (timeToNext + timeToPrev > 0)? timeToPrev : timeToNext;

        RythmState rythmState;
        if(closestTime < _greatTimeframeRatio * secPerBeat)
        {
            rythmState = RythmState.Great;
        }
        else if (closestTime < _goodTimeframeRatio * secPerBeat)
        {
            rythmState = RythmState.Good;
        }
        else
        {
            rythmState = RythmState.Bad;
        }

        return rythmState;
    }

    #region EVENT_ACTIONS
    private void OnEffectsVolumeChange(float value)
    {
        bgm.volume = value;
        tick.volume = value;
    }

    #endregion

    // helpers
    enum RythmState
    {
        Great,
        Good,
        Bad
    };
}
