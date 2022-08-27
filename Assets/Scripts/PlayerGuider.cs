using UnityEngine;

public class PlayerGuider : MonoBehaviour
{
    [SerializeField] private Transform[] _players;
    [SerializeField] private float _height = 1.25f;

    private Camera _camera;
    private float _aspectRatio;
    private float _tanFOV;
    [SerializeField] private float _distanceMargin = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (_players.Length != 2)
        {
            Debug.LogError("Player Guider requires both players to be referenced!");
            Debug.Break();
        }
        _camera = GetComponentInChildren<Camera>();

        _camera.transform.rotation = Quaternion.Euler(0, -90, 0);
        _camera.transform.position = new Vector3(3, 0, 0);

        _aspectRatio = Screen.width / Screen.height;
        _tanFOV = Mathf.Tan(Mathf.Deg2Rad * _camera.fieldOfView / 2.0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var player1Pos = _players[0].position;
        var player2Pos = _players[1].position;
        var middle = (player1Pos + player2Pos) / 2;
        var dist = Vector3.Distance(player1Pos, player2Pos);
        _players[0].LookAt(new Vector3(player2Pos.x, player1Pos.y, player2Pos.z));
        _players[1].LookAt(new Vector3(player1Pos.x, player2Pos.y, player1Pos.z));

        transform.position = middle + Vector3.up*_height;
        transform.LookAt(new Vector3(player2Pos.x, transform.position.y, player2Pos.z));

        var cameraDist = (dist / 2 / _aspectRatio) / _tanFOV;
        _camera.transform.position = transform.position + transform.right * (cameraDist + _distanceMargin);
    }
}
