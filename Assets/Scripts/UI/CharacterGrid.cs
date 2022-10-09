using System.Collections.Generic;
using UnityEngine;

public class CharacterGrid : MonoBehaviour
{
    [SerializeField] private List<CharacterData> _characters = new List<CharacterData>();
    [SerializeField] private GameObject _charaCellPrefab;
    // Start is called before the first frame update
    void Start()
    {
        foreach(CharacterData chara in _characters){
            SpawnCharacterCell(chara);
        }
    }

    private void SpawnCharacterCell(CharacterData character){
        GameObject cell = Instantiate(_charaCellPrefab, transform);

        CharacterCell charaCell = cell.GetComponent<CharacterCell>();
        charaCell.SetData(character.Portrait, character.Name);
    }

    public CharacterData GetCharacter(int index){
        return _characters[index];
    }

    public void ConfirmCharacter(int playerId, CharacterData chara){
        //TODO: code  code  code
        AppManager.Instance.SetPlayerCharacter(playerId, chara);
        Debug.Log("confirmed " + chara.Name + " for player " + playerId + 1);
        return;
    }

    public void ShowCharacterInSlot(int playerId, CharacterData chara){
        //TODO: code  code  code
        Debug.Log("now showing " + chara.Name + " for player " + playerId + 1);
        return;
    }
}
