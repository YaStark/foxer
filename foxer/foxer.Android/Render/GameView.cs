using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Views;
using foxer.Render;
using Xamarin.Forms.Platform.Android;
using static Android.Views.View;

namespace foxer.Droid.Render
{
    public class GameView : Android.Views.View, INativeView, IOnTouchListener
    {
        private readonly IRenderer _renderer;

        public Xamarin.Forms.View View => this.ToView();

        public SizeF Size => new SizeF(Width, Height);

        public GameView(IRenderer renderer, Context context) 
            : base(context)
        {
            _renderer = renderer;
            SetOnTouchListener(this);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            _renderer.Draw(new CanvasDecorator(canvas));
        }

        public void RequestRedraw()
        {
            Invalidate();
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if(e.Action == MotionEventActions.Down)
            {
                _renderer.Touch(
                    new System.Drawing.PointF(e.GetX(), e.GetY()),
                    new SizeF(Width, Height));
            }

            return true;
        }
    }
}