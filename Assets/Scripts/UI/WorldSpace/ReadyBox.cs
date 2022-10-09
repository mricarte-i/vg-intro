using UnityEngine;

public class ReadyBox : MonoBehaviour
{
    [SerializeField] private SelectMenuConductor _menu;

    public void CalledReady(int playerId){
        _menu.CalledReady(playerId);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
