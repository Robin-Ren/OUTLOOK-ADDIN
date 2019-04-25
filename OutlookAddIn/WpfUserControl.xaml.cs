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

            //var dialogHost = new DialogHost();
            //var binding = new CommandBinding(DialogHost.OpenDialogCommand, dialogHost.OpenDialogHandler, CanOpenDialog);
            //// Register CommandBinding for all windows.
            //CommandManager.RegisterClassCommandBinding(typeof(UserControl), binding);
            //DialogHost.SetDialogOpenedAttached(btnMeetingTo, ToDialogOpenedEventHandler);
            //DialogHost.SetDialogClosingAttached(btnMeetingTo, ToDialogClosingEventHandler);
            //dialogHost.OnApplyTemplate();

            DataContext = new MeetingAddinViewModel();
        }

        private void CanOpenDialog(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the Application object
                Outlook.Application application = Globals.ThisAddIn.Application;

                // Get the active Inspector object and check if is type of MailItem
                Outlook.Inspector inspector = application.ActiveInspector();
                Outlook.AppointmentItem meetingItem = inspector.CurrentItem as Outlook.AppointmentItem;
                if (meetingItem == null)
                {
                    MessageBox.Show("the meeting is null,create a meeting Item");
                    meetingItem = (Outlook.AppointmentItem)inspector.Application.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olAppointmentItem);
                    return;
                }

                //meetingItem.Location = cboRooms.SelectedItem.ToString();

                meetingItem.MeetingStatus = Outlook.OlMeetingStatus.olMeeting;
                meetingItem.Body = "Robin is the best!";

                object missing = System.Reflection.Missing.Value;
                //Microsoft.Office.Interop.Word.Application wordAppl;
                //Word.Document wordDoc;
                //Word.Selection wordSel = null;
                //wordDoc = (Word.Document)inspector.WordEditor;
                //wordAppl = wordDoc.Parent as Word.Application;
                //wordDoc.Activate();
                //String path = "D://en_US.rtf";
                //String fileName = path.ToString();
                //wordSel = (Word.Selection)wordAppl.Selection;
                //wordSel.Range.Delete(ref missing, ref missing);
                //object falseRef = false;
                //wordSel.Range.InsertFile(fileName, ref missing, ref falseRef, ref falseRef, ref falseRef);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Robbin click error!" + ex.ToString());
            }
        }

        public void FromDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            CombinedCalendarFrom.SelectedDate = ((MeetingAddinViewModel)DataContext).DateStart;
            CombinedClockFrom.Time = ((MeetingAddinViewModel)DataContext).TimeStart;
        }

        public void FromDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1"))
            {
                var combined = CombinedCalendarFrom.SelectedDate.Value.AddSeconds(CombinedClockFrom.Time.TimeOfDay.TotalSeconds);
                ((MeetingAddinViewModel)DataContext).TimeStart = combined;
                ((MeetingAddinViewModel)DataContext).DateStart = combined;

                ((MeetingAddinViewModel)DataContext).TimeEnd = combined.AddHours(1);
                ((MeetingAddinViewModel)DataContext).DateEnd = combined.AddHours(1);
            }
        }

        public void ToDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            CombinedCalendarTo.SelectedDate = ((MeetingAddinViewModel)DataContext).DateEnd;
            CombinedClockTo.Time = ((MeetingAddinViewModel)DataContext).TimeEnd;
        }

        public void ToDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1"))
            {
                var combined = CombinedCalendarTo.SelectedDate.Value.AddSeconds(CombinedClockTo.Time.TimeOfDay.TotalSeconds);
                ((MeetingAddinViewModel)DataContext).TimeEnd = combined;
                ((MeetingAddinViewModel)DataContext).DateEnd = combined;
            }
        }

        private void Attendees_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;

            if (!string.IsNullOrWhiteSpace(Attendee.Text))
                AttendeesList.Items.Add(Attendee.Text.Trim());
        }
    }
}
