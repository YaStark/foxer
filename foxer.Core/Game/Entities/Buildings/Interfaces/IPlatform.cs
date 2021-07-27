using System;

namespace foxer.Core.Game.Entities
{
    public interface IPlatform
    {
        float Z { get; }

        float Level { get; }

        bool Active(Stage stage);

        bool CanSupport(EntityBase entity);

        bool IsColliderFor(Type entityType);
    }

    public class DefaultStagePlatform : IPlatform
    {
        public float Z { get; } = 0;

        public float Level { get; } = 0;

        public bool Active(Stage stage)
        {
            return true;
        }

        public bool IsColliderFor(Type entityType)
        {
            return false;
        }

        public bool CanSupport(EntityBase entity)
        {
            return true;
        }
    }
}
