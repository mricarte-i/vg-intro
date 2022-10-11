using UnityEngine;
using System;
using System.Collections.Generic;


public enum MenuState {
    PAIR_CHAR_SELECT,
    STAGE_MODE_SELECT,
    MUSIC_BPM_SELECT,
};

public enum MoveState {
    PAIR_CHAR_SELECT,
    STAGE_MODE_SELECT,
    MUSIC_BPM_SELECT,
    DONE,
};


public class SelectMenuConductor : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera _vCam;
    [SerializeField] private MenuState _state = MenuState.PAIR_CHAR_SELECT;
    private bool[] _playersReady = {false, false};

    private Cinemachine.CinemachineTrackedDolly _dolly;

    [SerializeField] private float _pathPosTarget = 0f;
    [SerializeField] private List<GameObject> _dividers = new List<GameObject>();

    private List<GameObject> _cursors = new List<GameObject>();
    [SerializeField] private MoveState _cursor_move_state = MoveState.DONE;
    [SerializeField] private Transform _pair_place_target, _stage_place_target, _music_place_target;

    void Start(){
        _dolly = _vCam.GetCinemachineComponent<Cinemachine.CinemachineTrackedDolly>();
        _dolly.m_PathPosition = 0f;
    }

    void LateUpdate(){
        if(_dolly.m_PathPosition != _pathPosTarget){
            _dolly.m_PathPosition = Mathf.Lerp(_dolly.m_PathPosition, _pathPosTarget, 0.1f);
        }

        if(_cursor_move_state != MoveState.DONE){
            Vector3 _target = Vector3.zero;
            switch(_cursor_move_state){
                case MoveState.PAIR_CHAR_SELECT:
                    _target  = _pair_place_target.position;
                    break;
                case MoveState.STAGE_MODE_SELECT:
                    _target  = _stage_place_target.position;
                    break;
                case MoveState.MUSIC_BPM_SELECT:
                    _target  = _music_place_target.position;
                    break;
            }
            foreach(GameObject cursor in _cursors){
                cursor.GetComponent<CursorInputHandler>().SetCinematicMode(true);
                _target.x = cursor.transform.position.x;
                cursor.transform.position = Vector3.Lerp(cursor.transform.position, _target, 0.1f);
            }

            if(Mathf.Approximately(_cursors[0].transform.position.z, _target.z)){
                foreach(GameObject cursor in _cursors){
                    cursor.GetComponent<CursorInputHandler>().SetCinematicMode(false);
                }
                _cursor_move_state = MoveState.DONE;
                ToggleDividers();
            }
        }
    }

    public void CalledReady(int playerId){
        if(playerId > 1){
            throw new Exception("CalledReady received a weird playerId");
        }
        _playersReady[playerId] = true;
        Debug.Log(_playersReady[0] + " " + _playersReady[1]);
        if(_playersReady[0] && _playersReady[1] && _cursor_move_state == MoveState.DONE){
            NextMenuState();
            _playersReady[0] = false;
            _playersReady[1] = false;
        }
    }

    private bool ArePlayersReady(){
        return _playersReady[0] && _playersReady[1];
    }

    public void AddCursor(GameObject go){
        _cursors.Add(go);
    }

    private void ToggleDividers(){
        foreach(var wall in _dividers){
            wall.SetActive(!wall.activeInHierarchy);
        }
    }

    private void NextMenuState(){
        Debug.Log(_state.ToString());

        switch(_state){
            case MenuState.PAIR_CHAR_SELECT:
                _pathPosTarget = 0;
                if(PlayersHaveCharacters() && _cursor_move_state == MoveState.DONE && ArePlayersReady()){
                    _state = MenuState.STAGE_MODE_SELECT;
                    ToggleDividers();
                    _pathPosTarget = 1;
                    _cursor_move_state = MoveState.STAGE_MODE_SELECT;

                    Debug.Log("advance! to 1");
                }
                break;
            case MenuState.STAGE_MODE_SELECT:
                _pathPosTarget = 1;
                if(_cursor_move_state == MoveState.DONE && ArePlayersReady()){
                    ToggleDividers();
                    _state = MenuState.MUSIC_BPM_SELECT;
                    Debug.Log("advance! to 2");
                    _pathPosTarget = 2;
                    _cursor_move_state = MoveState.MUSIC_BPM_SELECT;
                }
                break;
            case MenuState.MUSIC_BPM_SELECT:
                _pathPosTarget = 2;
                if(_cursor_move_state == MoveState.DONE && ArePlayersReady()){
                    ToggleDividers();
                    _pathPosTarget = 0;
                    _cursor_move_state = MoveState.PAIR_CHAR_SELECT;
                    Debug.Log("advance! to 0");
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
