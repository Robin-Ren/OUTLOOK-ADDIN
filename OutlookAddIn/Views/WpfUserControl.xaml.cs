using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OutlookAddIn
{
    /// <summary>
    /// WpfUserControl.xaml
    /// </summary>
    public partial class WpfUserControl : UserControl
    {
        public WpfUserControl()
        {
            InitializeComponent();
        }

        private void CanOpenDialog(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
