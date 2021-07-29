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

        public static Func<EntityCoroutineArgs, IEnumerable<EntityAnimation>> DelegateCoroutine(Action<EntityCoroutineArgs> action)
        {
            return (arg) =>
            {
                action.Invoke(arg);
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

        public static Func<EntityCoroutineArgs, IEnumerable<EntityAnimation>> EnsureCoroutine(Func<EntityCoroutineArgs, bool> action)
        {
            return (arg) =>
            {
                if(!action.Invoke(arg))
                {
                    arg.CancellationToken.Cancel();
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
    }
}
