namespace foxer.Core.Game.Entities
{
    public class StoneOvenEntity : PlatformEntityBase
    {
        public StoneOvenEntity(int x, int y, float z) 
            : base(x, y, z)
        {
        }

        public override float GetHeight()
        {
            return 0.4f;
        }
    }
}
