using UnityEngine;
using UnityEngine.InputSystem.Users;

[CreateAssetMenu(fileName ="PlayableCharacter", menuName = "Playable Character (yes)")]
public class PlayableCharacter : ScriptableObject
{
    public string Name;
    public float Speed;
    public GameObject Model;
    public GameObject Collider;
    public InputUser inputUser;
    public void SetInputUser(InputUser iu) => inputUser = iu;
    public GeneratedPlayerControls controls = null;
    public void SetControls(GeneratedPlayerControls c) => controls = c;
}
