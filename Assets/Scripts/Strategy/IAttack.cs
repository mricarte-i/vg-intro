using System.Collections.Generic;
using UnityEngine;

namespace Strategy
{
    public interface IAttack : Command
    {
        List<GameObject> Hitboxes { get; }
        
        int Damage { get; }
        
        bool IsAttacking { get; }
        
        float Duration { get; }
    }
}