using UnityEngine;

public class Pause : MonoBehaviour
{
    public void TogglePause(){
        AppManager.Instance.TogglePause();
    }
}
