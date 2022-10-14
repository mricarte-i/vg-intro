using UnityEngine;

public class CharacterPushInteraction : MonoBehaviour
{
    [SerializeField] private Vector3 _speed;

    public void OnTriggerStay(Collider col){
        var ctrl = col.gameObject.GetComponent<CharacterController>();
        if(ctrl && this.GetComponentInParent<CharacterController>() != ctrl){
            ctrl.SimpleMove(_speed);
        }
    }

    public void Speed(Vector3 speed) => _speed = speed;
}
