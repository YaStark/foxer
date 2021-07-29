using System;

namespace foxer.Core.Game.Entities
{
    public abstract class PlatformEntityBase : EntityBase, IPlatform, IConstruction
    {
        public float Level => Z + GetHeight();

        public ConstructionLevel ConstructionLevel { get; }

        protected PlatformEntityBase(int x, int y, float z, ConstructionLevel constructionLevel) 
            : base(x, y, z)
        {
            ConstructionLevel = constructionLevel;
        }

        public bool Active(Stage stage)
        {
            return true;
        }

        public virtual bool CanSupport(EntityBase entity)
        {
            return true;
        }

        public virtual bool IsColliderFor(Type entityType)
        {
            return true;
        }
    }
}
