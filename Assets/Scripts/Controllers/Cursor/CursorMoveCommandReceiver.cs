using UnityEngine;

public class CursorMoveCommandReceiver
{
    public void MoveOperation(Rigidbody rb, Vector3 diff){
        //transform.position += diff;
        rb.MovePosition(rb.position + diff);
    }
}