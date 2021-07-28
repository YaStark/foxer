using System;
using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu;
using foxer.Render;
using foxer.Render.Menu;

namespace foxer.Pages.Game.Menu
{
    public class BuildingOptionsMenuItem : MenuItemBase
    {
        private static readonly byte[] _iconGrassWall = Properties.Resources.icon_grass_wall;
        private static readonly byte[] _iconGrassWindow = Properties.Resources.icon_grass_window;
        private static readonly byte[] _iconGrassDoor = Properties.Resources.icon_grass_door;

        private readonly MenuButtonRenderer[] _buttons;

        private readonly GameUIViewModel _viewModel;

        public BuildingOptionsMenuItem(GameUIViewModel viewModel, int buttonsCount)
        {
            _viewModel = viewModel;
            _buttons = new MenuButtonRenderer[buttonsCount];
            for (int i = 0; i < buttonsCount; i++)
            {
                _buttons[i] = new MenuButtonRenderer();
            }
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            base.OnRender(canvas, args);

            if(_viewModel?.ActiveEntity?.Hand is IBuildableWall wall) // todo
            {
                if(args.Bounds.Width > args.Bounds.Height)
                {
                    var cells = GeomUtils.SplitOnCells(args.Bounds, _buttons.Length, 1);
                    RenderButton(canvas, _buttons[0], cells[0, 0], _iconGrassWall);
                    RenderButton(canvas, _buttons[1], cells[1, 0], _iconGrassWindow);
                    RenderButton(canvas, _buttons[2], cells[2, 0], _iconGrassDoor);
                }
                else
                {
                    var cells = GeomUtils.SplitOnCells(args.Bounds, 1, _buttons.Length);
                    RenderButton(canvas, _buttons[0], cells[0, 0], _iconGrassWall);
                    RenderButton(canvas, _buttons[1], cells[0, 1], _iconGrassWindow);
                    RenderButton(canvas, _buttons[2], cells[0, 2], _iconGrassDoor);
                }
            }
        }

        protected override bool OnTouch(PointF pt, MenuItemInfoArgs args)
        {
            if (_viewModel?.ActiveEntity?.Hand is IBuildableWall wall) // todo
            {
                if(args.Bounds.Width > args.Bounds.Height)
                {
                    var cells = GeomUtils.SplitOnCells(args.Bounds, _buttons.Length, 1);
                    return ProcessClick(_buttons[0], pt, cells[0, 0], () => wall.WallKind = WallKind.Wall)
                        || ProcessClick(_buttons[1], pt, cells[1, 0], () => wall.WallKind = WallKind.Window)
                        || ProcessClick(_buttons[2], pt, cells[2, 0], () => wall.WallKind = WallKind.Door);
                }
                else
                {
                    var cells = GeomUtils.SplitOnCells(args.Bounds, 1, _buttons.Length);
                    return ProcessClick(_buttons[0], pt, cells[0, 0], () => wall.WallKind = WallKind.Wall)
                        || ProcessClick(_buttons[1], pt, cells[0, 1], () => wall.WallKind = WallKind.Window)
                        || ProcessClick(_buttons[2], pt, cells[0, 2], () => wall.WallKind = WallKind.Door);
                }
            }

            return base.OnTouch(pt, args);
        }

        private bool ProcessClick(MenuButtonRenderer renderer, PointF pt, RectangleF bounds, Action action)
        {
            if (bounds.Contains(pt))
            {
                action.Invoke();
                StartAnimation(renderer.WaitAfterClick.Coroutine);
                return true;
            }

            return false;
        }

        private void RenderButton(INativeCanvas canvas, MenuButtonRenderer renderer, RectangleF bounds, byte[] icon)
        {
            renderer.Render(
                canvas,
                bounds,
                icon,
                true,
                ActiveAnimation == renderer.WaitAfterClick);
        }
    }
}
