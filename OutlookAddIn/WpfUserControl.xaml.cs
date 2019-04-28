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
using OutlookAddIn.Domain;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using Word = Microsoft.Office.Interop.Word;

namespace OutlookAddIn
{
    /// <summary>
    /// WpfUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class WpfUserControl : UserControl
    {
        public WpfUserControl()
        {
            InitializeComponent();

            DataContext = new MeetingAddinViewModel();
        }

        private void CanOpenDialog(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void FromDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            CombinedCalendarFrom.SelectedDate = ((MeetingAddinViewModel)DataContext).DateStart.AddSeconds(-((MeetingAddinViewModel)DataContext).DateStart.TimeOfDay.TotalSeconds);
            CombinedClockFrom.Time = ((MeetingAddinViewModel)DataContext).DateStart;
        }

        public void FromDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1"))
            {
                var combined = CombinedCalendarFrom.SelectedDate.Value.AddSeconds(CombinedClockFrom.Time.TimeOfDay.TotalSeconds);
                ((MeetingAddinViewModel)DataContext).DateStart = combined;

                ((MeetingAddinViewModel)DataContext).DateEnd = combined.AddHours(1);
            }
        }

        public void ToDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            CombinedCalendarTo.SelectedDate = ((MeetingAddinViewModel)DataContext).DateEnd.AddSeconds(-((MeetingAddinViewModel)DataContext).DateEnd.TimeOfDay.TotalSeconds);
            CombinedClockTo.Time = ((MeetingAddinViewModel)DataContext).DateEnd;
        }

        public void ToDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1"))
            {
                var combined = CombinedCalendarTo.SelectedDate.Value.AddSeconds(CombinedClockTo.Time.TimeOfDay.TotalSeconds);
                ((MeetingAddinViewModel)DataContext).DateEnd = combined;
            }
        }

        private void Attendees_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            try
            {
                if (eventArgs.Parameter is TextBox txtNewAttendee
                    && !string.IsNullOrWhiteSpace(txtNewAttendee.Text))
                {
                    ((MeetingAddinViewModel)DataContext).OnAddRecipient(txtNewAttendee.Text);
                }
            }
            catch (Exception e)
            {
            }

            return;
        }
    }
}
