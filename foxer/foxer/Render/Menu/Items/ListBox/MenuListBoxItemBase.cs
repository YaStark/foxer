using foxer.Core.Game;
using foxer.Core.Utils;
using System.Drawing;

namespace foxer.Render.Menu.Items
{
    public abstract class MenuListBoxItemBase : MenuItemBase
    {
        private static readonly byte[] _backgroundNormal = Properties.Resources.list_item_normal;
        private static readonly byte[] _backgroundSelected = Properties.Resources.list_item_selected;
        private static readonly byte[] _backgroundDisabled = Properties.Resources.list_item_disabled;

        private static readonly RectangleF[,] _spriteMask = GeomUtils.CreateSpriteMask3x3(0.3f, 0.4f, 0.3f, 0.4f);

        public virtual bool GetVisible(Stage stage)
        {
            return true;
        }

        public virtual bool GetEnabled(Stage stage)
        {
            return true;
        }

        public virtual bool GetSelected(Stage stage)
        {
            return true;
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            base.OnRender(canvas, args);
            if(GetVisible(args.Stage))
            {
                RenderSprite3x3(canvas, GetBackgroundImage(args.Stage), _spriteMask, args.Bounds, args.CellSize);
            }
        }

        protected byte[] GetBackgroundImage(Stage stage)
        {
            if (!GetEnabled(stage))
            {
                return _backgroundDisabled;
            }

            return GetSelected(stage) 
                ? _backgroundSelected 
                : _backgroundNormal;
        }
    }
}
