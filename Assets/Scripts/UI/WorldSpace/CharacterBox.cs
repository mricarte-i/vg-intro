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
        _portrait.sprite = chara.Portrait;
        _name.text = chara.Name;
        _chara = chara;
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
