using System;
using UnityEngine;

namespace Entities
{
    public class LifeCountdown : MonoBehaviour
    {
        [SerializeField] private float _life;
        private float _currentLife;
        private float _timeElapsed;

        public void Start()
        {
            _currentLife = _life;
            _timeElapsed = 0;
        }

        public void Update()
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed > 1)
            {
                _currentLife -= 1f;
                if (_currentLife <= 0)
                {
                    Destroy(gameObject);
                }

                _timeElapsed = 0;
            }
        }
    }
}