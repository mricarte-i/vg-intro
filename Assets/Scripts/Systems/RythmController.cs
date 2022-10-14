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
    private float songPosition = 0;
    private float songPosInBeats = 0;
    private float secPerBeat;
    private float dsptimesong;
    private int completedLoops;
    private float loopPositionInBeats;

    [Header("Rhythm related")]
    [SerializeField] private float _greatTimeframe = 3.0F / 60;
    [SerializeField] private float _goodTimeframe = 9.0F / 60;
    [SerializeField] private AudioSource tick;
    private boolean onBeat = false;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        // bgm related
        secPerBeat = 60f / bgmData.Bpm;
        dsptimesong = (float) AudioSettings.dspTime;
        bgmData.Bgm.Play();

        // beat marker related
        tick = GetComponent<AudioSource>();
        QueueNextTick();

        // volume change
        AudioManager.Instance.OnEffectsVolumeChange += OnEffectsVolumeChange;
    }

    // Update is called once per frame
    void Update()
    {
        //calculate the position in seconds
        songPosition = (float) (AudioSettings.dspTime - dsptimesong - bgmData.FirstBeatOffset);

        //calculate the position in beats & check if changed
        if ((int) songPosInBeats < (int)(songPosition / secPerBeat)) {
            onBeat = true;
            QueueNextTick();
        }
        songPosInBeats = songPosition / secPerBeat;
        
        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;
    }
    
    void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
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

    RythmObject getBeat(int lastBeat)
    {
        // works with data updated from update
        RythmObject rythmObject = new RythmObject();
        rythmObject.currentBeat = songPosInBeats;

        float timeToPrev = GetPrevBeatPosition() - songPosition;
        float timeToNext = GetNextBeatPosition() - songPosition;

        // always work with next if lastBeat is prev
        float closestTime;
        if (lastBeat == songPosInBeats) {
            closestTime = timeToNext;
        } else {
            closestTime = (timeToNext + timeToPrev > 0)? timeToPrev : timeToNext;
        }
        rythmObject.closestBeatTime = closestTime;

        RythmState rythmState;
        if(closestTime < _greatTimeframe/2)
        {
            rythmState = RythmState.Great;
        }
        else if (closestTime < _goodTimeframe/2)
        {
            rythmState = RythmState.Good;
        }
        else
        {
            rythmState = RythmState.Bad;
        }
        rythmObject.rythmState = rythmState;

        return rythmObject;
    }

    #region EVENT_ACTIONS
    private void OnEffectsVolumeChange(float value)
    {
        bgmData.volume = value;
        tick.volume = value;
    }

    #endregion
    
    // helpers
    struct RythmObject
    {
        public RythmState rythmState;
        public int currentBeat;
        public float closestBeatTime;
    }

    enum RythmState: string
    {
        Great = "GREAT",
        Good = "GOOD",
        Bad = "BAD"
    };
}
