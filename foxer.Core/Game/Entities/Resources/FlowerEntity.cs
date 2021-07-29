namespace foxer.Core.Game.Entities
{
    public class FlowerEntity : EntityBase
    {
        public int Kind { get; }

        public FlowerEntity(int x, int y, int kind)
            : base(x, y, 0)
        {
            Kind = kind;
        }

        public override float GetHeight()
        {
            return 0.2f;
        }
    }
}
