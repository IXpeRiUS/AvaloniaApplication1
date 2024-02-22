using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        IList<Device> devices = new List<Device>();
        
        public NewSkia()
        {
            var random = new Random();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    var device = new Device()
                    {
                        Position = new Point(10 + i * 10, 10 + j * 10),
                        Color = new SKColor((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255))
                    };

                    devices.Add(device);
                }
            }
        }

        class NewCustomDraw : ICustomDrawOperation
        {
            IEnumerable<Device> objectsToDraw;

            public NewCustomDraw(Rect bounds, IEnumerable<Device> objectsToDraw)
            {
                this.objectsToDraw = objectsToDraw;
                Bounds = bounds;                          
            }

            public Rect Bounds { get; }
            Rect ICustomDrawOperation.Bounds { get; }

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

                foreach (var device in this.objectsToDraw)
                {
                    device.Draw(canvas);
                }

                canvas.Restore();
            }
        }

        public override void Render(DrawingContext context)
        {
            context.Custom(new NewCustomDraw(new Rect(0, 0, Bounds.Width, Bounds.Height), this.devices));
            Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
        }
    }
}
