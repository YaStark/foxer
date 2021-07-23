namespace foxer.Core.Game.Entities
{
    public class GrassFloorEntity : EntityBase, IPlatform
    {
        public GrassFloorEntity(int x, int y)
            : base(x, y, 0)
        {
        }

        public float Level => Z + 0.2f;

        public bool Active(Stage stage)
        {
            return true;
        }
    }
}
