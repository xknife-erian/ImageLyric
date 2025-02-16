﻿using System.Windows;

namespace ImageLyre.UI.Controls.Attaches
{
    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogCanClosedProperty =
            DependencyProperty.RegisterAttached(
                "DialogCanClosed",
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(OnDialogCanClosedChanged));

        private static void OnDialogCanClosedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window && (bool?)e.NewValue == true)
                window.Close();
        }

        public static void SetDialogCanClosed(Window target, bool? value)
        {
            target.SetValue(DialogCanClosedProperty, value);
        }
    }
}
