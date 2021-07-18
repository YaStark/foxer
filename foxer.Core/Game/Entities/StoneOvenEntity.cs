using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public class StoneOvenEntity : EntityBase
    {
        public StoneOvenEntity(int x, int y) 
            : base(x, y)
        {
        }
    }

    public class BuilderEntity : EntityBase
    {
        public IBuildableItem Item { get; }

        public BuilderEntity(int x, int y, IBuildableItem item) 
            : base(x, y)
        {
            Item = item;
        }
    }
}
