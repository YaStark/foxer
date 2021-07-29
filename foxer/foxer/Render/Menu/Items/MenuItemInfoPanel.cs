using foxer.Core.Game.Info;
using foxer.Core.Utils;
using System.Drawing;

namespace foxer.Render.Menu.Items
{
    public class MenuItemInfoPanel : MenuItemBase
    {
        private readonly IItemInfoProviderFactory _infoProviderFactory;

        public MenuItemInfoPanel(IItemInfoProviderFactory infoProviderFactory)
        {
            _infoProviderFactory = infoProviderFactory;
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            base.OnRender(canvas, args);

            RenderBackground(canvas, args.Bounds, args.CellSize);
            var selected = _infoProviderFactory.GetItem();
            var infoProvider = _infoProviderFactory.GetItemInfoProvider();
            if (selected == null ||  infoProvider == null)
            {
                return;
            }

            float textSize = args.CellSize.Height / 3;
            var bounds = GeomUtils.Deflate(args.Bounds, args.CellSize.Width / 5, args.CellSize.Height / 5);
            var boundsHeader = new RectangleF(bounds.Location, new SizeF(bounds.Width, textSize * 2));
            canvas.DrawText(
                infoProvider.GetText(selected, args.Stage), 
                boundsHeader, 
                Color.Black, 
                textSize, 
                HorAlign.Near);
        }
    }
}
