using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BgmBox : MonoBehaviour
{
    [SerializeField] private Image _preview;
    [SerializeField] private TextMeshProUGUI _name, _stat;
    private BgmData _bgm;

    public BgmData GetBGMData(){
        return _bgm;
    }

    public void SetData(BgmData data){
        if(data == _bgm){
            return;
        }
        _bgm = data;
        _name.text = data.Name;
        _stat.text = "BGM: " + data.Bpm.ToString();
        _preview.sprite = data.Preview;
        Vector2 pixelSize = new Vector2(_preview.sprite.texture.width, _preview.sprite.texture.height);
        Vector2 pixelPivot = _preview.sprite.pivot;
        Vector2 uiPivot = new Vector2(pixelPivot.x / pixelSize.x, pixelPivot.y / pixelSize.y);

        _preview.GetComponent<RectTransform>().pivot = uiPivot;
        _preview.GetComponent<RectTransform>().sizeDelta *= data.Zoom;
    }
}
