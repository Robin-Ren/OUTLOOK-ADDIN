using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace OutlookAddin.Domain
{
    public delegate void AddAppointmentEventHandler(object sender, AddAppointmentEventArgs e);

    public class AppointmentViewModel : ABaseViewModel
    {
        #region Properties

        private string subject;
        public string Subject 
        {
            get { return subject; }
            set
            {
                if (subject != value)
                {
                    subject = value;
                    OnPropertyChanged();
                }
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime endTime;
        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string body;
        public string Body
        {
            get { return body; }
            set
            {
                if (body != value)
                {
                    body = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DateFormatted
        {
            get { return StartTime.ToLongDateString(); }
        }

        public string TimeSlot
        {
            get
            {
                return string.Format("{0} - {1}",
                StartTime.ToShortTimeString(),
                EndTime.ToShortTimeString());
            }
        }

        public override string ToString()
        {
            return Subject;
        }
        #endregion

        #region Commands
        public ICommand AddAppointmentCommand { get; set; }
        public ICommand OpenNewBookingRoomsDialogCommand { get; set; }
        #endregion

        #region Events
        public AppointmentViewModel()
        {
            AddAppointmentCommand = new RelayCommand(OpenAddAppointmentDialog);
            OpenNewBookingRoomsDialogCommand = new RelayCommand(OpenNewBookingRoomsControl);
        }

        /// <summary>
        /// Raised Add new appointment button is pressed.
        /// </summary>
        public static event AddAppointmentEventHandler AddAppointmentEvent;

        /// <summary>
        /// Raises the NagigateToBookings event
        /// </summary>
        protected void OnAddAppointment()
        {
            AddAppointmentEvent?.Invoke(this, new AddAppointmentEventArgs());
        }

        /// <summary>
        /// Raised opening new booking rooms button is pressed.
        /// </summary>
        public static event OpenNewBookingRoomsEventHandler NavigateToBookingRooms;

        /// <summary>
        /// Raises the OnNavigateToBookingRooms event
        /// </summary>
        protected void OnNavigateToBookingRooms()
        {
            NavigateToBookingRooms?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Command Implementations
        private void OpenAddAppointmentDialog(object obj)
        {
            OnAddAppointment();

        }

        private void OpenNewBookingRoomsControl(object obj)
        {
            // Just raise the OnOpenNewBookingRooms Event
            OnNavigateToBookingRooms();
        }
        #endregion
    }
}
