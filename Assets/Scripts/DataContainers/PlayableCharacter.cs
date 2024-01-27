using UnityEngine;
using UnityEngine.InputSystem.Users;

[CreateAssetMenu(fileName ="New PlayableCharacter", menuName = "Playable Character")]
public class PlayableCharacter : ScriptableObject
{
    public CharacterData characterData = null;
    public InputUser inputUser;
    public void SetInputUser(InputUser iu) => inputUser = iu;
    public GeneratedPlayerControls controls = null;
    public void SetControls(GeneratedPlayerControls c) => controls = c;
    public PlayerId playerId = PlayerId.P1;
    public GameObject _cursor { get; set; }
}
