using UnityEngine;

namespace Flyweight
{
    [CreateAssetMenu(fileName = "AttackStats", menuName = "Stats/Attacks", order = 0)]
    public class AttackStats : ScriptableObject
    {
        [SerializeField] private AttackStatValues _attackStatValues;
        
        public Collider Hitbox => _attackStatValues.Hitbox;
        public int Damage => _attackStatValues.Damage;
    }
    
    [System.Serializable]
    public struct AttackStatValues
    {
        public Collider Hitbox { get; }
        public int Damage { get; }
    }

}