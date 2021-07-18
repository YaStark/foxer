using foxer.Pages;
using System.Drawing;

namespace foxer.Render
{
    public interface IRenderer : ITouchController
    {
        void AddGameLayer(IGameLayerRenderer renderer);
        void Draw(INativeCanvas canvas);
    }
}
