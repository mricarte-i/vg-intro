using UnityEngine;

namespace Strategy
{
    public interface IExplosiveAttack : IAttack
    {
        GameObject ExplosionPrefab { get; }
        float DeltaPositionForward { get; }
        float DeltaPositionY { get; }
        float ExplosionDelay { get; }
    }
}