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

        public void DrawText(string text, RectangleF bounds, System.Drawing.Color color, HorAlign align)
        {
            var paint = new TextPaint
            {
                Color = Android.Graphics.Color.Argb(color.A, color.R, color.G, color.B),
                TextSize = 1,
                TextAlign = Paint.Align.Center,
            };
         //   paint.SetTypeface(_typeface);

            var rect = new Rect();
            paint.GetTextBounds(text, 0, text.Length, rect);
            paint.TextSize = Math.Min(bounds.Width / rect.Width(), bounds.Height / rect.Height());
            paint.GetTextBounds(text, 0, text.Length, rect);
            var pt = GetTextLocation(bounds, rect, align);
            _canvas.DrawText(text, pt.X, pt.Y, paint);
        }

        public void DrawText(string text, RectangleF bounds, System.Drawing.Color color, float textSize, HorAlign align)
        {
            var paint = new TextPaint
            {
                Color = Android.Graphics.Color.Argb(color.A, color.R, color.G, color.B),
                TextSize = textSize,
                TextAlign = Paint.Align.Center,
            };
         //   paint.SetTypeface(_typeface);
            var rect = new Rect();
            paint.GetTextBounds(text, 0, text.Length, rect);
            if(rect.Width() <= bounds.Width)
            {
                var pt = GetTextLocation(bounds, rect, align);
                _canvas.DrawText(text, pt.X, pt.Y, paint);
                return;
            }

            var widths = new float[text.Length];
            paint.GetTextWidths(text, widths);
            string[] lines = SplitTextByBounds(text, bounds.Width, widths);
            int linesCount = Math.Min((int)(bounds.Height / rect.Height()), lines.Length);
            float gap = Math.Min(textSize / 2, bounds.Height - linesCount * rect.Height());

            float dy = 0;
            for (int i = 0; i < linesCount; i++)
            {
                var lineBounds = new RectangleF(bounds.Left, bounds.Top + dy, bounds.Width, rect.Height());
                paint.GetTextBounds(lines[i], 0, lines[i].Length, rect);
                var linePlace = GetTextLocation(lineBounds, rect, align);
                _canvas.DrawText(lines[i], linePlace.X, linePlace.Y, paint);
                dy += rect.Height() + gap;
            }
        }

        private static System.Drawing.PointF GetTextLocation(RectangleF bounds, Rect textBounds, HorAlign align)
        {
            switch(align)
            {
                case HorAlign.Center:
                default:
                    return new System.Drawing.PointF(
                        bounds.Left + bounds.Width / 2,
                        bounds.Top + bounds.Height / 2 + textBounds.Height() / 2 - textBounds.Bottom);

                case HorAlign.Far:
                    return new System.Drawing.PointF(
                        bounds.Left + textBounds.Right - textBounds.Width() / 2,
                        bounds.Top + bounds.Height / 2 + textBounds.Height() / 2 - textBounds.Bottom);

                case HorAlign.Near:
                    return new System.Drawing.PointF(
                        bounds.Left + textBounds.Left + textBounds.Width() / 2,
                        bounds.Top + bounds.Height / 2 + textBounds.Height() / 2 - textBounds.Bottom);
            }
        }

        private string[] SplitTextByBounds(string text, float proposedWidth, float[] charWidths)
        {
            var breakIndexes = new List<int>();
            breakIndexes.Add(-1);
            int lastSpaceIndex = -1;
            float width = 0;
            float wordWidth = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsLetterOrDigit(text[i]))
                {
                    lastSpaceIndex = i;
                }

                if (width + wordWidth > proposedWidth)
                {
                    if (breakIndexes.Contains(lastSpaceIndex))
                    {
                        // пробелов нет, рубим строчку по текущему символу
                        lastSpaceIndex = i;
                        breakIndexes.Add(i);
                        width = wordWidth;
                        wordWidth = 0;
                    }
                    else
                    {
                        // рубим строчку по последнему разрывному символу
                        breakIndexes.Add(lastSpaceIndex);
                        width = wordWidth;
                    }
                }
                else if (lastSpaceIndex == i)
                {
                    width += wordWidth;
                    wordWidth = charWidths[i];
                }
                else
                {
                    wordWidth += charWidths[i];
                }
            }

            string[] result = new string[breakIndexes.Count];
            for (int i = 0; i < breakIndexes.Count - 1; i++)
            {
                result[i] = text.Substring(breakIndexes[i] + 1, breakIndexes[i + 1] - breakIndexes[i]);
            }

            result[result.Length - 1] = text.Substring(breakIndexes[breakIndexes.Count - 1] + 1);
            return result;
        }
    }
}