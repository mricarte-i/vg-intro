using UnityEngine;

public class CursorMoveCommand : Command
{
  private Vector3 _diff;
  private CursorMoveCommandReceiver _moveCommandReceiver;
  private Transform _transform;

  public CursorMoveCommand(CursorMoveCommandReceiver receiver, Vector3 diff, Transform transform){
    this._moveCommandReceiver = receiver;
    this._diff = diff;
    this._transform = transform;
  }

  public void Execute()
  {
    _moveCommandReceiver.MoveOperation(_transform, _diff);
  }

}