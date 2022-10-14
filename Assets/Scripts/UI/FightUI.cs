using UnityEngine;
using System.Collections;
using TMPro;

public class FightUI : MonoBehaviour
{
    [SerializeField] private HealthBar _HPBarP1;
    [SerializeField] private HealthBar _HPBarP2;
    [SerializeField] private GameObject _roundEndText;
    [SerializeField] private float _holdResultsTime = 3f;
    [SerializeField] private TimeCountdown _timer;
    [SerializeField] private GameObject _time, _beat;

    public HealthBar GetPlayer1HPBar() => _HPBarP1;
    public HealthBar GetPlayer2HPBar() => _HPBarP2;

    public void Reset(){
        Start();
        _timer.Reset();
    }

    // Start is called before the first frame update
    void Start()
    {
        var isNormal = AppManager.Instance.GetGameMode() == GameMode.NORMAL;
        _time.SetActive(isNormal);
        _beat.SetActive(!isNormal);

        _roundEndText.SetActive(false);
        EventsManager.Instance.OnShowRoundResult += ShowRoundResults;
    }

    private void ShowRoundResults(bool show){
        if(show){
            _timer.Pause();
            _roundEndText.SetActive(true);
            _roundEndText.GetComponent<TextMeshProUGUI>().text = EventsManager.Instance.IsDraw ? "DRAW" : "K.O.";
            base.StartCoroutine(this.HoldResults());

        }
    }
    private IEnumerator HoldResults(){
        yield return new WaitForSecondsRealtime(_holdResultsTime);
        _roundEndText.SetActive(false);
        EventsManager.Instance.NextRound();
        yield break;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
