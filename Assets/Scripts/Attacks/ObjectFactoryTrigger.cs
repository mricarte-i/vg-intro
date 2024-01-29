using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactoryTrigger : MonoBehaviour
{
    [SerializeField] private List<ObjectFactory> _instanciators;

    public void CreateInstances()
    {
        foreach (var instanciator in _instanciators)
        {
            instanciator.CreateInstance();
        }
    }
}
