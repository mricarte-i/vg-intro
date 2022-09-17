using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool _keyboard = true;
    [SerializeField] private int _device = 0;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("im alive!");

    }

    // Update is called once per frame
    void Update()
    {
        if(_keyboard){
            Debug.Log("using keyboard!");
        }else if(_device != -1){
            Debug.Log("joystick number " + _device);
        }
    }
}
