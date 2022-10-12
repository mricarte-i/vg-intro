using System;
using Strategy;
using UnityEngine;

namespace Controllers
{
    public class LifeController : MonoBehaviour, IHittable
    {
        public float MaxLife { get; }
        
        [SerializeField] private float _currentLife;

        private void Start()
        {
            _currentLife = MaxLife;
            //Update Life Event
        }

        public void GetHit(float damage)
        {
            _currentLife -= damage;
            //Update Life Event
            if (_currentLife <= 0) Lose();
        }

        public void Lose()
        {
            throw new System.NotImplementedException();
        }
    }
}