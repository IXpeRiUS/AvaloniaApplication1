using Avalonia.Controls;
using AvaloniaApplication1.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using System.Drawing.Imaging;
using System.Formats.Asn1;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Drawing;
using Avalonia.Input;
using Avalonia.Media;
using OpenTK.Graphics.OpenGL;

namespace AvaloniaApplication1.Views
{
    public partial class MainWindow : Window
    {
        private double zoom = 1;

        public MainWindow()
        {
            InitializeComponent();            
        }

        protected void OnWheel(object sender, PointerWheelEventArgs e)
        {
            var control = sender as LayoutTransformControl;

            if (control == null)
            {
                return;
            }

            if (control.RenderTransform == null)
            {
                control.RenderTransform = new ScaleTransform();
            }

            var scale = control.RenderTransform as ScaleTransform;

            double zoom = e.Delta.Y > 0 ? .2 : -.2;
            scale.ScaleX += zoom;
            scale.ScaleY += zoom;
        }
    }

}