using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public partial class NewSkia : Control
    {
        public NewSkia()
        {
        }

        class NewCustomDraw : ICustomDrawOperation
        {
            Random random = new Random();
            public NewCustomDraw(Rect bounds)
            {
                Bounds = bounds;
            }
            public Rect Bounds { get; }
            public void Dispose()
            {
                throw new NotImplementedException();
            }
            public bool Equals(ICustomDrawOperation other) => false;
            public bool HitTest(Point p) => false;
           

            public void Render(ImmediateDrawingContext context)
            {
                var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();

                using var lease = leaseFeature.Lease();
                var canvas = lease.SkCanvas;
                canvas.Save();
                //разноцветный квадратик!
                var color = new SKColor((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                using (var rect = new SKPaint())
                {
                    rect.Color = color;
                    rect.Style = SKPaintStyle.Stroke;
                    rect.StrokeWidth = 10;
                    canvas.DrawRect(random.Next(10, 700), random.Next(10, 500), 100, 100, rect);
                }
                canvas.Restore();
            }
        }

        public override void Render(DrawingContext context)
        {
            context.Custom(new NewCustomDraw(new Rect(0, 0, Bounds.Width, Bounds.Height)));
            Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
        }
    }
}
