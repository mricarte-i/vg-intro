using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private string _player1Device, _player2Device;
    [SerializeField] private bool _paused = false;

    void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public void TogglePause(){
        _paused = !_paused;
        Debug.Log("TOGGLE PAUSE CALLED " + _paused + " " + Time.timeScale);

        if(_paused){
            Time.timeScale = 0;
            _settingsMenu.SetActive(true);
        }else{
            Time.timeScale = 1f;
            _settingsMenu.SetActive(false);
        }
    }

    void Start(){
        _settingsMenu.SetActive(false);
    }
}
