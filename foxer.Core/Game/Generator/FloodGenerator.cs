using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game.Generator
{
    public class FloodGenerator
    {
        private readonly float _avgFloodCount;
        private readonly IFloodGeneratorUser _floodGeneratorUser;

        public FloodGenerator(int avgFloodCount, IFloodGeneratorUser floodGeneratorUser)
        {
            _avgFloodCount = avgFloodCount;
            _floodGeneratorUser = floodGeneratorUser;
        }

        public virtual float GetFloodProbability(int count)
        {
            return _avgFloodCount / count / 2;
        }

        public void Generate(Stage stage, Random rnd)
        {
            int[,] field = new int[stage.Width, stage.Height];
            var countAll = new RefValue<int>(0);
            foreach(Point pt in _floodGeneratorUser.GetNextEpicenter(stage, rnd, countAll))
            {
                if(!_floodGeneratorUser.CanPropagateTo(stage, rnd, pt.X, pt.Y))
                {
                    continue;
                }

                Stack<Point> stack = new Stack<Point>();
                field[pt.X, pt.Y] = 1;
                stack.Push(pt);

                int count = 1;
                while(stack.Any())
                {
                    var pt0 = stack.Pop();
                    TryPush(stage, rnd, stack, pt0.X + 1, pt0.Y, field, ref count);
                    TryPush(stage, rnd, stack, pt0.X - 1, pt0.Y, field, ref count);
                    TryPush(stage, rnd, stack, pt0.X, pt0.Y + 1, field, ref count);
                    TryPush(stage, rnd, stack, pt0.X, pt0.Y - 1, field, ref count);
                }
                
                countAll.Value += count;
            }

            for (int i = 0; i < stage.Width; i++)
            {
                for (int j = 0; j < stage.Height; j++)
                {
                    if(field[i, j] != 0)
                    {
                        int neighbours = GetFieldValue(field, i - 1, j, 0)
                            + GetFieldValue(field, i + 1, j, 0)
                            + GetFieldValue(field, i, j - 1, 0)
                            + GetFieldValue(field, i, j + 1, 0);
                        var args = new CreateCellEventArgs(stage.Cells[i, j], i, j, rnd, neighbours);
                        stage.Cells[i, j] = _floodGeneratorUser.CreateCell(args);
                    }
                }
            }
        }

        private int GetFieldValue(int[,] field, int x, int y, int defaultValue)
        {
            if (x < 0 || y < 0 || x >= field.GetLength(0) || y >= field.GetLength(1))
            {
                return defaultValue;
            }

            return field[x, y];
        }

        private void TryPush(Stage stage, Random rnd, Stack<Point> stack, int x, int y, int[,] field, ref int count)
        {
            if(!stage.CheckArrayBounds(x, y)
                || field[x, y] != 0
                || !_floodGeneratorUser.CanPropagateTo(stage, rnd, x, y))
            {
                return;
            }

            if(rnd.NextDouble() < GetFloodProbability(count))
            {
                field[x, y] = 1;
                stack.Push(new Point(x, y));
                count++;
            }
        }

        public static IEnumerable<Point> GetEpicenters(int iterations, RefValue<int> count, Func<Point> rndPointFactory)
        {
            int value = 0;
            while (iterations > 0)
            {
                while (count.Value == value)
                {
                    yield return rndPointFactory();
                }

                value = count.Value;
                iterations--;
            }
        }
    }
}
