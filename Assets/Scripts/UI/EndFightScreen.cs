using UnityEngine;
using TMPro;

public enum FightResult {
    PLAYER1WINS,
    PLAYER2WINS,
    DRAW,
}
public class EndFightScreen : MonoBehaviour
{
    [SerializeField] private CharacterPortrait _portrait;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private FightResult _fightResult = FightResult.PLAYER1WINS;
    [SerializeField] private CharacterData _draw;
    [SerializeField] private PlayableCharacter _winner;

    public void SetFightResult(FightResult fr){
        _fightResult = fr;
    }

    public void SetWinner(PlayableCharacter pc){
        _winner = pc;
    }

    // Start is called before the first frame update
    void Start()
    {
        switch(_fightResult){
            case FightResult.DRAW:
                ShowDRAWScreen();
                break;
            default:
                ShowWinnerScreen();
                break;
        }
    }

    private void ShowDRAWScreen(){
        _portrait.ShowCharacterPortrait(_draw);
        _text.text = "DRAW";
        //play "booh" music from losing in tf2
    }

    private void ShowWinnerScreen(){
        _portrait.ShowCharacterPortrait(_winner.characterData);
        _text.text = "Player " + _winner.playerId + " WINS";
        //play tf2 victory music
    }

    // Update is called once per frame
    void Update()
    {

    }
}
