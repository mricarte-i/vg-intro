using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProyectile
{
    float Speed { get; set; }
    float Dropoff { get; set; }
    float Lifetime { get; set; }
    float Damage { get; set; }
    void Travel();
}