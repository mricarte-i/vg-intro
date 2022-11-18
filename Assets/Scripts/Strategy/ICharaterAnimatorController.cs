namespace Strategy
{
    public interface ICharaterAnimatorController
    {
        #region Movement

        bool IsIdle();
        void TriggerIdle();

        bool IsWalkingForwards();
        void TriggerWalkingForwards();

        bool IsWalkingBackwards();
        void TriggerWalkingBackwards();

        bool IsJumping();
        void TriggerJump();

        #endregion
        
        

        #region Damage

        bool IsDying();
        void TriggerDying();

        bool IsTakeDamage();
        void TriggerTakeDamage();

        bool IsReseting();
        void TriggerReset();

        #endregion



        #region Attacks

        bool IsUpperAttack();
        void TriggerUpperAttack();

        bool IsNeutralAttack();
        void TriggerNeutralAttack();

        bool IsDownAttack();
        void TriggerDownAttack();

        #endregion
    }
}