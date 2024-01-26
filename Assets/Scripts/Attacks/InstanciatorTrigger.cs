using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciatorTrigger : MonoBehaviour
{
    [SerializeField] private List<Instanciator> _instanciators;

    public void CreateInstances()
    {
        foreach (var instanciator in _instanciators)
        {
            instanciator.CreateInstance();
        }
    }
}
