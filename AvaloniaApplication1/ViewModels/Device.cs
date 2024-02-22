using Avalonia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AvaloniaApplication1.ViewModels
{
    public class Device
    {
        public SKColor Color { get; set; }

        public Point Position { get; set; }
        
        public void Draw(SKCanvas canvas)
        {            
            using (var rect = new SKPaint())
            {
                rect.Color = this.Color;
                rect.Style = SKPaintStyle.Stroke;
                rect.StrokeWidth = 1;
                canvas.DrawRect((float)this.Position.X, (float)this.Position.Y, 5, 5, rect);
            }
        }
    }
}
