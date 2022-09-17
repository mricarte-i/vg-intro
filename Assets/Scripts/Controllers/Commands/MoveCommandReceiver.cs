using UnityEngine;

public class MoveCommandReceiver
{
    public void MoveOperation(GameObject gameObject, Vector3 direction){
        gameObject.transform.position += direction;
    }
}
