using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CharacterPortrait : MonoBehaviour
{
    private Image _image;
    [SerializeField] private CharacterData _chara;
    private CharacterData _knownchara;
    [SerializeField] private float Zoom = 1f;

    public void ShowCharacterPortrait(CharacterData chara){
        if(chara == _knownchara){
            //return;
        }
        _knownchara = chara;

        _image.sprite = chara.Portrait;

        Vector2 pixelSize = new Vector2(_image.sprite.texture.width, _image.sprite.texture.height);
        Vector2 pixelPivot = _image.sprite.pivot;
        Vector2 uiPivot = new Vector2(pixelPivot.x / pixelSize.x, pixelPivot.y / pixelSize.y);

        _image.GetComponent<RectTransform>().pivot = uiPivot;
        _image.GetComponent<RectTransform>().sizeDelta = Zoom * pixelSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowCharacterPortrait(_chara);
    }
}
