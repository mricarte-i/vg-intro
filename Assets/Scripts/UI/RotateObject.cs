using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 0.1f;
    [SerializeField] private Vector3 _rotationVector = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotationVector * (_rotationSpeed * Time.deltaTime));
    }
}
