using System;
using System.Collections.Generic;
using Controllers;
using Strategy;
using UnityEngine;

namespace Entities
{
    public class Hitbox : MonoBehaviour, IHitbox
    {
        private Action<LifeController> _executeOnHit;

        public void AddActionOnHit(Action<LifeController> action)
        {
            _executeOnHit += action;
        }
        
        public void OnTriggerEnter(Collider other)
        {
            var lifeControllerHit = other.gameObject.GetComponent<LifeController>();
            if(lifeControllerHit != null) _executeOnHit(lifeControllerHit);
        }
    }
}