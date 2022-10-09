using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerFloor : MonoBehaviour
{
    [SerializeField] private GameObject _notPairedMsg;
    [SerializeField] private GameObject _characterShow;
    [SerializeField] private Image _charaPortrait;
    [SerializeField] private TextMeshProUGUI _charaName;
    // Start is called before the first frame update
    void Start()
    {
        _notPairedMsg.SetActive(true);
        _characterShow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowCharacterData(CharacterData chara){
        _charaPortrait.sprite = chara.Portrait;
        _charaName.text = chara.Name;
    }

    public void SetPaired(CursorInputHandler cih){
        _notPairedMsg.SetActive(false);
        _characterShow.SetActive(true);
        return;
    }
}
