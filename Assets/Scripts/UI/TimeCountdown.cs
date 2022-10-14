using UnityEngine;
using TMPro;

public class TimeCountdown : MonoBehaviour
{
    [SerializeField] private bool _paused = false;
    public void Pause() => _paused = true;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int _durationInSeconds = 90;
    private float _remainingDuration;

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
    }

    private void OnEndTimer(){
        EventsManager.Instance.EventTimeout();
    }

    // Update is called once per frame
    void Update()
    {
        if(_paused) return;

        _remainingDuration -= Time.deltaTime;
        _text.text = $"{(int) Mathf.Max(_remainingDuration, 0)}";

        if(_remainingDuration <= 0){
            Pause();
            OnEndTimer();
        }

    }
}
