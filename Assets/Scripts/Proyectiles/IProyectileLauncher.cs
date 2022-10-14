using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProyectileLauncher
{
    GameObject Proyectile { get; }
    float ProyectileCount { get; }
    float ProyectileDelay { get; }
    void Attack();
}