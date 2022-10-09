using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;
using System;
using System.Collections.Generic;

public class WSCharacterSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject _playerCursorPrefab;
    [SerializeField] private List<Transform> _cursorSpawnPositions = new List<Transform>();
    [Space]
    [SerializeField] private GameObject _charaBoxPrefab;
    [SerializeField] private List<Transform> _charaBoxPositions = new List<Transform>();
    [SerializeField] private List<CharacterData> _characters = new List<CharacterData>();
    [Space]
    [SerializeField] private List<GameObject> _playerFloors = new List<GameObject>();

    private bool _listening = false;

    // Start is called before the first frame update
    void Start()
    {
        //if players come back from a fight, they are already paired!
        if(!AppManager.Instance.IsNotPlayerControlled(0)){
            InitializePlayerCursor(0);
        }
        if(!AppManager.Instance.IsNotPlayerControlled(1)){
            InitializePlayerCursor(1);
        }

        if(AppManager.Instance.IsNotPlayerControlled(0) && AppManager.Instance.IsNotPlayerControlled(1)){
            Debug.Log("no players paired, start from zero!!!");
            //please just listen to me, man
            InputUser.listenForUnpairedDeviceActivity++;
            InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
            _listening = true;
        }

        InitializeCharacterGrid();

    }

    private void OnDestroy(){
        if(_listening){
            //stop listening
            InputUser.onUnpairedDeviceUsed -= OnUnpairedDeviceUsed;
        }
    }

    private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr eventPtr){
        //don't pay attention to the mouse or inputs things that aren't a button press!
        if(control.ToString().Contains("Mouse") || !(control is ButtonControl)){
            return;
        }

        Debug.Log("Unpaired device detected! " + control.device.displayName);

        var device = control.device;
        var controlScheme = default(String);
        if(device is Keyboard){
            controlScheme = "Keyboard";
        }else if(device is Gamepad){
            controlScheme = "Joystick";
        }else{
            Debug.Log("what are you? " + device);
            return; //who are you??
        }

        var user = InputUser.PerformPairingWithDevice(device);
        GeneratedPlayerControls usersControls = new GeneratedPlayerControls();
        usersControls.Enable();
        user.AssociateActionsWithUser(usersControls);
        user.ActivateControlScheme(controlScheme);

        if(AppManager.Instance.IsNotPlayerControlled(0)){
            AppManager.Instance.SetPlayerControl(0, user, usersControls, control);
            InitializePlayerCursor(0);
        }else if(AppManager.Instance.IsNotPlayerControlled(1)){
            AppManager.Instance.SetPlayerControl(1, user, usersControls, control);
            InitializePlayerCursor(1);
        }else{
            Debug.Log("neither are null!!!");
        }
    }

    private void InitializePlayerCursor(int playerId){
        var player = AppManager.Instance.GetInputUser(playerId);
        if(player == null){
            throw new System.Exception("how dare you");
        }

        //instatiation order matters!
        GameObject go = Instantiate(_playerCursorPrefab);
        go.transform.position = _cursorSpawnPositions[playerId].position;

        CursorInputHandler cih = go.GetComponent<CursorInputHandler>();
        cih.SetInputUser(player.inputUser);
        cih.BindControls(player.controls);
        CursorMaterialHandler cmh = go.GetComponent<CursorMaterialHandler>();
        if(cmh == null){
            Debug.Log("cursor gameobject material not gottem");
        }

        switch(playerId){
            case 0:
                cmh.SetOutlineRed();
                var pf1 = _playerFloors[0].GetComponent<PlayerFloor>();
                pf1.SetPaired(cih);
                pf1.ShowCharacterData(_characters[0]);
                cih.SetPaired(pf1);

                break;
            case 1:
                cmh.SetOutlineBlue();
                var pf2 = _playerFloors[1].GetComponent<PlayerFloor>();
                pf2.SetPaired(cih);
                pf2.ShowCharacterData(_characters[0]);
                cih.SetPaired(pf2);

                break;
            default:
                cmh.SetOutlineBlack();
                Debug.Log("monkey business?");
                break;
        }
        //Set the token's player id, give its reference to the pointer!
    }

    private void InitializeCharacterGrid(){
        for(int i = 0; i < _characters.Count; i++){
            Debug.Log(_characters[i].Name);
            SpawnCharacterBox(_characters[i], _charaBoxPositions[i]);
        }
    }

    private void SpawnCharacterBox(CharacterData character, Transform t){
        GameObject box = Instantiate(_charaBoxPrefab);
        box.transform.position = t.position;

        CharacterBox charaBox = box.GetComponent<CharacterBox>();
        charaBox.SetData(character);
    }

}
