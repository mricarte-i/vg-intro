using UnityEngine;
using UnityEngine.UI;

public class BeatBar : MonoBehaviour
{
    [SerializeField] private bool _paused = false;

    [SerializeField] private Color _greatColor, _goodColor, _badColor;
    [SerializeField] private Image _beatBar;

    // Start is called before the first frame update
    void Start()
    {
        RhythmController.Instance.OnPause += OnRhythmPause;
    }

    private void OnRhythmPause(bool isPaused){
        _paused = isPaused;
    }

    // Update is called once per frame
    void Update()
    {
        _beatBar.fillAmount = RhythmController.Instance.getBeatProgress();
        switch(RhythmController.Instance.GetBeat()){
            case RhythmState.Bad:
                _beatBar.color = _badColor;
                break;
            case RhythmState.Good:
                _beatBar.color = _goodColor;
                break;
            case RhythmState.Great:
                _beatBar.color = _greatColor;
                break;
        }
    }
}
