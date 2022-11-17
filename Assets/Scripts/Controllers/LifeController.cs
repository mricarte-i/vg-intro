using System;
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

        [SerializeField] private PlayerId _playerId;
        public void SetPlayerId(PlayerId id) => _playerId = id;

        private event Action _onHitEvents;
        public void AddOnHitEvents(Action onHitAction) => _onHitEvents += onHitAction;

        public void SetHPBar(HealthBar healthBar){
            _healthBar = healthBar;
            _healthBar.UpdateMaxHealth(MaxLife);
            _healthBar.UpdateCurrentHealth(_currentLife);
        }

        public void Reset(){
            Start();
        }

        private void Start()
        {
            _currentLife = MaxLife;
            if(_healthBar != null) SetHPBar(_healthBar);
            //Update Life Event
        }

        public void GetHit(float damage)
        {
            _currentLife -= damage;
            _healthBar.UpdateCurrentHealth(_currentLife);
            _onHitEvents?.Invoke();
            //Update Life Event
            if (_currentLife <= 0) Lose();
        }

        public void Lose()
        {
            EventsManager.Instance.EventPlayerDeath(_playerId);
        }
    }
}