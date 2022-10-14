using System.Collections.Generic;
using UnityEngine;

namespace Flyweight
{
    [CreateAssetMenu(fileName = "AttackStats", menuName = "Stats/Attacks", order = 0)]
    public class AttackStats : ScriptableObject
    {
        [SerializeField] private AttackStatValues _attackStatValues;
        public int Damage => _attackStatValues.Damage;
        public float Duration => _attackStatValues.Duration;
    }
    
    [System.Serializable]
    public struct AttackStatValues
    {
        public int Damage;
        public float Duration;
    }

}