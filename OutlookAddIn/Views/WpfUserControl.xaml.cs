using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using OutlookAddin.Domain;
using OutlookAddIn.CustomScheduler;
using OutlookAddIn.CustomScheduler.Model;
using OutlookAddIn.WebAPIClient;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using Word = Microsoft.Office.Interop.Word;

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
            //WebAPIDataAccess apiDataAccess = new WebAPIDataAccess();
            //var appointments = await apiDataAccess.GetBookingRecords();

            //SetCurrentValue(CustomScheduler.Controls.Calendar.AppointmentsProperty, new Appointments());
        }
    }
}
