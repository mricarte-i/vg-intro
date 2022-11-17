namespace Strategy
{
    public interface ICharaterAnimatorController
    {
        bool IsIdle();
        void TriggerIdle();

        bool IsWalkingForwards();
        void TriggerWalkingForwards();

        bool IsWalkingBackwards();
        void TriggerWalkingBackwards();

        bool IsJumping();
        void TriggerJump();

        bool IsUpperAttack();
        void TriggerUpperAttack();

        bool IsNeutralAttack();
        void TriggerNeutralAttack();

        bool IsDownAttack();
        void TriggerDownAttack();
    }
}