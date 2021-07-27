namespace foxer.Core.Game.Entities
{
    public class GrassRoofEntity : PlatformEntityBase
    {
        public GrassRoofEntity(int x, int y, float z)
            : base(x, y, z)
        {
        }

        public override float GetHeight()
        {
            return 0.4f;
        }

        public override bool CanSupport(EntityBase entity)
        {
            return false;
        }

        public override bool CanBeCreated(Stage stage, int x, int y, IPlatform platform)
        {
            return base.CanBeCreated(stage, x, y, platform)
                && platform is IWall;
        }
    }
}
