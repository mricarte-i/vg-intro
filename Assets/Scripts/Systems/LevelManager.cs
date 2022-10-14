using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;



public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Image _progressBar;
    private float _target;
    [Space]
    [SerializeField] private GameObject _normalSetup;
    [SerializeField] private GameObject _rhythmSetup;
    [Space]
    [SerializeField] private string _endFightSceneName = "EndFightScene";
    private string _currentScene;
    private NormalFightSetup _normalFight;
    private RhythmFightSetup _rhythmFight;


    void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    void Start() {
        EventsManager.Instance.OnGameOver += OnFightOver;
    }

    private void OnFightOver(bool isFightOver) {
        Invoke("LoadEndgameScene", 3f);
    }

    private void LoadEndgameScene() => LoadScene(_endFightSceneName);

    private void AddSetup(AsyncOperation op) {
        if(AppManager.Instance.GetAppState() != AppState.FIGHT){
            return;
        }
        //we could get the current scene by asking for the sceneName used and set that as the active scene...
        switch(AppManager.Instance.GetGameMode()){
            case GameMode.NORMAL:
                var go = Instantiate(_normalSetup);
                _normalFight = go.GetComponent<NormalFightSetup>();
                break;
            case GameMode.RHYTHM:
                var go1 = Instantiate(_rhythmSetup);
                _rhythmFight = go1.GetComponent<RhythmFightSetup>();
                break;
            default:
                Debug.Log("what");
                break;
        }
    }

    public void LoadScene(string sceneName) {
        base.StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string sceneName){
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        op.allowSceneActivation = false;
        op.completed += AddSetup;

        float progress = 0;
        while(!op.isDone){
            progress = op.progress;
            _progressBar.fillAmount = progress;

            if(op.progress >= 0.9f){
                op.allowSceneActivation = true;
            }
            yield return null;
        }

        var oldScene = SceneManager.GetActiveScene();

        var newScene = SceneManager.GetSceneByName(sceneName);
        if(!newScene.IsValid()) yield break;
        SceneManager.SetActiveScene(newScene);
        SceneManager.UnloadSceneAsync(oldScene);
    }

    void Update(){
        var newX = Mathf.MoveTowards(_progressBar.rectTransform.localScale.x, _target * 5f, 3 * Time.deltaTime);
        _progressBar.rectTransform.localScale.Set(newX, 1, 1);
    }

    public void ResetFight(){
        if(AppManager.Instance.GetAppState() == AppState.FIGHT){
            switch(AppManager.Instance.GetGameMode()){
                case GameMode.NORMAL:
                    _normalFight.ResetFight();
                    break;
                case GameMode.RHYTHM:
                    LoadEndgameScene();
                    break;
                default:
                    Debug.Log("bruh");
                    break;
            }
        }
    }
}
