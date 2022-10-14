using UnityEngine;

namespace Strategy
{
    public interface IHitbox
    {
        void OnTriggerEnter(Collider other);
    }
}