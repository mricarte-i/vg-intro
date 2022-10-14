using UnityEngine;

[CreateAssetMenu(fileName ="New ProyectileData", menuName = "Proyectile Data")]
public class ProyectileData : ScriptableObject
{
    public string Name;
    public GameObject Proyectile;
    public float ProyectileCount;
    public float ProyectileDelay;
    public float Speed;
    public float Dropoff;
    public float Lifetime;
    public float Damage;
}