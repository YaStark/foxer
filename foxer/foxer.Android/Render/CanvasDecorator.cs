using System;
using System.Collections.Generic;
using System.Drawing;
using Android.Graphics;
using Android.Text;
using foxer.Render;

namespace foxer.Droid.Render
{
    internal class CanvasDecorator : INativeCanvas
    {
        private static readonly Dictionary<byte[], Bitmap> _bitmapCache = new Dictionary<byte[], Bitmap>();
        private static readonly Typeface _typeface = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, "fonts/Luna.ttf");

        private Canvas _canvas;

        public SizeF Size => new SizeF(_canvas.Width, _canvas.Height);

        public CanvasDecorator(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void Save()
        {
            _canvas.Save();
        }

        public void Restore()
        {
            _canvas.Restore();
        }

        public void Scale(float x, float y)
        {
            _canvas.Scale(x, y);
        }

        public void Translate(float x, float y)
        {
            _canvas.Translate(x, y);
        }

        public void RotateDegrees(float angle)
        {
            _canvas.Rotate(angle);
        }

        public void DrawRectangle(RectangleF rect, System.Drawing.Color color)
        {
            var paint = new Paint()
            {
                Color = Android.Graphics.Color.Argb(color.A, color.R, color.G, color.B)
            };
            _canvas.DrawRect(new RectF(rect.Left, rect.Top, rect.Right, rect.Bottom), paint);
            paint.Dispose();

        }

        public void DrawImage(byte[] bitmap, RectangleF bounds)
        {
            Bitmap bmp = _bitmapCache.ContainsKey(bitmap)
                ? _bitmapCache[bitmap]
                : _bitmapCache[bitmap] = BitmapFactory.DecodeByteArray(bitmap, 0, bitmap.Length, null);

            _canvas.DrawBitmap(
                bmp, 
                null, 
                new RectF(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom), 
                null);
        }

        public void DrawImage(byte[] bitmap, RectangleF sourceRelative, RectangleF bounds)
        {
            Bitmap bmp = _bitmapCache.ContainsKey(bitmap)
                ? _bitmapCache[bitmap]
                : _bitmapCache[bitmap] = BitmapFactory.DecodeByteArray(bitmap, 0, bitmap.Length, null);

            var source = new Rect(
                (int)(sourceRelative.Left * bmp.Width),
                (int)(sourceRelative.Top * bmp.Height),
                (int)(sourceRelative.Right * bmp.Width),
                (int)(sourceRelative.Bottom * bmp.Height));

            _canvas.DrawBitmap(
                bmp,
                source,
                new RectF(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom),
                null);
        }

        public void DrawText(string text, RectangleF bounds, System.Drawing.Color color)
        {
            var paint = new TextPaint
            {
                Color = Android.Graphics.Color.Argb(color.A, color.R, color.G, color.B),
                TextSize = 1,
                TextAlign = Paint.Align.Center,
            };
            paint.SetTypeface(_typeface);

            var rect = new Rect();
            paint.GetTextBounds(text, 0, text.Length, rect);
            var scale = Math.Min(bounds.Width / rect.Width(), bounds.Height / rect.Height());
            paint.TextSize = scale;

            _canvas.DrawText(
                text,
                (bounds.Left + bounds.Right) / 2,
                (bounds.Top + bounds.Bottom + scale) / 2,
                paint);
        }
    }
}