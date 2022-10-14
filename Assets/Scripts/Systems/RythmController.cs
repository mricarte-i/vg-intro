using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class RythmController : MonoBehaviour
{
    public static RythmController Instance;

    #region Properties and Fields

    [Header("Audio related")]
    [SerializeField] private BgmData bgmData;
    private bool isInitialized = false;
    private AudioSource bgm;
    private float songPosition = 0f;
    private float songPosInBeats = 0f;
    private float secPerBeat;
    private float dsptimesong;
    [SerializeField] private bool isPlaying;
    private float pauseStartDsptime = 0;

    [Header("Rhythm related")]
    [SerializeField] private float _greatTimeframeRatio = 0.2f;
    [SerializeField] private float _goodTimeframeRatio = 0.5f;
    [SerializeField] private AudioClip tickClip;
    private AudioSource tick;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // volume change
        AudioManager.Instance.OnEffectsVolumeChange += OnEffectsVolumeChange;
        AudioManager.Instance.OnMusicVolumeChange += OnMusicVolumeChange;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInitialized || !isPlaying) return;

        if(!bgm.isPlaying){
            EventsManager.Instance.EventTimeout();
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
    public void Play()
    {
        if(isPlaying) return;

        isPlaying = true;
        dsptimesong += ((float) AudioSettings.dspTime - pauseStartDsptime);
        pauseStartDsptime = 0f;
        bgm.UnPause();

        if(OnPause != null) OnPause(false);

        QueueNextTick();
    }

    public void Pause()
    {
        if(!isPlaying) return;

        isPlaying = false;
        pauseStartDsptime = (float) AudioSettings.dspTime;
        bgm.Pause();

        if(OnPause != null) OnPause(true);

    }

    public void setData(BgmData data)
    {
        if(isInitialized) throw new InvalidOperationException("Already initialized. Can setData only if not initialized");
        bgmData = data;
    }
    
    private void init()
    {
        if(isInitialized) throw new InvalidOperationException("Already initialized");

        AudioSource[] audioSources = GetComponents<AudioSource>();

        // bgm related
        bgm = audioSources[0];
        secPerBeat = 60f / bgmData.Bpm;
        dsptimesong = (float) AudioSettings.dspTime;
        bgm.clip = bgmData.BGM;
        bgm.PlayOneShot(bgmData.BGM);
        if(!isPlaying) bgm.Pause();

        // beat marker related
        tick = audioSources[1];
        tick.clip = tickClip;
        if(isPlaying) QueueNextTick();

        isInitialized = true;
    }

    public float getBeatProgress() {
        return songPosInBeats%1;
    }

    private float GetPrevBeatPosition()
    {
        return dsptimesong + ((int) songPosInBeats) * secPerBeat;
    }

    private float GetNextBeatPosition()
    {
        return dsptimesong + ((int) songPosInBeats + 1) * secPerBeat;
    }

    private void QueueNextTick()
    {
        // dsptime to queue next tick
        float nextTickTime = GetNextBeatPosition();
        tick.PlayScheduled(nextTickTime);
    }

    RythmState GetBeat()
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
    private void OnEffectsVolumeChange(float value){
        tick.volume = value;
    }

    private void OnMusicVolumeChange(float value){
        bgm.volume = value;
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
