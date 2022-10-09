using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System.Collections.Generic;

public class PlayerPointer : MonoBehaviour
{
    private InputAction movement;
    private InputAction confirm;
    private InputAction ready;
    private InputAction cancel;

    [Space][SerializeField] private InputActionAsset playerControls;
    [SerializeField] private InputUser _user;
    public void SetInputUser(InputUser newUser) => _user = newUser;

    [SerializeField] private float _speed;
    [SerializeField] private int _playerId = 0;

    private CharacterGrid _charaGrid;
    private GraphicRaycaster _gr;
    private PointerEventData _pointerEventData = new PointerEventData(null);
    [SerializeField] private Transform _currentChara;

    void Start(){
        _gr = GetComponentInParent<GraphicRaycaster>();
        _charaGrid = GetComponentInParent<CharacterGrid>();

    }

    // Update is called once per frame
    void Update()
    {
        if(_user == null){
            Debug.Log("oh no");
            return;
        }

        if(IsMoving){
            //TODO: refactor to command pattern
            transform.position += Direction * Time.deltaTime * _speed;

            /* CLAMPING DOESN;T WORK AS HOPED
            Vector3 worldSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            Debug.Log("world size " + worldSize);

            Debug.Log("screen width " + Screen.width);
            Debug.Log("screen height " + Screen.height);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -worldSize.x, worldSize.x),
                Mathf.Clamp(transform.position.y, -worldSize.y, worldSize.y),
                transform.position.z
            );
            */
        }

        if(IsConfirmPressed){
            _pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
            var resultAppendList = new List<RaycastResult>();
            _gr.Raycast(_pointerEventData, resultAppendList);
            Debug.Log("CONFIRMING! " + resultAppendList.Count);

            if(resultAppendList.Count > 0){
                Transform raycastChara = resultAppendList[0].gameObject.transform;
                SetCurrentCharacter(raycastChara);
            }else{
                if(_currentChara != null){
                    SetCurrentCharacter(null);
                }
            }
        }

        if(IsReadyPressed){
            if(_currentChara != null){
                Debug.Log("READY! " + _currentChara.name);
                _charaGrid.ConfirmCharacter(_playerId, _charaGrid.GetCharacter(_currentChara.GetSiblingIndex()));
            }
        }

        if(IsCancelPressed){
            _charaGrid.ConfirmCharacter(_playerId, null);
        }

    }

    private void SetCurrentCharacter(Transform t){
        _currentChara = t;

        if(t != null){
            var index = t.GetSiblingIndex();
            CharacterData chara = _charaGrid.GetCharacter(index);
            _charaGrid.ShowCharacterInSlot(_playerId, chara);
        }else{
            _charaGrid.ShowCharacterInSlot(0, null);
        }
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
        Debug.Log("pressed confirm!");
        IsConfirmPressed = context.ReadValueAsButton();
    }

    private void OnReadyPerformed(InputAction.CallbackContext context) {
        Debug.Log("pressed ready!");
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
            return new Vector3(Horizontal, Vertical, 0);
        }
    }

    #endregion
}
