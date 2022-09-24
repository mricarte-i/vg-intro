using UnityEngine;

public class MoveCommand : Command
{
  private Vector3 _force;
  private MoveCommandReceiver _moveCommandReceiver;
  private CharacterController _characterController;

  public MoveCommand(MoveCommandReceiver receiver, Vector3 force, CharacterController characterController){
    this._moveCommandReceiver = receiver;
    this._force = force;
    this._characterController = characterController;
  }

  public void Execute()
  {
    _moveCommandReceiver.MoveOperation(_characterController, _force);
  }

}
