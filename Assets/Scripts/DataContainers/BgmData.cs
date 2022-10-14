using UnityEngine;

[CreateAssetMenu(fileName ="New BgmData", menuName = "Bgm Data")]
public class BgmData : ScriptableObject
{
    public string Name;
    public AudioSource Bgm;
    public int Bpm;
    public float FirstBeatOffset;
    public float BeatsPerLoop;
}