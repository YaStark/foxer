namespace foxer.Core.Game.Entities
{
    public class GrassFloorEntity : PlatformEntityBase
    {
        public GrassFloorEntity(int x, int y, float z)
            : base(x, y, z)
        {
        }

        public override float GetHeight()
        {
            return 0.2f;
        }

        public override bool CanSupport(EntityBase entity)
        {
            return !(entity is GrassFloorEntity)
                && !(entity is GrassRoofEntity);
        }
    }
}
