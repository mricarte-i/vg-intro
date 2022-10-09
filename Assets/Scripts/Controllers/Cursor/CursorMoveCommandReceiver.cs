using UnityEngine;

public class CursorMoveCommandReceiver
{
    public void MoveOperation(Transform transform, Vector3 diff){
        transform.position += diff;
    }
}