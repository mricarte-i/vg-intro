using System.Collections;
using UnityEngine;
using TMPro;

public class TimeCountdown : MonoBehaviour
{
    [SerializeField] private bool _paused = false;
    public void Pause() => _paused = true;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int _durationInSeconds = 90;
    private int _remainingDuration;

    public void Reset(){
        _paused = false;
        Begin(_durationInSeconds);
    }

    // Start is called before the first frame update
    void Start()
    {
        Begin(_durationInSeconds);
    }

    private void Begin(int duration){
        _remainingDuration = duration;
        base.StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer(){
        while(_remainingDuration >= 0){
            if(!_paused){
                _text.text = $"{_remainingDuration}";
                _remainingDuration--;
                yield return new WaitForSecondsRealtime(1f);
            }else{
                yield return null;
            }
        }
        OnEndTimer();
    }

    private void OnEndTimer(){
        EventsManager.Instance.EventTimeout();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
