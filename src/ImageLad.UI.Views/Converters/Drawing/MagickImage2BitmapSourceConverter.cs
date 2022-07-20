﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using ImageMagick;

namespace ImageLad.UI.Views.Converters.Drawing;

[ValueConversion(typeof(MagickImage), typeof(WriteableBitmap))]
public class MagickImage2BitmapSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var bitmap = value as MagickImage;
        if (bitmap == null)
            throw new ArgumentNullException(nameof(value));
        return null;//
    }

    /// <summary>Converts a binding target value to the source binding values.</summary>
    /// <param name="value">The value that the binding target produces.</param>
    /// <param name="targetTypes">
    ///     The array of types to convert to. The array length indicates the number and types of values
    ///     that are suggested for the method to return.
    /// </param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
    public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}