using UnityEngine;

[CreateAssetMenu(fileName ="New CharacterData", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    public string Name;
    public float Speed;
    public GameObject Model;
    public GameObject Collider;
    public Sprite Portrait;
    public float Zoom = 1f;
}
