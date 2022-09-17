using UnityEngine;

[ExecuteInEditMode]
public class PlayerGuider : MonoBehaviour
{
    [SerializeField] private Transform _p1, _p2;
    [SerializeField] private float _height = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        if (_p1 == null || _p2 == null)
        {
            Debug.LogError("Player Guider requires both players to be referenced!");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_p1 == null || _p2 == null)
        {
            Debug.LogError("Player Guider requires both players to be referenced!");
            return;
        }
        var player1Pos = _p1.position;
        var player2Pos = _p2.position;
        _p1.LookAt(new Vector3(player2Pos.x, player1Pos.y, player2Pos.z));
        _p2.LookAt(new Vector3(player1Pos.x, player2Pos.y, player1Pos.z));

        var middle = (player1Pos + player2Pos) / 2;
        transform.position = middle + Vector3.up*_height;
        transform.LookAt(new Vector3(player2Pos.x, transform.position.y, player2Pos.z));
    }
}
