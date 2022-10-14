using UnityEngine;

[CreateAssetMenu(fileName ="New BgmData", menuName = "Bgm Data")]
public class BgmData : ScriptableObject
{
    public string Name;
    public AudioClip BGM;
    public int Bpm;
    public float FirstBeatOffset;
    public Sprite Preview;
    public float Zoom = 1;
}