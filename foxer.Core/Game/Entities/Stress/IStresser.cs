using System;
using System.Collections.Generic;

namespace foxer.Core.Game.Entities.Stress
{
    public interface IStresser
    {
        Type EntityType { get; }
        IReadOnlyCollection<Type> TreatersType { get; }
        StressInfo GetStressInfoFor(EntityBase aggressor);
    }
}
