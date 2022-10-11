using UnityEngine;
using UnityEngine.UI;
using TMPro;
[ExecuteInEditMode]
public class ButtonBox : MonoBehaviour
{
    [SerializeField] private string Text;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _bg_solid, _bg_partly, _bg_misty;
    [SerializeField] private float solid = 1f;
    [SerializeField] private float partly = 0.5f;
    [SerializeField] private float misty = 0.1f;
    [SerializeField] private Color Color;

    [SerializeField] private string _data;

    public string GetData(){
        return _data;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _text.text = Text;
        var solidColor = Color;
        solidColor.a = solid;

        _bg_solid.color = solidColor;

        var partlyColor = Color;
        partlyColor.a = partly;

        _bg_partly.color = partlyColor;

        var mistyColor = Color;
        mistyColor.a = misty;

        _bg_misty.color = mistyColor;
    }
}
