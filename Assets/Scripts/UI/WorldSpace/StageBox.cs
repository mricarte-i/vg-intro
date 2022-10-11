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
        _preview.sprite = data.Preview;
        _name.text = data.Name;
        _stage = data;
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
