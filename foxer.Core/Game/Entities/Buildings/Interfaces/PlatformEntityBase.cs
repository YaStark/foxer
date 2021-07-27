using System;

namespace foxer.Core.Game.Entities
{
    public abstract class PlatformEntityBase : EntityBase, IPlatform
    {
        protected PlatformEntityBase(int x, int y, float z) 
            : base(x, y, z)
        {
        }

        public float Level => Z + GetHeight();

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
