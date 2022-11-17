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
        
        

        #region Take Damage

        bool IsTakeDamage();
        void TriggerTakeDamage();

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