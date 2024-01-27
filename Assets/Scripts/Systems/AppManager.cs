using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System;

public enum GameMode {
    NORMAL,
    RHYTHM,
};
public enum AppState {
    START,
    CSM,
    FIGHT,
    VICTORY,
    RHYTHMRESULTS,
};

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;

    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _eventsSystem;
    [Space]
    [SerializeField] private PlayableCharacter _player1Device = null, _player2Device = null;
    [Space]
    [SerializeField] private bool _paused = false;
    [SerializeField] private AppState _appState = AppState.START;
    [SerializeField] private GameMode _selectedGameMode = GameMode.NORMAL;
    [SerializeField] private string _selectedStage = "SceneStageCerro";
    [SerializeField] private BgmData _selectedBGMData;

    public bool IsAIControlled() => _aiControlled;
    
    private bool _aiControlled = false;

    public WSCharacterSelectMenu _wsCharacterSelectMenu { get; set; }

    public void SetWSCharacterSelectMenu(WSCharacterSelectMenu ws) => _wsCharacterSelectMenu = ws;

    public BgmData GetBGMData() => _selectedBGMData;
    public void SetBGMData(BgmData bgmData) => _selectedBGMData = bgmData;

    public string GetStageName(){
        return _selectedStage;
    }

    public void SetStage(StageData stageData){
        _selectedStage = stageData.SceneName;
    }

    public void SetGameMode(GameMode gm){
        _selectedGameMode = gm;
    }

    public void SetAppState(AppState appState){
        _appState = appState;
    }

    public AppState GetAppState(){
        return _appState;
    }

    void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }

    }

    public event Action<bool> OnPause;
    public void TogglePause(){
        _paused = !_paused;
        Debug.Log("TOGGLE PAUSE CALLED " + _paused + " " + Time.timeScale);

        if(_paused){
            Time.timeScale = 0;
            if(OnPause != null) OnPause(true);
            _settingsMenu.SetActive(true);

            //don't active more than one EventSystem at once!
            if(GameObject.Find("EventSystem") == null){
                _eventsSystem.SetActive(true);
            }
        }else{
            Time.timeScale = 1f;
            if(OnPause != null) OnPause(false);
            _settingsMenu.SetActive(false);
            _eventsSystem.SetActive(false);
        }
    }

    public PlayableCharacter GetInputUser(int playerId){
        if(playerId == 0){
            return _player1Device;
        }else if(playerId == 1){
            return _player2Device;
        }else{
            throw new Exception("AppManager's GetInputUser being mishandled!");
        }
    }

    public bool IsNotPlayerControlled(int playerId){
        if(playerId == 0){
            return _player1Device.controls == null;
        }else if(playerId == 1){
            return _player2Device.controls == null;
        }else{
            throw new Exception("AppManager's IsPlayerControlled being mishandled!");
        }
    }

    public void SetPlayerControl(int playerId, InputUser user, GeneratedPlayerControls controls, InputControl control){
        if(playerId == 0){
            _player1Device.SetInputUser(user);
            _player1Device.SetControls(controls);
            Debug.Log("Paired " + control.device.displayName + " to  Player 1! " + _player1Device);
        }else if(playerId == 1){
            _player2Device.SetInputUser(user);
            _player2Device.SetControls(controls);
            Debug.Log("Paired " + control.device.displayName + " to  Player 2! " + _player2Device);
        }else{
            throw new Exception("AppManager's SetPlayerControl being mishandled!");
        }
    }
    
    public void SetAIPlayerControl(){
        _player2Device.SetInputUser(_player1Device.inputUser);
        _player2Device.SetControls(_player1Device.controls);
        _player1Device._cursor.SetActive(false);
        _aiControlled = true;
    }

    public void ReturnControl()
    {
        _player1Device._cursor.SetActive(true);
        _player2Device._cursor.SetActive(false);
    }

    public void SetPlayerCharacter(int playerId, CharacterData character){
        if(playerId == 0){
            _player1Device.characterData = character;
        }else if(playerId == 1){
            _player2Device.characterData = character;
        }else{
            throw new Exception("AppManager's SetPlayerCharacter being mishandled!");
        }
    }

    public GameMode GetGameMode() {
        return _selectedGameMode;
    }

    void Start(){
        _settingsMenu.SetActive(false);
        _eventsSystem.SetActive(false);
    }

    public void StartFight(){
        SetAppState(AppState.FIGHT);
        LevelManager.Instance.LoadScene(_selectedStage);
    }
}
