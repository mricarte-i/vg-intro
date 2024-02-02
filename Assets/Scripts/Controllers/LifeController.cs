using System;
using System.Collections;
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

        [SerializeField] private GameObject _hurtLight;

        [SerializeField] private PlayerId _playerId;
        public void SetPlayerId(PlayerId id) => _playerId = id;

        private event Action _onHitEvents;
        public void AddOnHitEvents(Action onHitAction) => _onHitEvents += onHitAction;
        
        private event Action _afterHitEvents;
        public void AddAfterHitEvents(Action afterHitAction) => _afterHitEvents += afterHitAction;

        private event Action _onLoseEvents;
        public void AddOnLoseEvents(Action onLoseEvents) => _onLoseEvents += onLoseEvents;

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
            if(_hurtLight != null) _hurtLight.SetActive(false);
            _currentLife = MaxLife;
            if(_healthBar != null) SetHPBar(_healthBar);
            var colliderComponent = gameObject.GetComponent<Collider>();
            colliderComponent.enabled = true;
            //Update Life Event
        }

        private int _hits = 0;

        public void GetHit(float damage)
        {
            _hits += 1;
            _currentLife -= damage;
            _healthBar.UpdateCurrentHealth(_currentLife);
            _onHitEvents?.Invoke();

            if(_hurtLight != null) _hurtLight.SetActive(true);
            //if(damage > 0) TimeManager.Instance.HitStop(1f/15f);

            //Update Life Event
            if (_currentLife <= 0) Lose();
            else StartCoroutine(Wait(1f/10f));
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            
            _hits -= 1;
            if (_hits == 0 && _hurtLight != null)
            {
                _hurtLight.SetActive(false);
                if (_currentLife > 0) _afterHitEvents?.Invoke();
            }
        }

        public void Lose()
        {
            _hits -= 1;
            if(_hits == 0 && _hurtLight != null) _hurtLight.SetActive(false);
            
            var colliderComponent = gameObject.GetComponent<Collider>();
            colliderComponent.enabled = false;
            _onLoseEvents?.Invoke();
            //TimeManager.Instance.SlowDown(4f);
            EventsManager.Instance.EventPlayerDeath(_playerId);
        }
    }
}