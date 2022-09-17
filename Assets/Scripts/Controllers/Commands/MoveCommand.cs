using UnityEngine;

public class MoveCommand : Command
{
  private Vector3 _direction;
  private MoveCommandReceiver _moveCommandReceiver;
  private GameObject _gameObject;

  public MoveCommand(MoveCommandReceiver receiver, Vector3 direction, GameObject gameObject){
    this._moveCommandReceiver = receiver;
    this._direction = direction;
    this._gameObject = gameObject;
  }

  public void Execute()
  {
    _moveCommandReceiver.MoveOperation(_gameObject, _direction);
  }

}
