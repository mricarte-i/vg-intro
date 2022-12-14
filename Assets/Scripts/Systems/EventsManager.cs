using System;
using System.Collections;
using UnityEngine;

public enum PlayerId {
    P1,
    P2,
}
public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance;

    [SerializeField] private int _stocksWinCondition = 2;
    [SerializeField] private float _awaitInCaseOfDraw = 0.3f;
    [Space]
    [SerializeField] private bool _player1Dead, _player2Dead, _draw;
    [SerializeField] private int _player1Stocks, _player2Stocks;
    [SerializeField] private int _player1WinStrike, _player2WinStrike;
    [Space]
    [SerializeField] private int _rounds = 0;
    [SerializeField] private int _maxRounds = 3;
    public void SetMaxRounds(int newMax) => _maxRounds = newMax;

    public bool IsDraw => _draw;

    private FightResult _winner = FightResult.DRAW;

    void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public event Action<bool> OnPlayer1Death;
    public event Action<bool> OnPlayer2Death;
    public event Action<bool> OnFightDraw;

    public void EventPlayerDeath(PlayerId id){
        switch(id){
            case PlayerId.P1:
                if(_player1Dead == true) return;
                if(OnPlayer1Death != null) OnPlayer1Death(true);
                _player1Dead = true;
                break;
            case PlayerId.P2:
                if(_player2Dead == true) return;
                if(OnPlayer2Death != null) OnPlayer2Death(true);
                _player2Dead = true;
                break;
        }

        //TODO: if there's a time manager, maybe do a slow motion for dramatic effect...
        base.StartCoroutine(this.HoldInCaseOfDraw());
    }

    public void EventTimeout(){
        base.StartCoroutine(this.HoldInCaseOfDraw());
    }

    private IEnumerator HoldInCaseOfDraw(){
        yield return new WaitForSecondsRealtime(_awaitInCaseOfDraw);
        if((_player1Dead && _player2Dead) ||(!_player1Dead && !_player2Dead)){
            if(OnFightDraw != null) OnFightDraw(true);
            _draw = true;
        }
        this.RoundResult();
        yield break;
    }

    public event Action<bool> OnShowRoundResult;
    private void RoundResult(){
        OnShowRoundResult(true);
        if(!_player1Dead && _player2Dead){
            _player1Stocks++;
        }else if(_player1Dead && !_player2Dead){
            _player2Stocks++;
        }
    }

    public event Action<bool> OnGameOver;

    public void NextRound(){
        _player1Dead = false;
        _player2Dead = false;
        _draw = false;
        _rounds++;

        if(OnGameOver != null){
            if(_player1Stocks == _stocksWinCondition || _player1Stocks == _stocksWinCondition || _rounds == _maxRounds){

                if(_player1Stocks == _stocksWinCondition) _winner = FightResult.PLAYER1WINS;
                if(_player2Stocks == _stocksWinCondition) _winner = FightResult.PLAYER2WINS;

                _player1Stocks = 0;
                _player2Stocks = 0;
                _rounds = 0;

                AppManager.Instance.SetAppState(AppState.VICTORY);
                OnGameOver(true);
                return;
            }
        }


        if(OnPlayer1Death != null) OnPlayer1Death(false);
        if(OnPlayer2Death != null) OnPlayer2Death(false);
        if(OnFightDraw != null) OnFightDraw(false);
        LevelManager.Instance.ResetFight();
    }

    public FightResult GetFightResult(){
        if(_player1Dead && _player2Dead){
            return FightResult.DRAW;
        }else if(_player2Dead){
            return FightResult.PLAYER1WINS;
        }else{
            return FightResult.PLAYER2WINS;
        }
    }

    public PlayableCharacter GetWinner(){
        if(_winner == FightResult.DRAW){
            return null;
        }else if(_winner == FightResult.PLAYER1WINS){
            return AppManager.Instance.GetInputUser(0);
        }else{
            return AppManager.Instance.GetInputUser(1);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
