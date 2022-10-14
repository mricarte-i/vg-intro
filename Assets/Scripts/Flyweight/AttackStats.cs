using UnityEngine;

namespace Flyweight
{
  [CreateAssetMenu(fileName = "AttackStats", menuName = "Attack Stats", order = 0)]
    public class AttackStats : ScriptableObject
    {
        [SerializeField] private AttackStatValues _attackStatValues;
        public int Damage => _attackStatValues.Damage;
        public float Duration => _attackStatValues.Duration;
        public AudioClip SFXTHROW => _attackStatValues.SFXTHROW;
        public AudioClip SFXCONTACT => _attackStatValues.SFXCONTACT;
    }

    [System.Serializable]
    public struct AttackStatValues
    {
        public int Damage;
        public float Duration;
        public AudioClip SFXTHROW, SFXCONTACT;
    }

}