using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region InputActions
    [SerializeField] private InputAction movement;
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

    // Start is called before the first frame update
    void Start()
    {
        _moveCommandReceiver = new MoveCommandReceiver();

        movement.performed += OnMovementPerformed;
        movement.canceled += OnMovementPerformed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext context) {
        var direction = context.ReadValue<Vector2>();

        Horizontal = direction.x;
        Vertical = direction.y;

    }

    void OnEnable() {
        movement.Enable();
    }

    void OnDisable(){
        movement.Disable();
    }

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
    // => new Vector3(Horizontal, 0, Vertical);
    private bool IsMoving => Direction != Vector3.zero;

    #endregion
}
