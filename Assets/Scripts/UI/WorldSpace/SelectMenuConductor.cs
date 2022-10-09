using UnityEngine;
using System;


public enum MenuState {
    PAIR_CHAR_SELECT,
    STAGE_MODE_SELECT,
    MUSIC_BPM_SELECT,
};

public class SelectMenuConductor : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera _vCam;
    [SerializeField] private MenuState _state = MenuState.PAIR_CHAR_SELECT;
    private bool[] _playersReady = {false, false};
    private Cinemachine.CinemachineTrackedDolly _dolly;
    [SerializeField] private float _pathPosTarget = 0f;

    void Start(){
        _dolly = _vCam.GetCinemachineComponent<Cinemachine.CinemachineTrackedDolly>();
        _dolly.m_PathPosition = 0f;
    }

    void LateUpdate(){
        if(_dolly.m_PathPosition != _pathPosTarget){
            _dolly.m_PathPosition = Mathf.Lerp(_dolly.m_PathPosition, _pathPosTarget, 0.5f);
        }
    }

    public void CalledReady(int playerId){
        if(playerId > 1){
            throw new Exception("CalledReady received a weird playerId");
        }
        _playersReady[playerId] = true;
        Debug.Log(_playersReady[0] + " " + _playersReady[1]);
        if(_playersReady[0] && _playersReady[1]){
            NextMenuState();
            _playersReady[0] = false;
            _playersReady[1] = false;
        }
    }

    private void NextMenuState(){
        switch(_state){
            case MenuState.PAIR_CHAR_SELECT:
                _pathPosTarget = 0;
                if(PlayersHaveCharacters()){
                    _state = MenuState.STAGE_MODE_SELECT;
                    Debug.Log("advance!");
                    _pathPosTarget = 1;
                }
                break;
            case MenuState.STAGE_MODE_SELECT:
                _pathPosTarget = 1;
                if(PlayersHaveCharacters()){
                    _state = MenuState.MUSIC_BPM_SELECT;
                    Debug.Log("advance!");
                    _pathPosTarget = 2;
                }
                break;
            case MenuState.MUSIC_BPM_SELECT:
                _pathPosTarget = 2;
                if(PlayersHaveCharacters()){
                    _pathPosTarget = 0;
                    Debug.Log("advance!");
                    LevelManager.Instance.LoadScene(AppManager.Instance.GetStageName());
                }
                break;
            default:
                throw new Exception("what");
        }
    }

    private bool PlayersHaveCharacters(){
        var p1 = AppManager.Instance.GetInputUser(0);
        var resp1 = !AppManager.Instance.IsNotPlayerControlled(0) && (p1.characterData != null);

        var p2 = AppManager.Instance.GetInputUser(1);
        var resp2 = !AppManager.Instance.IsNotPlayerControlled(1) && (p2.characterData != null);
        return resp1 && resp2;
    }

  }
