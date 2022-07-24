﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ImageLad.ImageEngine;
using ImageLad.ImageEngine.Analyze;
using ImageLad.UI.Controls;
using ImageLad.UI.ViewModels;
using ImageLad.UI.Views.Utils;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmDialogs;

namespace WPFSample.Panes;

public class HistogramSampleViewModel : ObservableRecipient
{
    private readonly IDialogService _dialogService;
    private ObservableCollection<UiGrayHistogram> _histograms = new();
    private string _info;
    private ObservableCollection<Bitmap> _photos = new();

    public HistogramSampleViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }


    public ObservableCollection<Bitmap> Photos
    {
        get => _photos;
        set => SetProperty(ref _photos, value);
    }

    /// <summary>
    ///     直方图数据
    /// </summary>
    public ObservableCollection<UiGrayHistogram> Histograms
    {
        get => _histograms;
        set => SetProperty(ref _histograms, value);
    }

    public string Info
    {
        get => _info;
        set => SetProperty(ref _info, value);
    }

    public ICommand ReadPhotosCommand => new RelayCommand(ReadPhotos);

    private void ReadPhotos()
    {
        var pvm = new ProgressViewModel();
        Task.Factory.StartNew(() =>
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Assets\HistogramSample\");
            if (!Directory.Exists(path))
                return;
            var images = Directory.GetFiles(path);
            var i = 0;
            pvm.Minimum = 0;
            pvm.Maximum = images.Length * 2;
            foreach (var file in images)
            {
                i++;
                var target = new ImageTarget(file);
                target.Open();
                var bmp = target.Bitmap;
                UI.RunAsync(() =>
                {
                    Photos.Add(bmp);
                    pvm.Current = i;
                    pvm.Message = $"{i} - {new FileInfo(file).Name}";
                });
            }

            var sw = new Stopwatch(); 
            while (i < images.Length * 2)
            {
                sw.Restart();
                var histogram = GrayHistogram.Compute(Photos[i - images.Length], GrayFormula.Weighted);
                sw.Stop();
                var e = sw.ElapsedMilliseconds;
                UI.RunAsync(() =>
                {
                    Histograms.Add(new UiGrayHistogram
                    {
                        Histogram = histogram,
                        Color = Color.CadetBlue,
                        Visible = true
                    });
                    pvm.Current = i;
                    pvm.Message = $"{i} - {e}";
                });
                i++;
            }

            pvm.Finish();
        });
        _dialogService.ShowDialog(this, pvm);
    }
}