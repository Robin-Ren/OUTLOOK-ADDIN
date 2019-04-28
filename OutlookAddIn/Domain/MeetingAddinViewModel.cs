using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using Word = Microsoft.Office.Interop.Word;

namespace OutlookAddIn.Domain
{
    public class MeetingAddinViewModel : ABaseViewModel
    {
        #region Private Members
        private DateTime _dateStart;
        private DateTime _dateEnd;
        private string _mailBody;
        private DateTime? _futureValidatingDate;
        private string _selectedMeetingRoom;
        private string _selectedRecipient;
        private ObservableCollection<string> _recipients;
        #endregion


        #region Public Properties
        public DateTime DateStart
        {
            get { return _dateStart; }
            set
            {
                _dateStart = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set
            {
                _dateEnd = value;
                OnPropertyChanged();
            }
        }

        public string MailBody
        {
            get { return _mailBody; }
            set
            {
                _mailBody = value;
                OnPropertyChanged();
            }
        }

        public DateTime? FutureValidatingDate
        {
            get { return _futureValidatingDate; }
            set
            {
                _futureValidatingDate = value;
                OnPropertyChanged();
            }
        }

        public string SelectedMeetingRoom
        {
            get { return _selectedMeetingRoom; }
            set
            {
                this.MutateVerbose(ref _selectedMeetingRoom, value, RaisePropertyChanged());
            }
        }

        public string SelectedRecipient
        {
            get { return _selectedRecipient; }
            set
            {
                this.MutateVerbose(ref _selectedRecipient, value, RaisePropertyChanged());
            }
        }

        public ObservableCollection<string> Recipients
        {
            get
            { return _recipients; }
            set
            {
                _recipients = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets and sets the list of meeting rooms.
        public List<string> AvailableMeetingRooms { get; set; }
        #endregion Properties

        #region Commands
        public ICommand AcceptCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand AddRecipientCommand { get; set; }
        #endregion

        public MeetingAddinViewModel()
        {
            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;
            SelectedMeetingRoom = null;

            AvailableMeetingRooms = new List<string>
            {
                "Room 1 - West",
                "Room 2 - East",
                "Room 3 - North",
                "Room 4 - South"
            };
            MailBody = "This is mail body.";

            Recipients = new ObservableCollection<string>
            {
                "rore@163.com",
                "joe.wang@alibaba.com"
            };

            // Commands
            AcceptCommand = new RelayCommand(OnAccept);
            CancelCommand = new RelayCommand(OnCancel);
            AddRecipientCommand = new RelayCommand(OnAddRecipient);
        }

        #region Command Implementations
        private void OnAccept(object obj)
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
                    meetingItem = (Outlook.AppointmentItem)inspector.Application.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olAppointmentItem);
                    return;
                }

                meetingItem.Location = string.IsNullOrEmpty(SelectedMeetingRoom)
                    ? string.Empty
                    : SelectedMeetingRoom.ToString();

                meetingItem.MeetingStatus = Outlook.OlMeetingStatus.olMeeting;
                meetingItem.Body = MailBody;

                // Meeting dates
                meetingItem.Start = this.DateStart;
                meetingItem.End = this.DateEnd;
                meetingItem.Subject = "Time slot to discuss Outlook Addin";
                if (Recipients != null)
                {
                    foreach (var rec in Recipients)
                        meetingItem.Recipients.Add(rec);
                }

                object missing = System.Reflection.Missing.Value;
            }
            catch (Exception ex)
            {
                WindowDialogViewModel dialog = new WindowDialogViewModel()
                {
                    ShowPositiveButton = true,
                    PositiveButtonName = "Ok",
                    Title = "Error Occurred",
                    Text = ex.Message
                };
                _winDialogService.ShowDialog(dialog);
            }
        }

        private void OnCancel(object obj)
        {

        }

        private void OnAddRecipient(object obj)
        {
            try
            {
                if (obj is TextBox txtNewAttendee
                    && !string.IsNullOrWhiteSpace(txtNewAttendee.Text))
                {
                    Recipients.Add(txtNewAttendee.Text);
                    OnPropertyChanged("Recipients");
                }
            }
            catch (Exception e)
            {
            }
        }

        public void OnAddRecipient(string attendee)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(attendee))
                {
                    Recipients.Add(attendee);
                    OnPropertyChanged("Recipients");
                }
            }
            catch (Exception e)
            {
            }
        }
        #endregion Command Implementations
    }
}
