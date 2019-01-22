using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace generative
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 1600;
            int height = 1200;
            int pointCount = 24;
            int iterations = 2000;

            var bitmap = new Bitmap(width, height);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillRectangle(Brushes.Black, 0, 0, width, height);

                var p = new Pen(Color.FromArgb(13, 255, 255, 255));

                var random = new Random();
                var points = Enumerable.Range(0, pointCount).Select(i => new PointF((float)random.NextDouble() * width, (float)random.NextDouble() * height)).ToArray();

                for (int it = 0; it < iterations; it++)
                {
                    foreach (var (i, x0, y0) in points.Select((x, i) => (i, x.X, x.Y)))
                    {
                        var (x1, y1) = points[(i + 1) % pointCount];
                        var dx = x1 - x0;
                        var dy = y1 - y0;
                        var d = (float)Math.Sqrt(dx * dx + dy * dy);

                        points[i] = new PointF(x0 + dx / d, y0 + dy / d);
                        g.DrawCircle(p, x0, y0, d);
                    }
                }
            }

            bitmap.Save("out.png", ImageFormat.Png);
        }
    }

    static class Extensions
    {
        public static void Deconstruct(this PointF p, out float x, out float y)
        {
            x = p.X;
            y = p.Y;
        }

        public static void DrawCircle(this Graphics g, Pen pen, float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius, radius + radius, radius + radius);
        }
    }
}
