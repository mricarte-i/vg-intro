using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerInputHandler : MonoBehaviour
{
    #region InputActions
    private InputAction walking;
    [Space][SerializeField] private InputActionAsset playerControls;
    #endregion

    #region Properties and Fields
    [SerializeField] private bool _isDummy = false;
    [SerializeField] private int _playerId = 0;
    [SerializeField] private float movementSpeed = 10f;

    [SerializeField] private bool _inverted = false;
    public void Invert(bool isInverted) => _inverted = isInverted;

    [SerializeField] private bool _inputKeyboard = false;
    [SerializeField] private int _controllerId = 0;

    private MoveCommandReceiver _moveCommandReceiver;
    private List<MoveCommand> commands = new List<MoveCommand>();
    //private int currentCmdNum = 0; memento lmao

    #endregion

    void Awake()
    {
        _moveCommandReceiver = new MoveCommandReceiver();

        var gameplayActionMap = playerControls.FindActionMap("Gameplay").Clone();

        if(_inputKeyboard){
            gameplayActionMap.ApplyBindingOverridesOnMatchingControls(Keyboard.current);
            Debug.Log("keyboard");
        }else{
            int i = 0;
            foreach(var device in InputUser.GetUnpairedInputDevices()){
                if(_controllerId == i && device.name.Contains("Controller")){
                    gameplayActionMap.ApplyBindingOverridesOnMatchingControls(device);
                    Debug.Log(device.name);
                    break; //no need to keep iterating
                }
                i++;
            }
        }

        walking = gameplayActionMap.FindAction("Walking");

        walking.performed += OnWalkingPerformed;
        walking.canceled += OnWalkingPerformed;
    }

    #region Walking Boilerplate
    private void OnWalkingPerformed(InputAction.CallbackContext context) {
        var direction = context.ReadValue<Vector2>();

        Horizontal = direction.x;
        Vertical = direction.y;

    }

    void OnEnable() {
        walking.Enable();
    }

    void OnDisable(){
        walking.Disable();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if(!IsMoving || _isDummy) return;

        Move(Direction * movementSpeed * Time.deltaTime);
    }

    private void Move(Vector3 diff){
        MoveCommand moveCommand = new MoveCommand(
            _moveCommandReceiver,
            diff,
            this.gameObject);

        moveCommand.Execute();
        commands.Add(moveCommand);
        //_currentCmdNum++; memento lmao
    }

    #region Movement Properties

    private float Vertical { get; set; }
    private float Horizontal { get; set; }

    private Vector3 Direction {
        get {
            var dir = (transform.forward * Horizontal * (_inverted ? -1 : 1)) + (transform.right * Vertical * (_inverted ? 1 : -1));
            return dir;
        }
    }

    private bool IsMoving => Direction != Vector3.zero;

    #endregion
}
