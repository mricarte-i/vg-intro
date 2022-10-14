using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class RythmController : MonoBehaviour
{
    public static RythmController Instance;

    #region Properties and Fields

    [Header("Audio related")]
    [SerializeField] private BgmData bgmData;
    private AudioSource bgm;
    private float songPosition = 0f;
    private float songPosInBeats = 0f;
    private float secPerBeat;
    private float dsptimesong;
    [SerializeField] private bool isPlaying;
    private float pauseStartDsptime = 0;
    [SerializeField] private AudioSource _bgmSource, _tickSource;

    [Header("Rhythm related")]
    [SerializeField] private float _greatTimeframeRatio = 0.2f;
    [SerializeField] private float _goodTimeframeRatio = 0.5f;
    [SerializeField] private AudioClip tickClip;
    private AudioSource tick;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //AudioSource[] audioSources = GetComponents<AudioSource>();

        // bgm related
        bgm = _bgmSource;
        secPerBeat = 60f / bgmData.Bpm;
        dsptimesong = (float) AudioSettings.dspTime;
        bgm.clip = bgmData.BGM;
        bgm.PlayOneShot(bgmData.BGM);

        // beat marker related
        tick = _tickSource;
        tick.clip = tickClip;
        QueueNextTick();

        // volume change
        AudioManager.Instance.OnEffectsVolumeChange += OnEffectsVolumeChange;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlaying) return;

        if(!bgm.isPlaying){
            //TODO: tell EventsManager.Instance.EventTimeout()
        }

        //calculate the position in seconds
        songPosition = (float) (AudioSettings.dspTime - dsptimesong);

        //calculate the position in beats & check if changed
        if ((int) songPosInBeats < (int)(songPosition / secPerBeat)) QueueNextTick();
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

    public event Action<bool> OnPause;
    void Play()
    {
        if(isPlaying) return;

        isPlaying = true;
        dsptimesong += ((float) AudioSettings.dspTime - pauseStartDsptime);
        pauseStartDsptime = 0f;
        bgm.UnPause();

        if(OnPause != null) OnPause(false);

        QueueNextTick();
    }

    void Pause()
    {
        if(!isPlaying) return;

        isPlaying = false;
        pauseStartDsptime = (float) AudioSettings.dspTime;
        bgm.Pause();

        if(OnPause != null) OnPause(true);

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
    public enum RythmState
    {
        Great,
        Good,
        Bad
    };
}
