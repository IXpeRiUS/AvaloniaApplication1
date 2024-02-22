using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Primitives;
using AvaloniaApplication1.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;




namespace AvaloniaApplication1.ViewModels
{

    public partial class MainWindowViewModel : ViewModelBase
    {
       public void DrawRandomRect()
        {
            int countRectangle = 10;
            while (countRectangle > 0)
            {
                var rect = new NewSkia();
                countRectangle--;
            }
        }

    }
}
