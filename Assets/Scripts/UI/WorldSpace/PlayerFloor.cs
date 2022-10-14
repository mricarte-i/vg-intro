using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerFloor : MonoBehaviour
{
    [SerializeField] private GameObject _notPairedMsg;
    [SerializeField] private GameObject _characterShow;
    [SerializeField] private Image _charaPortrait;
    [SerializeField] private TextMeshProUGUI _charaName;
    private CharacterData _chara;
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
        if(_chara == chara){
            return;
        }
        _chara = chara;
        _charaName.text = chara.Name;
        _charaPortrait.sprite = chara.Portrait;

        Vector2 pixelSize = new Vector2(_charaPortrait.sprite.texture.width, _charaPortrait.sprite.texture.height);
        Vector2 pixelPivot = _charaPortrait.sprite.pivot;
        Vector2 uiPivot = new Vector2(pixelPivot.x / pixelSize.x, pixelPivot.y / pixelSize.y);

        _charaPortrait.GetComponent<RectTransform>().pivot = uiPivot;
        //_charaPortrait.GetComponent<RectTransform>().sizeDelta *= chara.Zoom;
    }

    public void SetPaired(CursorInputHandler cih){
        _notPairedMsg.SetActive(false);
        _characterShow.SetActive(true);
        return;
    }
}
