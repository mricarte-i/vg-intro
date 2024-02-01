using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private bool _bobbingEnabled = false;
    [SerializeField] private float _bobbingRate = 0.1f;
    [SerializeField] private Vector3 _bobbingVector = Vector3.up;
    [Space]
    [SerializeField] private float _rotationSpeed = 0.1f;
    [SerializeField] private Vector3 _rotationVector = Vector3.up;

    private Vector3 _initialPos;
    private Quaternion _initialRot;

    void Awake()
    {
        _initialPos = transform.position;
        _initialRot = transform.rotation;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_bobbingEnabled)
        {
            transform.position = _initialPos + (_bobbingVector * Mathf.Sin(_bobbingRate * Time.deltaTime));
        }
        transform.Rotate(_rotationVector * (_rotationSpeed * Time.deltaTime));
    }
}
