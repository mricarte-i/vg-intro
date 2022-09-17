using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
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

    private void MoveRight() { Move(Vector3.forward); }
    private void MoveLeft() { Move(Vector3.back); }
    private void MoveUp() { Move(Vector3.left); }
    private void MoveDown() { Move(Vector3.right); }

    #region Movement Properties

    private float Vertical {
        get{
            var keyboard = Keyboard.current;
            var vertical = 0;

            if(keyboard.wKey.isPressed){
                vertical = 1;
            }else if(keyboard.sKey.isPressed){
                vertical = -1;
            }
            return (_inverted ? 1 : -1) * vertical;
        }

    }
    private float Horizontal {
        get{
            var keyboard = Keyboard.current;
            var horizontal = 0;

            if(keyboard.dKey.isPressed){
                horizontal = 1;
            }else if(keyboard.aKey.isPressed){
                horizontal = -1;
            }
            return (_inverted ? -1 : 1) * horizontal;
        }

    }

    private Vector3 Direction {
        get {
            var dir = (transform.forward * Horizontal) + (transform.right * Vertical);
            return dir;
        }
    }
    // => new Vector3(Horizontal, 0, Vertical);
    private bool IsMoving => Direction != Vector3.zero;

    #endregion
}
