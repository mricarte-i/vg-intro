using UnityEngine;

[CreateAssetMenu(fileName ="New BgmData", menuName = "Bgm Data")]
public class BgmData : ScriptableObject
{
    public string Name;
    public AudioClip Bgm;
    public int Bpm;
    public float FirstBeatOffset;
}