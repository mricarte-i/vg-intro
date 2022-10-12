using Flyweight;
using Strategy;
using UnityEngine;

namespace Attacks
{
    public class Attack : MonoBehaviour, IAttack
    {
        [SerializeField] private AttackStats _attackStats;

        public Collider Hitbox => _attackStats.Hitbox;
        public int Damage => _attackStats.Damage;
        
        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<CharacterBox>()){
                var chara = other.gameObject.GetComponent<CharacterBox>().GetCharacterData();
                Debug.Log("Banana");
            }
            Debug.Log("Manzana");
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}