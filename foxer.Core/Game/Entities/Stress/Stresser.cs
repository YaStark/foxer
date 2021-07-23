using System;
using System.Collections.Generic;

namespace foxer.Core.Game.Entities.Stress
{
    public class Stresser<TEntity> : IStresser
    {
        private interface IStressItem
        {
            StressInfo GetStressInfoFor(EntityBase aggressor);
        }

        private class StressItem<TTreater> : IStressItem
            where TTreater : EntityBase
        {
            private readonly Func<TTreater, StressInfo> _stressInfoFactory;
            
            public StressItem(Func<TTreater, StressInfo> stressInfoFactory)
            {
                _stressInfoFactory = stressInfoFactory;
            }

            public StressInfo GetStressInfoFor(EntityBase aggressor)
            {
                return _stressInfoFactory.Invoke((TTreater)aggressor);
            }
        }

        private readonly Dictionary<Type, IStressItem> _stressItems = new Dictionary<Type, IStressItem>();

        public Type EntityType { get; } = typeof(TEntity);

        public IReadOnlyCollection<Type> TreatersType => _stressItems.Keys;

        public Stresser<TEntity> Add<TTreater>(Func<TTreater, StressInfo> stressInfoFactory)
            where TTreater : EntityBase
        {
            _stressItems[typeof(TTreater)] = new StressItem<TTreater>(stressInfoFactory);
            return this;
        }

        public Stresser<TEntity> Add<TTreater>(StressInfo stressInfo)
            where TTreater : EntityBase
        {
            _stressItems[typeof(TTreater)] = new StressItem<TTreater>(e => stressInfo);
            return this;
        }

        public StressInfo GetStressInfoFor(EntityBase aggressor)
        {
            return _stressItems[aggressor.GetType()].GetStressInfoFor(aggressor);
        }
    }
}
