using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace OutlookAddIn.Domain
{
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
        public static event EventHandler AddAppointmentEventHandler;

        /// <summary>
        /// Raises the NagigateToBookings event
        /// </summary>
        protected void OnAddAppointment()
        {
            AddAppointmentEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Raised opening new booking rooms button is pressed.
        /// </summary>
        public static event OpenNewBookingRoomsEventHandler OpenNewBookingRooms;

        /// <summary>
        /// Raises the OpenNewBookingRooms event
        /// </summary>
        protected void OnOpenNewBookingRooms()
        {
            OpenNewBookingRooms?.Invoke(this, new EventArgs());
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
            OnOpenNewBookingRooms();
        }
        #endregion
    }
}
