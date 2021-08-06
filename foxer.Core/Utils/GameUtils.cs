using foxer.Core.Game;
using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace foxer.Core.Utils
{
    public static class GameUtils
    {
        /// <summary>
        /// Order: left - top - right - bottom
        /// </summary>
        public static Point[] Nearest4(this Point source)
        {
            return new[] {
                new Point(source.X - 1, source.Y),
                new Point(source.X, source.Y - 1),
                new Point(source.X + 1, source.Y),
                new Point(source.X, source.Y + 1)
            };
        }

        public static EntityCoroutineDelegate DelegateCoroutine(Action<EntityCoroutineArgs> action)
        {
            return (arg) =>
            {
                if(!arg.CancellationToken.IsCancellationRequested)
                {
                    action.Invoke(arg);
                }

                return Enumerable.Empty<EntityAnimation>();
            };
        }

        public static float GetZIndexForWalls(int rotation)
        {
            switch (rotation)
            {
                case 0:
                case 270:
                    return 0.5f;

                case 90:
                case 180:
                    return -0.5f;

                default:
                    return 0;
            }
        }

        public static EntityCoroutineDelegate EnsureCoroutine(Func<EntityCoroutineArgs, bool> action)
        {
            return (arg) =>
            {
                if(!arg.CancellationToken.IsCancellationRequested
                    && !action.Invoke(arg))
                {
                    arg.CancellationToken.Cancel();
                }

                return Enumerable.Empty<EntityAnimation>();
            };
        }

        public static EntityCoroutineDelegate ConditionalCoroutine(
            Func<EntityCoroutineArgs, bool> condition,
            EntityCoroutineDelegate coroutine)
        {
            return (arg) =>
            {
                if (!arg.CancellationToken.IsCancellationRequested
                    && condition.Invoke(arg))
                {
                    return coroutine(arg);
                }

                return Enumerable.Empty<EntityAnimation>();
            };
        }

        public static void CreateDroppedLootItem(this Stage stage, EntityBase entity)
        {
            var loot = stage.GetLoot(entity);
            if(loot != null)
            {
                stage.AddEntity(new DroppedItemEntity(entity.Cell, loot));
            }
        }

        public static Point[] GetPathToItem(Stage stage, EntityBase walker, EntityBase target, int maxDistance)
        {
            // todo set distance
            var targetPlatform = stage.GetPlatform(target);
            var cells = target.Cell.Nearest4()
                // transit from target cell to nearest4 cells
                .Select(pt => new WalkCell(pt, stage.GetPlatformOnTransit(walker, target.Cell, pt, targetPlatform)))
                .Where(cell => cell.Platform != null)
                .ToArray();

            if (!cells.Any())
            {
                return null;
            }

            using (var builder = new WalkBuilderWithMultipleTargets(stage, walker, null, null, cells, maxDistance))
            {
                return builder.GetShortestPath();
            }
        }

        public static T Uniform<T>(IList<T> collection, int seed)
        {
            return collection[Math.Abs(seed) % collection.Count];
        }

        public static List<TResult> OfType<TSource, TResult>(this TSource[,] array)
            where TSource : class
            where TResult : TSource
        {
            var result = new List<TResult>();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] is TResult item)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        public static IEnumerable<Point> EnumerateAllPointsInRangeL1(int width, int height, Point start, int range)
        {
            int x0 = Math.Max(0, start.X - range);
            int x1 = Math.Min(width - 1, start.X + range);
            int y0 = Math.Max(0, start.Y - range);
            int y1 = Math.Min(height - 1, start.Y + range);

            for (int x = x0; x <= x1; x++)
            {
                for (int y = y0; y <= y1; y++)
                {
                    yield return new Point(x, y);
                }
            }
        }
    }
}
