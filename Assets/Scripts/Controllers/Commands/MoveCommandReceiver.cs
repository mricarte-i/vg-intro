using UnityEngine;

public class MoveCommandReceiver
{
    public void MoveOperation(CharacterController characterController, Vector3 force){
        //gameObject.transform.position += direction;
        //rigidbody.AddForce(force, ForceMode.VelocityChange);
        characterController.Move(force);
    }
}
