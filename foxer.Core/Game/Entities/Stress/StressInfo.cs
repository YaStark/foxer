namespace foxer.Core.Game.Entities.Stress
{
    public class StressInfo
    {
        public float Power { get; }
        public float Distance { get; }

        public StressInfo(float power, float distance)
        {
            Power = power;
            Distance = distance;
        }
    }
}
