using UnityEngine;

public class CursorMoveCommand : Command
{
  private Vector3 _diff;
  private CursorMoveCommandReceiver _moveCommandReceiver;
  private Rigidbody _rb;

  public CursorMoveCommand(CursorMoveCommandReceiver receiver, Vector3 diff, Rigidbody rb){
    this._moveCommandReceiver = receiver;
    this._diff = diff;
    this._rb = rb;
  }

  public void Execute()
  {
    _moveCommandReceiver.MoveOperation(_rb, _diff);
  }

}