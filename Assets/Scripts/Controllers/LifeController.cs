using Strategy;
using UnityEngine;

namespace Controllers
{
  public class LifeController : MonoBehaviour, IHittable
    {
        public float MaxLife => _maxLife;
        [SerializeField] private float _maxLife;

        [SerializeField] private float _currentLife;

        [SerializeField] private HealthBar _healthBar;

        public void SetHPBar(HealthBar healthBar){
            _healthBar = healthBar;
            _healthBar.UpdateMaxHealth(MaxLife);
        }

        private void Start()
        {
            _currentLife = MaxLife;
            _healthBar.UpdateMaxHealth(MaxLife);
            //Update Life Event
        }

        public void GetHit(float damage)
        {
            _currentLife -= damage;
            _healthBar.UpdateCurrentHealth(_currentLife);
            //Update Life Event
            if (_currentLife <= 0) Lose();
        }

        public void Lose()
        {
            throw new System.NotImplementedException();
        }
    }
}