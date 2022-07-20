﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Input;
using ImageLad.ImageEngine;
using ImageLad.ImageEngine.Analyze;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace WPFSample
{
    public class MainViewModel : ObservableRecipient
    {
        private readonly string[] _images;
        private int _currentImage =0;

        public MainViewModel()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"assets\");
            if (!Directory.Exists(path))
                return;
            _images = Directory.GetFiles(path);

            ReadImage();
        }

        private void ReadImage()
        {
            var file = _images[_currentImage];

            var target = new ImageTarget(file);
            target.Open();
            var bmp = target.Bitmap;

            var histogram = GrayHistogram.Compute(bmp, GrayFormula.Weighted);
            Histogram = histogram;

            var fileInfo = new FileInfo(file);
            Info = $"{fileInfo.Name.ToUpper()}, {fileInfo.Length / 1000}k, {histogram}";
            Image = bmp;
        }

        #region 为界面准备的可被绑定的属性
        
        private GrayHistogram _histogram;
        private string _info;
        private Bitmap _image;

        /// <summary>
        /// 直方图数据
        /// </summary>
        public GrayHistogram Histogram
        {
            get => _histogram;
            set => SetProperty(ref _histogram, value);
        }

        public string Info
        {
            get => _info;
            set => SetProperty(ref _info, value);
        }

        public Bitmap Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        #endregion

        public ICommand OpenLastImageCommand => new RelayCommand(LastImage);

        public ICommand OpenNextImageCommand => new RelayCommand(NextImage);

        private void LastImage()
        {
            if (_currentImage == 0)
                _currentImage = _images.Length - 1;
            else
                _currentImage--;
            ReadImage();
        }   
        private void NextImage()
        {
            if (_currentImage == _images.Length - 1)
                _currentImage = 0;
            else
                _currentImage++;
            ReadImage();
        }
    }
}