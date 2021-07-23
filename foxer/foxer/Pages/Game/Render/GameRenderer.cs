using foxer.Core.ViewModel;
using foxer.Pages.Game.Menu;
using foxer.Render;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Pages
{
    public class GameRenderer : IRenderer
    {
        private const float MATH_COS_45 = 0.7071068f;

        private readonly PageGameViewModel _vm;
        private readonly MenuHost _menuHost;

        private readonly List<IGameLayerRenderer> _layers = new List<IGameLayerRenderer>();

        public GameRenderer(PageGameViewModel vm, MenuHost menuHost)
        {
            _vm = vm;
            _menuHost = menuHost;
            var entityRenderer = new GameLayerEntityRenderer(vm);
            _layers.Add(new GameLayerCellsRenderer(vm));
            _layers.Add(entityRenderer);
            _layers.Add(new GameLayerBuilderRenderer(vm.GameUI, entityRenderer, menuHost));
        }

        public void Draw(INativeCanvas canvas)
        {
            canvas.Save();

            RectangleF viewport = _vm.GetViewPort(canvas.Size);
            var size = canvas.Size;
            var sx = size.Width / viewport.Width;
            var sy = size.Height / viewport.Height;
            var scale = Math.Min(sx, sy);
            canvas.Translate(size.Width / 2, size.Height / 2);
            canvas.Scale(scale, scale);
            canvas.RotateDegrees(45);
            canvas.Translate(-viewport.Width / 2, -viewport.Height / 2);
            canvas.Translate(-viewport.X, -viewport.Y);

            var fieldModel = _vm.GetStageField();
            var center = new PointF(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 2);
            var rBounds = GetRenderableBounds(size, center, viewport, fieldModel.GetLength(0), fieldModel.GetLength(1));

            var cells = new List<Point>();
            for (int i = rBounds.Left; i <= rBounds.Right; i++)
            {
                for (int j = rBounds.Top; j <= rBounds.Bottom; j++)
                {
                    if (CheckCellVisibility(i, j, center, viewport))
                    {
                        cells.Add(new Point(i, j));
                    }
                }
            }

            foreach (var layer in _layers)
            {
                if (layer.Enabled)
                {
                    layer.Render(canvas, cells);
                }
            }

            canvas.Restore();
            _menuHost.Menu.Render(canvas, size);
        }

        public bool Touch(PointF pt, SizeF size)
        {
            if (_menuHost.Menu.Touch(pt, size))
            {
                return true;
            }

            var touchedCell = ConvertTouchToCell(pt, size);
            for (int i = _layers.Count - 1; i >= 0; i--)
            {
                if (_layers[i].Touch(touchedCell.X, touchedCell.Y))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddGameLayer(IGameLayerRenderer renderer)
        {
            _layers.Add(renderer);
        }

        private Point ConvertTouchToCell(PointF pt, SizeF size)
        {
            RectangleF viewport = _vm.GetViewPort(size);
            var scale = Math.Min(size.Width / viewport.Width, size.Height / viewport.Height);
            var x0 = pt.X - size.Width / 2;
            var y0 = pt.Y - size.Height / 2;
            return new Point(
                (int)(MATH_COS_45 * (x0 + y0) / scale + viewport.Width / 2 + viewport.X),
                (int)(MATH_COS_45 * (y0 - x0) / scale + viewport.Height / 2 + viewport.Y));
        }

        private static bool CheckCellVisibility(float i, float j, PointF viewportCenter, RectangleF viewport)
        {
            float i0 = i - viewportCenter.X;
            float j0 = j - viewportCenter.Y;
            return viewport.Contains(
                (i0 - j0) * MATH_COS_45 * 0.7f + viewportCenter.X,
                (i0 + j0) * MATH_COS_45 * 0.7f + viewportCenter.Y);
        }

        private Rectangle GetRenderableBounds(SizeF canvasSize, PointF viewportCenter, RectangleF viewport, int width, int height)
        {
            var x0 = ConvertTouchToCell(new PointF(0, 0), canvasSize).X;
            var x1 = ConvertTouchToCell(new PointF(canvasSize.Width, canvasSize.Height), canvasSize).X;
            var y0 = ConvertTouchToCell(new PointF(canvasSize.Width, 0), canvasSize).Y;
            var y1 = ConvertTouchToCell(new PointF(0, canvasSize.Height), canvasSize).Y;
            return Rectangle.FromLTRB(
                Math.Min(width - 1, Math.Max(0, Math.Min(x0, x1))),
                Math.Min(height - 1, Math.Max(0, Math.Min(y0, y1))),
                Math.Min(width - 1, Math.Max(0, Math.Max(x0, x1))),
                Math.Min(height - 1, Math.Max(0, Math.Max(y0, y1))));
        }
    }
}