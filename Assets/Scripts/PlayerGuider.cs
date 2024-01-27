using Controllers;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerGuider : MonoBehaviour
{
    [SerializeField] private InputHandler _p1, _p2;
    [SerializeField] private Cinemachine.CinemachineTargetGroup _targetGroup;
    [SerializeField] private float _height = 1.25f;

    public void SetPlayers(InputHandler p1, InputHandler p2){
        _p1 = p1;
        _p2 = p2;
    }

    // Start is called before the first frame update
    /*
    void Start()
    {
        if (_p1 == null || _p2 == null || _targetGroup == null)
        {
            Debug.LogError("Player Guider requires both players to be referenced!");
        }
    }
    */

    // Update is called once per frame
    void LateUpdate()
    {
        if (_p1 == null || _p2 == null || _targetGroup == null)
        {
            //Debug.LogError("Player Guider requires both players to be referenced!");
            return;
        }
        var player1Pos = _p1.transform.position;
        var player2Pos = _p2.transform.position;

        //TODO: create RotateCommand, use that, maybe so players won't auto aim if shooting projectiles?
        _p1.transform.LookAt(new Vector3(player2Pos.x, player1Pos.y, player2Pos.z));
        _p2.transform.LookAt(new Vector3(player1Pos.x, player2Pos.y, player1Pos.z));

        var middle = (player1Pos + player2Pos) / 2;
        transform.position = middle + Vector3.up*_height;
        transform.LookAt(new Vector3(player2Pos.x, transform.position.y, player2Pos.z));

        if(Vector3.Dot(_targetGroup.transform.forward, _p1.transform.right) > 0){
            _p1.Invert(true);
            _p2.Invert(false);
        }else{
            _p1.Invert(false);
            _p2.Invert(true);
        }
    }
}
