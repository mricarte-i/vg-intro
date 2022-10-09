using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCell : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private TextMeshProUGUI _name;

    public void SetData(Sprite img, string txt){
        _portrait.sprite = img;
        _name.text = txt;
    }
}
