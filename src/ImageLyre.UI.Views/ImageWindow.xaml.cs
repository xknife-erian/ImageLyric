﻿using System.Windows;
using ImageLyre.UI.ViewModels;
using OpenCvSharp;
using Window = System.Windows.Window;

namespace ImageLyre.UI.Views;

/// <summary>
///     ImageWindow.xaml 的交互逻辑
/// </summary>
public partial class ImageWindow : Window
{
    public ImageWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var vm = (ImageViewModel) DataContext;
        Top = vm.Top;
        Left = vm.Left;
        AdjustWindowSize(vm.BmpMat);
    }

    /// <summary>
    /// 调整窗体大小
    /// </summary>
    /// <param name="bmp"></param>
    private void AdjustWindowSize(Mat? bmp)
    {
        var bmpHeight = 0;
        if (bmp != null)
            bmpHeight = bmp.Height;
        Height = bmpHeight + _StatusBar_.Height;
        if (bmp != null)
            Width = bmp.Width;
    }
}