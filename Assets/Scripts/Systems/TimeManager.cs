using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class TimeManager: MonoBehaviour
{
    public static TimeManager Instance;

    [SerializeField] private AudioMixer[] audioMixers;
    
    private float _baseTimeScale;
    private float _slowDown;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _baseTimeScale = 1f;
        _slowDown = _baseTimeScale;
        Time.timeScale = _baseTimeScale;
    }
    
    /**
     * Updates time slowly going back to normal time if it was slowed down
     * It also changes pitch of audio mixers to go along those changes
     */
    private void FixedUpdate()
    {
        if (_slowDown >= _baseTimeScale) return;
        
        _slowDown = Mathf.MoveTowards(_slowDown, _baseTimeScale, 0.02f);
        Time.timeScale = _slowDown;

        var pitchValue = _slowDown >= _baseTimeScale ? _baseTimeScale : (_slowDown / _baseTimeScale);
            
        foreach (var mixer in audioMixers)
        {
            mixer.SetFloat("allPitch", pitchValue);
        }
    }

    /**
     * Slows down time to given amount, time will gradually return to normal
     * Times it takes to go back to normal depends on how small given amount is
     * If time is already being altered by a previous function then this does nothing
     */
    public void SlowDown(float amount)
    {
        if (!IsNormalTime()) return;
        
        if (amount <= 0f)
        {
            amount = 0.01f;
        }
        _slowDown = amount;
    }
    
    /**
     * Stops time for given length (in seconds)
     * No changes to audio
     * If time is already being altered by a previous function then this does nothing
     */
    public void HitStop(float length)
    {
        if (!IsNormalTime()) return;
        
        Time.timeScale = 0f;
        StartCoroutine(TimeIsStopped(length, false));
    }

    /**
     * Stops time for given length (in seconds)
     * Stops audio for that duration
     * If time is already being altered by a previous function then this does nothing
     */
    public void TrueStop(float length)
    {
        if (!IsNormalTime()) return;

        Time.timeScale = 0f;
        foreach (var mixer in audioMixers)
        {
            mixer.SetFloat("allPitch", 0f);
        }
        StartCoroutine(TimeIsStopped(length, true));
    }

    /**
     * Waits for given length (in seconds real time) then calls ContinueTime
     */
    private IEnumerator TimeIsStopped(float length, bool trueStop)
    {
        yield return new WaitForSecondsRealtime(length);
        ContinueTime(length, trueStop);
        yield break;
    }
    
    /**
     * Resumes time scale
     * If true stop is true, it also restores audio pitch
     */
    private void ContinueTime(float length, bool trueStop)
    {
        Time.timeScale = _baseTimeScale;
        
        if (!trueStop) return;
        
        foreach (var mixer in audioMixers)
        {
            mixer.SetFloat("allPitch", _baseTimeScale);
        }
    }
    
    /**
     * Determines if time is normal
     */
    private bool IsNormalTime()
    {
        return Math.Abs(Time.timeScale - _baseTimeScale) < 0.01f;
    }
}
