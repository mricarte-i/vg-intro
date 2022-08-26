using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuider : MonoBehaviour
{
    [SerializeField] private Transform[] _players;

    // Start is called before the first frame update
    void Start()
    {
        if (_players.Length != 2)
        {
            Debug.LogError("Player Guider requires both players to be referenced!");
            Debug.Break();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var player1Pos = _players[0].position;
        var player2Pos = _players[1].position;
        var middle = (player1Pos - player2Pos) / 2;
        _players[0].LookAt(new Vector3(player2Pos.x, player1Pos.y, player2Pos.z));
        _players[1].LookAt(new Vector3(player1Pos.x, player2Pos.y, player1Pos.z));
        

    }
}
