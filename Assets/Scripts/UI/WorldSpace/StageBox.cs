using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageBox : MonoBehaviour
{
    [SerializeField] private Image _preview;
    [SerializeField] private TextMeshProUGUI _name;
    private StageData _stage;

    public StageData GetStageData(){
        return _stage;
    }

    public void SetData(StageData data){
        if(data == _stage){
            return;
        }
        _stage = data;
        _name.text = data.Name;
        _preview.sprite = data.Preview;
        Vector2 pixelSize = new Vector2(_preview.sprite.texture.width, _preview.sprite.texture.height);
        Vector2 pixelPivot = _preview.sprite.pivot;
        Vector2 uiPivot = new Vector2(pixelPivot.x / pixelSize.x, pixelPivot.y / pixelSize.y);

        _preview.GetComponent<RectTransform>().pivot = uiPivot;
        _preview.GetComponent<RectTransform>().sizeDelta *= data.Zoom;
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
