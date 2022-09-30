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

        Time.timeScale = _paused ? 0f : 1f;

        _settingsMenu.SetActive(_paused);
    }

    void Start(){
        _settingsMenu.SetActive(false);
    }
}
