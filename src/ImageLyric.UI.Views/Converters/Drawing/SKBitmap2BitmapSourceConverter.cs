﻿namespace ImageLyric.UI.Views.Converters.Drawing;

// [ValueConversion(typeof(SKBitmap), typeof(WriteableBitmap))]
// public class SKBitmap2BitmapSourceConverter : IValueConverter
// {
//     public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//     {
//         var bitmap = value as SKBitmap;
//         if (bitmap == null)
//             throw new ArgumentNullException(nameof(value));
//         return bitmap.ToWriteableBitmap();
//     }
//
//     /// <summary>Converts a binding target value to the source binding values.</summary>
//     /// <param name="value">The value that the binding target produces.</param>
//     /// <param name="targetTypes">
//     ///     The array of types to convert to. The array length indicates the number and types of values
//     ///     that are suggested for the method to return.
//     /// </param>
//     /// <param name="parameter">The converter parameter to use.</param>
//     /// <param name="culture">The culture to use in the converter.</param>
//     /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
//     public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
//     {
//         throw new NotSupportedException();
//     }
// }