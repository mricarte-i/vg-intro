using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterBox : MonoBehaviour
{

    [SerializeField] private Image _portrait;
    [SerializeField] private TextMeshProUGUI _name;
    private CharacterData _chara;

    public CharacterData GetCharacterData(){
        return _chara;
    }


    public void SetData(CharacterData chara){
        _chara = chara;
        _name.text = chara.Name;
        _portrait.sprite = chara.Portrait;

        Vector2 pixelSize = new Vector2(_portrait.sprite.texture.width, _portrait.sprite.texture.height);
        Vector2 pixelPivot = _portrait.sprite.pivot;
        Vector2 uiPivot = new Vector2(pixelPivot.x / pixelSize.x, pixelPivot.y / pixelSize.y);

        _portrait.GetComponent<RectTransform>().pivot = uiPivot;
        _portrait.GetComponent<RectTransform>().sizeDelta *= chara.Zoom;
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
