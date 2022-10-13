using UnityEngine;

public class ReloadFightButton : MonoBehaviour
{
    public void ReloadFight(){
        AppManager.Instance.StartFight();
    }
}
