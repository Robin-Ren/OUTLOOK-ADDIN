using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OutlookAddin.Domain
{
    public static class MessageBoxCloser
    {
        public static readonly DependencyProperty MessageBoxResultProperty = DependencyProperty.RegisterAttached(
            name: "MessageBoxResult",
            propertyType: typeof(bool?),
            ownerType: typeof(MessageBoxCloser),
            defaultMetadata: new PropertyMetadata(MessageBoxResultChanged));

        private static void MessageBoxResultChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var msgBox = obj as System.Windows.Window;

            if (msgBox != null)
            {
                msgBox.DialogResult = e.NewValue as bool?;
            }
        }

        public static void SetMessageBoxResult(System.Windows.Window target, bool? value)
        {
            target.SetValue(MessageBoxResultProperty, value);
        }
    }
}
