using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Image _progressBar;
    private float _target;
    [SerializeField] private GameObject _normalSetup;
    [SerializeField] private GameObject _rhythmSetup;


    void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    private void AddSetup(AsyncOperation op) {
        if(AppManager.Instance.GetAppState() != AppState.FIGHT){
            return;
        }
        //we could get the current scene by asking for the sceneName used and set that as the active scene...
        switch(AppManager.Instance.GetGameMode()){
            case GameMode.NORMAL:
                Instantiate(_normalSetup);
                break;
            case GameMode.RHYTHM:
                Instantiate(_rhythmSetup);
                break;
            default:
                Debug.Log("what");
                break;
        }
    }

    //if you remove the delay you can get rid of the async too...
    public async void LoadScene(string sceneName) {
        _target = 0f;
        _progressBar.rectTransform.localScale.Set(0, 1, 1);

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        scene.completed += AddSetup;

        _loadingScreen.SetActive(true);

        do{
            //TODO: remove this...
            await Task.Delay(100);
            if(_target != scene.progress){
                Debug.Log(scene.progress);
            }
            _target = scene.progress;
        }while(scene.progress < 0.9f);

        scene.allowSceneActivation = true;

        _loadingScreen.SetActive(false);
    }

    void Update(){
        var newX = Mathf.MoveTowards(_progressBar.rectTransform.localScale.x, _target * 5f, 3 * Time.deltaTime);
        _progressBar.rectTransform.localScale.Set(newX, 1, 1);
    }
}
