using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour, IProyectile
{
    public float Lifetime {
        get { return _lifetime; }
        set { _lifetime = value; }
    }
    [SerializeField] private float _lifetime = 5f;
    
    public float Speed {
        get { return _speed; }
        set { _speed = value; }
    }
    [SerializeField] private float _speed = 10f;

    public float Dropoff {
        get { return _dropoff; }
        set { _dropoff = value; }
    }
    [SerializeField] private float _dropoff = 5f;

    public float Damage {
        get { return _damage; }
        set { _damage = value; }
    }
    [SerializeField] private float _damage = 10f;

    public ProyectileLauncher Owner => _owner;
    [SerializeField] private ProyectileLauncher _owner;

    public Rigidbody Rigidbody => _rigidbody;
    [SerializeField] private Rigidbody _rigidbody;

    public Collider Collider => _collider;
    [SerializeField] private Collider _collider;

    [SerializeField] private List<int> _layerTarget;

    #region I_PROYECTILE_METHODS
    public void Travel(){
        Vector3 movementVector = (Vector3.forward * Speed) + (Vector3.down * Dropoff);
        transform.Translate(movementVector * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (_layerTarget.Contains(collision.gameObject.layer))
        {
            // TODO: implement these
            /*
            IDamageable damageable = collider.GetComponent<IDamageable>();
            damageable?.TakeDamage(_damage);
            */

            Destroy(this.gameObject);
        }
    }
    #endregion

    #region UNITY_EVENTS
    void Start() 
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _collider.isTrigger = true;
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    void Update()
    {
        Travel();

        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0) Destroy(this.gameObject);
    }
    #endregion

    public void SetOwner(ProyectileLauncher owner) => _owner = owner;
}