using UnityEngine;
using System;
using System.Collections.Generic;

public class BeatInfo {
    public int BeatNumber;
    public float Position;
    public float _initialPosition;
    private float _secPerBeat;

    public BeatInfo(int bn, float pos) {
        BeatNumber = bn;
        Position = pos;
        _initialPosition = pos;
    }

    private BeatInfo(int bn, float pos, float _initialPosition) {
        BeatNumber = bn;
        Position = pos;
        _initialPosition = _initialPosition;
    }

    public BeatInfo Clone() {
        return new BeatInfo(BeatNumber, Position, _initialPosition);
    }

    public float Progress() {
        return (_initialPosition-Position) / _initialPosition;
    }
}

[RequireComponent(typeof(AudioSource))]
public class RhythmController : MonoBehaviour
{
    public static RhythmController Instance;

    #region Properties and Fields

    [Header("Audio related")]
    [SerializeField] private BgmData bgmData;
    private bool isInitialized = false;
    private AudioSource bgm;
    private float songPosition = 0f;
    private float songPosInBeats = 0f;
    private float secPerBeat;
    private float dsptimesong;

    [SerializeField] private bool isPlaying = true;
    private float pauseStartDsptime = 0;

    [Header("Rhythm related")]
    [SerializeField] private float _beatFutureFrameWindow = 2f;
    [SerializeField] private float _greatTimeframeRatio = 0.12f;
    [SerializeField] private float _goodTimeframeRatio = 0.36f;
    [SerializeField] private AudioClip tickClip;
    private AudioSource tick;

    private int _maxSongBeats;
    [SerializeField] private float _latencySafeZone = 0.12f;
    public float LatencyThreshold => _latencySafeZone;
    //players
    private LinkedList<BeatInfo> _possibleBeatsP1 = new LinkedList<BeatInfo>();
    private LinkedList<BeatInfo> _possibleBeatsP2 = new LinkedList<BeatInfo>();

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        AppManager.Instance.OnPause += OnAppPause;
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
            Destroy(this); //end this man
        }

        //calculate the position in seconds
        songPosition = (float) (AudioSettings.dspTime - dsptimesong);

        //calculate the position in beats & check if changed
        if ((int) songPosInBeats < (int)(songPosition / secPerBeat)) QueueNextTick();
        songPosInBeats = songPosition / secPerBeat;

        UpdatePossibleBeats(_possibleBeatsP1);
        UpdatePossibleBeats(_possibleBeatsP2);
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

    private void OnAppPause(bool isPaused){
        if(isPaused){
            Pause();
        }else{
            Play();
        }
    }

    public void Init(BgmData data)
    {
        if(isInitialized) throw new InvalidOperationException("Already initialized.");

        bgmData = data;

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

        _maxSongBeats = Mathf.FloorToInt(bgmData.BGM.length / secPerBeat);

        UpdatePossibleBeats(_possibleBeatsP1);
        UpdatePossibleBeats(_possibleBeatsP2);
    }

    private void UpdatePossibleBeats(LinkedList<BeatInfo> possibleBeats) {
        if(possibleBeats.Count != 0){
            //update all beatinfo's positions based on current time (songPosition);
            var beatsToRemove = new List<BeatInfo>();

            foreach(BeatInfo beatInfo in possibleBeats){
                beatInfo.Position = (beatInfo.BeatNumber * secPerBeat) - songPosition;
                if(beatInfo.Position < -_latencySafeZone) {
                    //gone over the forgivable latency threshold
                    beatsToRemove.Add(beatInfo);
                }
            }

            //remove some beats...
            beatsToRemove.ForEach((b) => possibleBeats.Remove(b));
        }

        BeatInfo youngestBeat = null;
        if(possibleBeats.Last != null) youngestBeat = possibleBeats.Last.Value;
        if(youngestBeat == null || youngestBeat.BeatNumber < _maxSongBeats){
            //add all possible next beats (within the allowed time frame)
            var time = Math.Min(_beatFutureFrameWindow, bgmData.BGM.length - songPosition);
            var numberOfNewBeats =  time * secPerBeat;
            var newestBeat = Mathf.FloorToInt(songPosInBeats + numberOfNewBeats);

            for(int beat = Mathf.FloorToInt(songPosInBeats); beat <= newestBeat; beat++){
                if(youngestBeat == null || beat > youngestBeat.BeatNumber){
                    possibleBeats.AddLast(new BeatInfo(beat, (beat * secPerBeat) - songPosition));
                }
            }
        }
    }

    public BeatInfo HitStrum(PlayerId id){
        return GetPossibleBeat(GetPossibleBeats(id));
    }

    public LinkedList<BeatInfo> GetPossibleBeats(PlayerId id){
        switch(id){
            case PlayerId.P1:
                return _possibleBeatsP1;
            case PlayerId.P2:
                return _possibleBeatsP2;
            default:
                throw new InvalidOperationException("GetPossibleBeats Illegal Player ID " + id);
        }
    }

    private BeatInfo GetPossibleBeat(LinkedList<BeatInfo> possibleBeats){
        BeatInfo ret = new BeatInfo(-1, -1f);

        var peekBeat = possibleBeats.First;
        if(peekBeat != null && Mathf.Abs(peekBeat.Value.Position) < _latencySafeZone) {
            ret = peekBeat.Value.Clone();
            possibleBeats.Remove(peekBeat);
        }
        Debug.Log("beat given " + ret.BeatNumber + ret.Position);

        return ret;
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

    public RhythmState GetBeat()
    {
        // works with data updated from update

        float timeToPrev = songPosInBeats - (GetPrevBeatPosition() - dsptimesong);
        float timeToNext = (GetNextBeatPosition() - dsptimesong) - songPosition;

        // always work with next if lastBeat is prev
        float closestTime = (timeToPrev < timeToNext) ? timeToPrev : timeToNext;

        RhythmState rhythmState;
        if(closestTime < _greatTimeframeRatio * secPerBeat)
        {
            rhythmState = RhythmState.Great;
        }
        else if (closestTime < _goodTimeframeRatio * secPerBeat)
        {
            rhythmState = RhythmState.Good;
        }
        else
        {
            rhythmState = RhythmState.Bad;
        }

        return rhythmState;
    }

    #region EVENT_ACTIONS
    private void OnEffectsVolumeChange(float value){
        tick.volume = value;
    }

    private void OnMusicVolumeChange(float value){
        bgm.volume = value;
    }

    #endregion
}