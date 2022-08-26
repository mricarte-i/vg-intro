using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool keyboard = true;
    [SerializeField] private int device = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("im alive!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
