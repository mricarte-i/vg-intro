using System.Collections.Generic;
using Flyweight;
using JetBrains.Annotations;
using Strategy;
using UnityEngine;

namespace Attacks
{
    public class Attack : MonoBehaviour, IAttack
    {
        [SerializeField] private AttackStats _attackStats;

        [SerializeField] private List<GameObject> _hitboxes = new List<GameObject>();
        public List<GameObject> Hitboxes => _hitboxes;
        public int Damage => _attackStats.Damage;

        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}