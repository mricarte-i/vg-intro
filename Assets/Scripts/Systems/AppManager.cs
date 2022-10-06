using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;
using System;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private PlayableCharacter _player1Device = null, _player2Device = null;
    [SerializeField] private bool _paused = false;

    void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }


        //if you wanted to bind controls that any/every device can use...
        //ex:
        //Controls.Menu.StartGame.performed += context => StartGame();

        //please just listen to me, man
        InputUser.listenForUnpairedDeviceActivity++;
        InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
    }

    private void OnDestroy(){
        //stop listening
        InputUser.onUnpairedDeviceUsed -= OnUnpairedDeviceUsed;
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

        if(_player1Device.controls == null){
            _player1Device.SetInputUser(user);
            _player1Device.SetControls(usersControls);
            Debug.Log("Paired " + control.device.displayName + " to  Player 1! " + _player1Device);
        }else if(_player2Device.controls == null){
            _player2Device.SetInputUser(user);
            _player2Device.SetControls(usersControls);
            Debug.Log("Paired " + control.device.displayName + " to  Player 2! " + _player2Device);
        }else{
            Debug.Log("neither are null!!!");
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

    public PlayableCharacter GetInputUser(int playerId){
        if(playerId == 0){
            return _player1Device;
        }else if(playerId == 1){
            return _player2Device;
        }else{
            throw new Exception("AppManager's GetInputUser being mishandled!");
        }
    }

    void Start(){
        _settingsMenu.SetActive(false);
    }
}
