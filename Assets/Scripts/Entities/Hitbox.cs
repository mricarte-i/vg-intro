using System;
using System.Collections.Generic;
using Strategy;
using UnityEngine;

namespace Entities
{
    public class Hitbox : MonoBehaviour, IHitbox
    {
        private Action<Collider> _executeOnHit;

        public Hitbox(List<Action<Collider>> actions)
        {
            foreach (var action in actions)
            {
                _executeOnHit += action;
            }
        }
        
        public void OnTriggerEnter(Collider other)
        {
            _executeOnHit(other);
        }
    }
}