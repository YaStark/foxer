using System;
using System.Drawing;

namespace foxer.Core.Game
{
    public class WalkBuilderFieldFactory
    {
        private const int FIELD_DEPTH = 5;

        private readonly WalkBuilderCell[,][] _field;

        public WalkBuilderFieldFactory(int width, int height)
        {
            _field = new WalkBuilderCell[width, height][];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _field[i, j] = new WalkBuilderCell[FIELD_DEPTH];
                    for (int k = 0; k < FIELD_DEPTH; k++)
                    {
                        _field[i, j][k] = new WalkBuilderCell(new Point(i, j), 0);
                    }
                }
            }
        }

        public WalkBuilderCell[,][] Get(Type entityType)
        {
            return _field;
        }

        public void Update(Stage stage)
        {

        }
    }
}
