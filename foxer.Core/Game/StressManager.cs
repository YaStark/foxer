using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;

namespace foxer.Core.Game
{
    public class StressManager
    {
        private class StressSource
        {
            private readonly Func<EntityBase, int> _weightFactory;
            private readonly Func<EntityBase, int> _distanceFactory;
            
            public int GetWeight(EntityBase entity)
            {
                return _weightFactory.Invoke(entity);
            }

            public int GetDistance(EntityBase entity)
            {
                return _distanceFactory.Invoke(entity);
            }

            public StressSource(int weight, int distance)
            {
                _weightFactory = x => weight;
                _distanceFactory = x => distance;
            }

            public StressSource(Func<EntityBase, int> weightFactory, Func<EntityBase, int> distanceFactory)
            {
                _weightFactory = weightFactory;
                _distanceFactory = distanceFactory;
            }
        }

        private class StressSourceFactory<TEntity>
        {
            private readonly Dictionary<Type, StressSource> _settings;

            public StressSourceFactory(Dictionary<Type, Dictionary<Type, StressSource>> stressers)
            {
                var t = typeof(TEntity);
                if(!stressers.ContainsKey(t))
                {
                    stressers[t] = new Dictionary<Type, StressSource>();
                }

                _settings = stressers[t];
            }

            public StressSourceFactory<TEntity> AddSource<TSource>(int weight, int distance)
            {
                _settings[typeof(TSource)] = new StressSource(weight, distance);
                return this;
            }

            public StressSourceFactory<TEntity> AddSource<TSource>(Func<TSource, int> weightFactory, Func<TSource, int> distanceFactory)
                where TSource : EntityBase
            {
                _settings[typeof(TSource)] = new StressSource(
                    x => weightFactory(x as TSource), 
                    x => distanceFactory(x as TSource));
                return this;
            }
        }

        private static readonly  Dictionary<Type, Dictionary<Type, StressSource>> _stressers = new Dictionary<Type, Dictionary<Type, StressSource>>();
        private static readonly  Dictionary<Type, List<Type>> _treaters = new Dictionary<Type, List<Type>>();
        private Dictionary<Type, int[,]> _stressField = new Dictionary<Type, int[,]>();

        static StressManager()
        {
            AddStresser<SquirrelEntity>()
                .AddSource<WolfEntity>(10, 2)
                .AddSource<PlayerEntity>(player => player.AggressionLevel + 1, player => player.AggressionLevel / 2)
                .AddSource<TreeEntity>(-2, 0);

            AddStresser<BeeEntity>()
                .AddSource<PlayerEntity>(player => player.AggressionLevel / 2, player => player.AggressionLevel / 4);

            AddTreaters();
        }

        public int GetStressLevelInCell(EntityBase entity, int x, int y)
        {
            return _stressField.TryGetValue(entity.GetType(), out var field)
                ? field[x, y]
                : 0;
        }

        public void Update(Stage stage)
        {
            foreach(var stresser in _stressers.Keys)
            {
                if(_stressField.TryGetValue(stresser, out var field))
                {
                    Array.Clear(field, 0, field.Length);
                }
                else
                {
                    _stressField[stresser] = new int[stage.Width, stage.Height];
                }
            }

            for (int i = 0; i < stage.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < stage.Cells.GetLength(1); j++)
                {
                    foreach(var entity in stage.GetEntitesInCell(i, j))
                    {
                        var treater = entity.GetType();
                        if(_treaters.TryGetValue(treater, out var stressers))
                        {
                            foreach(var stresser in stressers)
                            {
                                AddStressSource(
                                    entity,
                                    _stressField[stresser], i, j,
                                    _stressers[stresser][treater]);
                            }
                        }
                    }
                }
            }
        }
        
        private void AddStressSource(EntityBase entity, int[,] field, int x, int y, StressSource settings)
        {
            var weight = settings.GetWeight(entity);
            field[x, y] += weight;
            int ds = settings.GetDistance(entity);

            for (int d = 1; d <= ds; d++)
            {
                int x0 = x - d;
                int x1 = x + d;
                int y0 = y - d;
                int y1 = y + d;
                int val = weight / d;
                if (val == 0)
                {
                    break;
                }

                if (x0 >= 0)
                {
                    for (int i = Math.Max(0, y0); i <= Math.Min(field.GetLength(1) - 1, y1); i++)
                    {
                        field[x0, i] += val;
                    }
                }

                if (x1 < field.GetLength(0))
                {
                    for (int i = Math.Max(0, y0); i <= Math.Min(field.GetLength(1) - 1, y1); i++)
                    {
                        field[x1, i] += val;
                    }
                }

                if (y0 >= 0)
                {
                    for (int i = Math.Max(0, x0); i <= Math.Min(field.GetLength(0) - 1, x1); i++)
                    {
                        field[i, y0] += val;
                    }
                }

                if (y1 < field.GetLength(1))
                {
                    for (int i = Math.Max(0, x0); i <= Math.Min(field.GetLength(0) - 1, x1); i++)
                    {
                        field[i, y1] += val;
                    }
                }
            }
        }

        private static StressSourceFactory<T> AddStresser<T>()
        {
            return new StressSourceFactory<T>(_stressers);
        }

        private static void AddTreaters()
        {
            foreach(var stresser in _stressers)
            {
                foreach(var stressSource in stresser.Value)
                {
                    if(!_treaters.ContainsKey(stressSource.Key))
                    {
                        _treaters[stressSource.Key] = new List<Type>();
                    }

                    _treaters[stressSource.Key].Add(stresser.Key);
                }
            }
        }
    }
}
