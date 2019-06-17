using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace OutlookAddin.Domain
{
    public delegate void AddAppointmentEventHandler(object sender, SaveBookingRequestArgs e);
    public delegate void BackToSelectDateEventHandler(object sender);

    public class AppointmentViewModel : ABaseViewModel
    {
        #region Properties
        private Facility _selectedFacility;
        public Facility SelectedFacility
        {
            get { return _selectedFacility; }
            set
            {
                if (_selectedFacility != value)
                {
                    _selectedFacility = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedFacilityName
        {
            get
            {
                if (SelectedFacility == null)
                    return string.Empty;
                return SelectedFacility.name;
            }
        }

        private string remarks;
        public string Remarks 
        {
            get { return remarks; }
            set
            {
                if (remarks != value)
                {
                    remarks = value;
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
            return Remarks;
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Commands
        public ICommand AddAppointmentCommand { get; set; }
        public ICommand BackToSelectDateDialogCommand { get; set; }
        #endregion

        #region Events
        public AppointmentViewModel()
        {
            AddAppointmentCommand = new RelayCommand(AddAppointment);
            BackToSelectDateDialogCommand = new RelayCommand(BackToSelectDateControl);
        }

        /// <summary>
        /// Raised Add new appointment button is pressed.
        /// </summary>
        public static event AddAppointmentEventHandler AddAppointmentEvent;

        /// <summary>
        /// Raises the NavigateToBookings event
        /// </summary>
        protected void OnAddAppointment()
        {
            AddAppointmentEvent?.Invoke(
                this,
                new SaveBookingRequestArgs
                {
                    requestRemark = this.remarks
                });
        }

        /// <summary>
        /// Raised opening new booking rooms button is pressed.
        /// </summary>
        public static event BackToSelectDateEventHandler BackToSelectDate;

        /// <summary>
        /// Raises the OnNavigateToBookingRooms event
        /// </summary>
        protected void OnBackToSelectDate()
        {
            BackToSelectDate?.Invoke(this);
        }
        #endregion

        #region Command Implementations
        private void AddAppointment(object obj)
        {
            OnAddAppointment();
        }

        private void BackToSelectDateControl(object obj)
        {
            // Just raise the OnOpenNewBookingRooms Event
            OnBackToSelectDate();
        }
        #endregion
    }
}
