using foxer.Core.Game;
using foxer.Core.ViewModel.Menu;
using System;
using System.Drawing;

namespace foxer.Render.Menu
{
    public abstract class MenuBase : IMenu
    {
        public class GridCellSetup
        {
            private MenuBase _menu;
            private IMenuItem _menuItem;

            private Rectangle? _bounds;
            private Rectangle? _boundsTransponed;

            public GridCellSetup(MenuBase menu, IMenuItem item)
            {
                _menu = menu;
                _menuItem = item;
            }

            public GridCellSetup SetDefaultLayout(int x, int y, int colspan = 1, int rowspan = 1)
            {
                _bounds = new Rectangle(x, y, Math.Max(colspan, 1), Math.Max(rowspan, 1));
                return this;
            }

            public GridCellSetup SetTransponedLayout(int x, int y, int colspan = 1, int rowspan = 1)
            {
                _boundsTransponed = new Rectangle(x, y, Math.Max(colspan, 1), Math.Max(rowspan, 1));
                return this;
            }

            public void End()
            {
                _menu.TryAddItem(_menuItem, _bounds, _boundsTransponed);
            }
        }

        private const float GRID_MIN_CELLS_MARGIN = 0.05f;

        private readonly GridCellInfo[,] _grid1;
        private readonly GridCellInfo[,] _grid2;
        private readonly GameMenuViewModelBase _viewModel;
        private readonly Size _size;
        private readonly Size _transponedSize;

        protected MenuBase(GameMenuViewModelBase viewModel, int gridWidth, int gridHeight)
        {
            _viewModel = viewModel;
            _size = new Size(gridWidth, gridHeight);
            _transponedSize = new Size(gridHeight, gridWidth);
            _grid1 = new GridCellInfo[gridWidth, gridHeight];
            _grid2 = new GridCellInfo[gridHeight, gridWidth];
        }

        protected GridCellSetup BeginCreateCell(IMenuItem item)
        {
            return new GridCellSetup(this, item);
        }

        private void TryAddItem(IMenuItem item, Rectangle? bounds, Rectangle? boundsTransponed)
        {
            if(!bounds.HasValue && !boundsTransponed.HasValue)
            {
                throw new ArgumentException($"Failed to create {item.GetType()} in menu {GetType()}: no bounds");
            }

            Rectangle rect1 = bounds.HasValue ? bounds.Value : boundsTransponed.Value;
            Rectangle rect2 = boundsTransponed.HasValue ? boundsTransponed.Value : bounds.Value;
            _grid1[rect1.X, rect1.Y] = new GridCellInfo(rect1.Width, rect1.Height, item);
            _grid2[rect2.X, rect2.Y] = new GridCellInfo(rect2.Width, rect2.Height, item);
        }
        
        public void Render(INativeCanvas canvas, SizeF size)
        {
            canvas.Save();

            bool transponed = IsTransponed(size);
            var gridSize = transponed ? _transponedSize : _size;
            var grid = transponed ? _grid2 : _grid1;

            float cellSizeW = (size.Width / gridSize.Width - GRID_MIN_CELLS_MARGIN) / (1 + GRID_MIN_CELLS_MARGIN);
            float cellSizeH = (size.Height / gridSize.Height - GRID_MIN_CELLS_MARGIN) / (1 + GRID_MIN_CELLS_MARGIN);
            int cellSize = (int)Math.Min(cellSizeW, cellSizeH);
            float gapX = (size.Width - cellSize * gridSize.Width) / (gridSize.Width - 1);
            float gapY = (size.Height - cellSize * gridSize.Height) / (gridSize.Height - 1);
            Size cellSizeX = new Size(cellSize, cellSize);
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == null)
                    {
                        continue;
                    }

                    var bounds = new RectangleF(
                        i * (cellSize + gapX),
                        j * (cellSize + gapY),
                        (cellSize + gapX) * grid[i, j].ColumnSpan - gapX,
                        (cellSize + gapY) * grid[i, j].RowSpan - gapY);

                    grid[i, j].Item.Render(
                        canvas, 
                        new MenuItemInfoArgs(_viewModel.Stage, bounds, cellSizeX));
                }
            }

            canvas.Restore();
        }

        public virtual bool Touch(PointF pt, SizeF size)
        {
            bool transponed = IsTransponed(size);
            var gridSize = transponed ? _transponedSize : _size;
            var grid = transponed ? _grid2 : _grid1;
            float cellSizeW = (size.Width / gridSize.Width - GRID_MIN_CELLS_MARGIN) / (1 + GRID_MIN_CELLS_MARGIN);
            float cellSizeH = (size.Height / gridSize.Height - GRID_MIN_CELLS_MARGIN) / (1 + GRID_MIN_CELLS_MARGIN);
            int cellSize = (int)Math.Min(cellSizeW, cellSizeH);
            float gapX = (size.Width - cellSize * gridSize.Width) / (gridSize.Width - 1);
            float gapY = (size.Height - cellSize * gridSize.Height) / (gridSize.Height - 1);
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == null)
                    {
                        continue;
                    }

                    var bounds = new RectangleF(
                        i * (cellSize + gapX),
                        j * (cellSize + gapY),
                        (cellSize + gapX) * grid[i, j].ColumnSpan - gapX,
                        (cellSize + gapY) * grid[i, j].RowSpan - gapY);

                    if (bounds.Contains(pt))
                    {
                        return grid[i, j].Item.Touch(
                            pt, 
                            new MenuItemInfoArgs(_viewModel.Stage, bounds, new Size(cellSize, cellSize)));
                    }
                }
            }

            return false;
        }

        public void Update(int delayMs)
        {
            for (int i = 0; i < _grid1.GetLength(0); i++)
            {
                for (int j = 0; j < _grid1.GetLength(1); j++)
                {
                    _grid1[i, j]?.Item.Update(delayMs);
                }
            }
        }
        
        private bool IsTransponed(SizeF size)
        {
            return Math.Sign(_size.Width - _size.Height) != Math.Sign(size.Width - size.Height);
        }
    }
}
