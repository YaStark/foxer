namespace foxer.Core.Game.Entities
{
    public interface IAttackTarget
    {
        int Hitpoints { get; set; }
        int MaxHitpoints { get; }
    }

    public class SimpleAttackTarget : IAttackTarget
    {
        public int Hitpoints { get; set; }

        public int MaxHitpoints { get; }

        public SimpleAttackTarget(int maxHitpoints)
        {
            Hitpoints = maxHitpoints;
            MaxHitpoints = maxHitpoints;
        }
    }
}
