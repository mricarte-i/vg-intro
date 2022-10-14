namespace Strategy
{
    public interface IHittable
    {
        float MaxLife { get; }

        void GetHit(float damage);

        void Lose();
    }
}