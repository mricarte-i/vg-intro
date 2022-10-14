using System;
using System.Collections.Generic;
using Entities;

namespace Strategy
{
    public interface IAttack : Command
    {
        List<Hitbox> Hitboxes { get; }
        
        int Damage { get; }
        
        bool IsAttacking { get; }
        
        float Duration { get; }

        void AddBeforeAttackingEvent(Action beforeAttackingAction);
        void AddAfterAttackingEvent(Action beforeAttackingAction);
    }
}