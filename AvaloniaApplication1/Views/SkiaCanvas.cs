using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace AvaloniaApplication1.Views
{
    public partial class SkiaCanvas : Control
    {
        private readonly GlyphRun _noSkia;
        //public int BorderObject;


        public SkiaCanvas()
        {
            ClipToBounds = true;
            var text = "jhgfjkhfgjhflj";
            ushort[] glyphs = text.Select(ch => Typeface.Default.GlyphTypeface.GetGlyph(ch)).ToArray();
            _noSkia = new GlyphRun(Typeface.Default.GlyphTypeface, 120, text.AsMemory(), glyphs);
        }

        class CustomDrawOp : ICustomDrawOperation
        {
            private readonly IImmutableGlyphRunReference _noSkia;

            public CustomDrawOp(Rect bounds, GlyphRun noSkia)
            {
                _noSkia = noSkia.TryCreateImmutableGlyphRunReference();
                Bounds = bounds;
            }

            public void Dispose()
            {
                // No-op
            }

            public Rect Bounds { get; }
            public bool HitTest(Point p) => false;
            public bool Equals(ICustomDrawOperation other) => false;
            static Stopwatch St = Stopwatch.StartNew();




            public void Render(ImmediateDrawingContext context)
            {
                var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
                if (leaseFeature == null)
                    context.DrawGlyphRun(Brushes.Black, _noSkia);
                else
                {

                    using var lease = leaseFeature.Lease();
                    var canvas = lease.SkCanvas;
                    canvas.Save();
                    // create the first shader
                    var colors = new SKColor[] {
                        new SKColor(0, 255, 255),
                        new SKColor(255, 0, 255),
                        new SKColor(255, 255, 0),
                        new SKColor(0, 255, 255)
                    };
                    var sx = Animate(100, 2, 10);
                    var sy = Animate(1000, 5, 15);

                    var lightPosition = new SKPoint(
                    (float)(180 + Math.Cos(St.Elapsed.TotalSeconds) * Bounds.Width / 40),
                    (float)(180 + Math.Sin(St.Elapsed.TotalSeconds) * Bounds.Height / 40));

                    using (var sweep =
                    SKShader.CreateSweepGradient(new SKPoint((int)Bounds.Width / 2, (int)Bounds.Height / 2), colors,
                        null))

                    using (var turbulence = SKShader.CreatePerlinNoiseFractalNoise(0.05f, 0.05f, 4, 0))
                    using (var shader = SKShader.CreateCompose(sweep, turbulence, SKBlendMode.SrcATop))
                    using (var blur = SKImageFilter.CreateBlur(Animate(100, 2, 10), Animate(100, 5, 15)))
                    using (var paint = new SKPaint
                    {
                        Shader = shader,
                        ImageFilter = blur
                    })
                        canvas.DrawPaint(paint);

                    using (var pseudoLight = SKShader.CreateRadialGradient(
                        lightPosition,
                        (float)(Bounds.Width / 3),
                        new[] {
                            new SKColor(255, 200, 200, 100),
                            SKColors.Transparent,
                            new SKColor(40,40,40, 220),
                            new SKColor(255,0,0, (byte)Animate(1, 10,22)) },
                        new float[] { 0.3f, 0.3f, 0.5f, 1 },
                        SKShaderTileMode.Decal))

                    using (var pseudoLight2 = SKShader.CreateSweepGradient(
                    lightPosition,

                    new[] {
                            new SKColor(255, 200, 200, 100),
                            SKColors.Transparent,
                            new SKColor(40,40,40, 220),
                            new SKColor(20,20,20, (byte)Animate(1, 10,22)) }))
                    //new float[] { 0.3f, 0.3f, 0.8f, 1 },
                    //SKShaderTileMode.Decal))


                    using (var paint = new SKPaint
                    {
                        Shader = pseudoLight
                    })
                        canvas.DrawPaint(paint);


                    using (var sunbeam = new SKPaint())
                    {
                        sunbeam.Color = SKColors.Orange;
                        sunbeam.IsAntialias = true;
                        sunbeam.StrokeWidth = 10;
                        canvas.DrawLine(180, 180, 180, 20, sunbeam);
                        canvas.DrawLine(180, 180, 180, 360, sunbeam);
                        canvas.DrawLine(180, 180, 20, 180, sunbeam);
                        canvas.DrawLine(180, 180, 360, 180, sunbeam);
                        canvas.DrawLine(180, 180, 90, 90, sunbeam);
                        canvas.DrawLine(180, 180, 360, 90, sunbeam);
                        canvas.DrawLine(180, 180, 90, 360, sunbeam);
                        canvas.DrawLine(180, 180, 300, 300, sunbeam);
                    }

                    using (var sun = new SKPaint())
                    {
                        sun.Color = SKColors.Yellow;
                        sun.IsAntialias = true;
                        sun.Style = SKPaintStyle.Fill;
                        canvas.DrawCircle(180, 180, 80, sun);
                    }
                    using (var suncorona = new SKPaint())
                    {
                        suncorona.Color = SKColors.Orange;
                        suncorona.IsAntialias = true;
                        suncorona.Style = SKPaintStyle.Stroke;
                        suncorona.StrokeWidth = 10;
                        canvas.DrawCircle(180, 180, 80, suncorona);
                    }

                    canvas.Restore();

                }
            }
            static int Animate(int d, int from, int to)
            {
                var ms = (int)(St.ElapsedMilliseconds / d);
                var diff = to - from;
                var range = diff * 2;
                var v = ms % range;
                if (v > diff)
                    v = range - v;
                var rv = v + from;
                if (rv < from || rv > to)
                    throw new Exception("WTF");
                return rv;
            }
        }



        public override void Render(DrawingContext context)
        {
            context.Custom(new CustomDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), _noSkia));
            Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
        }
    }
}