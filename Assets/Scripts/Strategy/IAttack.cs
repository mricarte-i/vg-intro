using UnityEngine;

namespace Strategy
{
    public interface IAttack : Command
    {
        Collider Hitbox { get; }
        int Damage { get; }

        void OnTriggerEnter(Collider collision);
    }
}