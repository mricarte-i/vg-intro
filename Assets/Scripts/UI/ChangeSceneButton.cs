using UnityEngine;
using UnityEngine.UI;

public class ChangeSceneButton : MonoBehaviour
{
    public void ChangeScene(string sceneName){
        LevelManager.Instance.LoadScene(sceneName);
    }

}
