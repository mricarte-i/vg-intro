using UnityEngine;

[CreateAssetMenu(fileName ="New StageData", menuName = "Stage Data")]
public class StageData : ScriptableObject
{
    public string Name;
    public string SceneName;
    public Sprite Preview;
    public float Zoom = 1;
}
