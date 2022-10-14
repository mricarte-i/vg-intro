using UnityEngine;

[CreateAssetMenu(fileName ="New RhythmHpModifierData", menuName = "Rhythm Hp modifier")]
public class RhythmHpModifierData : ScriptableObject
{
    public string Name;
    public float GreatHpModifier;
    public float GoodHpModifier;
    public float BadHpModifier;
}
