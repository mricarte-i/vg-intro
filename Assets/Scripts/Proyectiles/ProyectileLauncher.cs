using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileLauncher : MonoBehaviour, IProyectileLauncher
{
    [SerializeField] private ProyectileData _stats;

    public GameObject Proyectile => _stats.Proyectile;
    public float ProyectileCount => _stats.ProyectileCount;
    public float ProyectileDelay => _stats.ProyectileDelay;

    private bool isAttacking = false;
    private int currentCount = 0;
    private float currentLauncherTime = 0.0f;

    private void Start()
    {
        // nothing
    }

    public virtual void Attack()
    {
        if(isAttacking) return;

        isAttacking = true;
        currentCount = 0;
        currentLauncherTime = 0.0f;
    }

    private void Update()
    {
        if(isAttacking) {
            currentLauncherTime += Time.deltaTime;

            if(currentLauncherTime > currentCount * ProyectileDelay)
            {
                var proyectile = Instantiate(
                    Proyectile,
                    transform.position,
                    transform.rotation);
                proyectile.name = "Proyectile";
                Proyectile proyectileInstance = proyectile.GetComponent<Proyectile>();
                proyectileInstance.SetOwner(this);
                proyectileInstance.Speed = _stats.Speed;
                proyectileInstance.Dropoff = _stats.Dropoff;
                proyectileInstance.Lifetime = _stats.Lifetime;
                proyectileInstance.Damage = _stats.Damage;

                currentCount++;
            }

            if(currentCount >= ProyectileCount)
            {
                isAttacking = false;
                currentCount = 0;
                currentLauncherTime = 0.0f;
            }
        }
    }
}
