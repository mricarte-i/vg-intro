using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;
using System;

public class CharacterSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject _playerPointerPrefab;
    [SerializeField] private GameObject _playerTokenPrefab;
    [SerializeField] private GameObject _canvas;



    // Start is called before the first frame update
    void Start()
    {
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

        if(AppManager.Instance.IsNotPlayerControlled(0)){
            AppManager.Instance.SetPlayerControl(0, user, usersControls, control);
            InitializePlayerPointer(AppManager.Instance.GetInputUser(0));
        }else if(AppManager.Instance.IsNotPlayerControlled(1)){
            AppManager.Instance.SetPlayerControl(1, user, usersControls, control);
            InitializePlayerPointer(AppManager.Instance.GetInputUser(1));
        }else{
            Debug.Log("neither are null!!!");
        }
    }

    private void InitializePlayerPointer(PlayableCharacter player){
        if(player == null){
            throw new System.Exception("how dare you");
        }

        //instatiation order matters!
        GameObject gop = Instantiate(_playerPointerPrefab, _canvas.transform);

        PlayerPointer pc = gop.GetComponent<PlayerPointer>();
        pc.SetInputUser(player.inputUser);
        pc.BindControls(player.controls);

        //Set the token's player id, give its reference to the pointer!
    }



}
