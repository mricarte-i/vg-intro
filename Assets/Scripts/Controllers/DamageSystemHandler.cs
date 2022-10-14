using Attacks;
using UnityEngine;

namespace Controllers
{
  public class DamageSystemHandler : MonoBehaviour
    {
        [SerializeField] private NeutralAttack _neutralAttack;
        [SerializeField] private LifeController _hurtbox;
        public LifeController GetHurtbox => _hurtbox;

        private void Start()
        {
            _neutralAttack.SetHurtBox(_hurtbox);
        }

        public void DoNeutralAttack()
        {
            _neutralAttack.Execute();
        }
    }
}