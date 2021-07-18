using foxer.Render;
using foxer.Droid.Render;
using Android.Content;

namespace foxer.Droid
{
    public class AndroidViewFactory : INativeViewFactory
    {
        private Context _context;

        public AndroidViewFactory(Context context)
        {
            _context = context;
        }

        public INativeView CreateView(IRenderer renderer)
        {
            return new GameView(renderer, _context);
        }
    }
}