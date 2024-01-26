using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instanciator : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    
    public void CreateInstance()
    {
        Instantiate(_prefab, transform.position, transform.rotation);
    }

}
