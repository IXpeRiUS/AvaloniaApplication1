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
            IList<Device> devices = new List<Device>();

            Random random = new Random();
            public NewCustomDraw(Rect bounds)
            {
                Bounds = bounds;
            
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

                foreach (var device in this.devices)
                {
                    device.Draw(canvas);
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
