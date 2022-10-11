using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CursorInputHandler : MonoBehaviour
{
    private InputAction movement;
    private InputAction confirm;
    private InputAction ready;
    private InputAction cancel;

    [Space]
    [SerializeField] private InputActionAsset playerControls;
    [SerializeField] private InputUser _user;
    public void SetInputUser(InputUser newUser) => _user = newUser;

    [SerializeField] private float _speed;
    [SerializeField] private int _playerId = 0;
    private PlayerFloor _playerFloor;

    private CursorMoveCommandReceiver _moveCommandReceiver;
    private List<CursorMoveCommand> commands = new List<CursorMoveCommand>();
    [SerializeField] private BoxCollider _cursorCollider;
    private Rigidbody _rb;
    [SerializeField] private bool _mode = false;



    // Start is called before the first frame update
    void Start()
    {
        _moveCommandReceiver = new CursorMoveCommandReceiver();
        _cursorCollider = GetComponent<BoxCollider>();
        _cursorCollider.isTrigger = true;
        _rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if(_user == null){
            Debug.Log("oh no");
            return;
        }

        if(_mode){
            return;
        }

        if(IsMoving){
            CursorMoveCommand moveCommand = new CursorMoveCommand(
                _moveCommandReceiver,
                Direction * _speed * Time.deltaTime,
                _rb);
            moveCommand.Execute();
            commands.Add(moveCommand);
        }

        if(IsConfirmPressed){
            //check if stepping on a "button" trigger, get its info

            //if trigger is character cell, tell CharacterSelectMenu to ShowCharacterInSlot given its character data

            //if trigger, is next, tell CSM that IsReady+1, if IsReady == 2, move onto next part of the menu
            //(and ConfirmCharacter for each player)

            //if trigger is stage cell, tell CharacterSelectMenu to ConfirmStage
            //if trigger is mode cell, tell CSM to SetGameMode
            //if trigger, is next, tell CSM that IsReady+1, if IsReady == 2, move onto next part of the menu

            //if trigger is music cell, tell CSM to ConfirmMusicTrack
            //if trigger is BPM cell, tell CSM to ConfirmConductorBPM
            //if trigger, is next, tell CSM that IsReady+1, if IsReady == 2, load correct fight level scene

        }

    }

    public void SetCinematicMode(bool mode){
        _mode = mode;
    }

    public void SetPaired(PlayerFloor pf){
        _playerFloor = pf;
    }

    void OnTriggerEnter(Collider other){
        if(_mode){
            return;
        }
        CursorHover(other);
    }
    void OnTriggerStay(Collider other){
        if(_mode){
            return;
        }
        CursorHover(other);
    }

    private void CursorHover(Collider other){

        if(other.gameObject.GetComponent<CharacterBox>()){
            var chara = other.gameObject.GetComponent<CharacterBox>().GetCharacterData();
            _playerFloor.ShowCharacterData(chara);
            if(IsConfirmPressed){
                AppManager.Instance.SetPlayerCharacter(_playerId, chara);
            }
        }else if(other.gameObject.GetComponent<ReadyBox>()){
            if(IsConfirmPressed){
                other.gameObject.GetComponent<ReadyBox>().CalledReady(_playerId);
            }
        }else if(other.gameObject.GetComponent<StageBox>()){
            var stage = other.gameObject.GetComponent<StageBox>().GetStageData();
            if(IsConfirmPressed){
                AppManager.Instance.SetStage(stage);
            }
        }
    }

    void OnTriggerExit(Collider other){
        if(AppManager.Instance.GetInputUser(_playerId).characterData != null){
            _playerFloor.ShowCharacterData(AppManager.Instance.GetInputUser(_playerId).characterData);
        }
    }

    public void SetPlayerId(int id){
        _playerId = id;
    }

    public void BindControls(GeneratedPlayerControls controls){
        var menuActionMap = controls.Menu;

        movement = menuActionMap.Movement;
        confirm = menuActionMap.Confirm;
        ready= menuActionMap.Ready;
        cancel = menuActionMap.Cancel;


        movement.started += OnMovementPerformed;
        movement.performed += OnMovementPerformed;
        movement.canceled += OnMovementPerformed;

        confirm.started += OnConfirmPerformed;
        confirm.canceled += OnConfirmPerformed;

        ready.started += OnReadyPerformed;
        ready.canceled += OnReadyPerformed;

        cancel.started += OnCancelPerformed;
        cancel.canceled += OnCancelPerformed;

        //should be called OnEnable...
        movement.Enable();
        confirm.Enable();
        cancel.Enable();

    }

    private void OnMovementPerformed(InputAction.CallbackContext context) {
        var direction = context.ReadValue<Vector2>();

        Horizontal = direction.x;
        Vertical = direction.y;

    }

    private void OnConfirmPerformed(InputAction.CallbackContext context) {
        IsConfirmPressed = context.ReadValueAsButton();
    }

    private void OnReadyPerformed(InputAction.CallbackContext context) {
        IsConfirmPressed = context.ReadValueAsButton();
    }

    private void OnCancelPerformed(InputAction.CallbackContext context) {
        IsCancelPressed = context.ReadValueAsButton();
    }

    private bool IsConfirmPressed { get; set; }
    private bool IsReadyPressed { get; set; }
    private bool IsCancelPressed { get; set; }

    #region Movement Properties

    //names based on typical controller naming...
    private float Vertical { get; set; }
    private float Horizontal { get; set; }
    private bool IsMoving => Direction != Vector3.zero;

    private Vector3 Direction {
        get {
            return new Vector3(Horizontal, 0, Vertical);
        }
    }

    #endregion
}
