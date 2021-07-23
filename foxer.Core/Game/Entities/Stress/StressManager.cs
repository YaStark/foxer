using foxer.Core.Game.Entities.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Core.Game.Entities.Stress
{
    public class StressManager
    {
        private readonly IStresser[] _stressers;
        private Dictionary<Type, float[,]> _stressField = new Dictionary<Type, float[,]>();

        public StressManager(IReadOnlyList<EntityDescriptorBase> entityDescriptors)
        {
            _stressers = entityDescriptors.Select(ed => ed.Stresser).Where(s => s != null).ToArray();
        }

        public float GetStressLevelInCell(EntityBase entity, int x, int y)
        {
            return _stressField.TryGetValue(entity.GetType(), out var field)
                ? field[x, y]
                : 0;
        }

        public void Update(Stage stage)
        {
            foreach(var stresser in _stressers)
            {
                if(_stressField.TryGetValue(stresser.EntityType, out var field))
                {
                    Array.Clear(field, 0, field.Length);
                }
                else
                {
                    _stressField[stresser.EntityType] = new float[stage.Width, stage.Height];
                }

                foreach (var treaterType in stresser.TreatersType)
                {
                    foreach(var treaterEntity in stage.GetEntitesByType(treaterType))
                    {
                        AddStressSource(
                            treaterEntity,
                            _stressField[stresser.EntityType],
                            stresser);
                    }
                }
            }
        }
        
        private void AddStressSource(EntityBase entity, float[,] field, IStresser settings)
        {
            int x = entity.CellX;
            int y = entity.CellY;
            var stressInfo = settings.GetStressInfoFor(entity);
            field[x, y] += stressInfo.Power;
            int ds = (int)stressInfo.Distance;

            for (int d = 1; d <= ds; d++)
            {
                int x0 = x - d;
                int x1 = x + d;
                int y0 = y - d;
                int y1 = y + d;
                int val = (int)(stressInfo.Power / d);
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
    }
}
