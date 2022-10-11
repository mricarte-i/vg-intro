using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class FloatingTitle : MonoBehaviour
{
    [SerializeField] private string Text;
    [SerializeField] private TextMeshProUGUI _text_solid, _text_partly, _text_misty;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _text_solid.text = Text;
        _text_partly.text = Text;
        _text_misty.text = Text;
    }
}
