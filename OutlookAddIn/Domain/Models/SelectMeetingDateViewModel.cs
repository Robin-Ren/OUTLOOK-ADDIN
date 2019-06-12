using System;
using System.Windows.Input;
using OutlookAddIn.WebAPIClient;

namespace OutlookAddin.Domain
{
    public delegate void NavigateToConfirmAppointmentEventHandler(object sender, NavigateToConfirmAppointmentArgs e);
    public delegate void BackToRoomsEventHandler(object sender);

    public class SelectMeetingDateViewModel : ABaseViewModel
    {
        public SelectMeetingDateViewModel(int facilityId)
        {
            // Initialize WebAPI Client
            apiDataAccess = new WebAPIDataAccess();
            FacilityID = facilityId;
            SelectedDate = DateTime.Now;

            NavigateToConfirmAppointmentDialogCommand = new RelayCommand(NavigateToConfirmAppointmentControl);
            BackToRoomsCommand = new RelayCommand(BackToRoomsControl);
            SelectTimeslotCommand = new RelayCommand(SelectTimeSlot);
        }

        #region Properties
        private static WebAPIDataAccess apiDataAccess;

        private int FacilityID { get; set; }

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value;
                OnPropertyChanged();

                OnSelectedDateChanged();
            }
        }

        private bool _isLoaded;

        public bool IsLoading
        {
            get { return _isLoaded; }
            set
            {
                if (_isLoaded == value) return;
                _isLoaded = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollectionWrapper<TimeSlot> _timeSlotsOfSelectedDate;

        public ObservableCollectionWrapper<TimeSlot> TimeSlotsOfSelectedDate
        {
            get { return _timeSlotsOfSelectedDate; }
            set
            {
                if (_timeSlotsOfSelectedDate == value) return;
                _timeSlotsOfSelectedDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Raised opening Timeslots page button is pressed.
        /// </summary>
        public static event NavigateToConfirmAppointmentEventHandler NavigateToConfirmAppointmentEvent;

        /// <summary>
        /// Raises the OnNavigateToConfirmAppointment event
        /// </summary>
        protected void OnNavigateToConfirmAppointment()
        {
            NavigateToConfirmAppointmentEvent?.Invoke(this, new NavigateToConfirmAppointmentArgs());
        }

        /// <summary>
        /// Raised opening Timeslots page button is pressed.
        /// </summary>
        public static event BackToRoomsEventHandler BackToRoomsEvent;

        /// <summary>
        /// Raises the NavigateToTimeslotsEvent event
        /// </summary>
        protected void OnBackToRooms()
        {
            BackToRoomsEvent?.Invoke(this);
        }
        #endregion

        #region Commands
        public ICommand NavigateToConfirmAppointmentDialogCommand { get; set; }
        public ICommand BackToRoomsCommand { get; set; }
        public ICommand SelectTimeslotCommand { get; set; }
        #endregion

        #region Command Implementations
        private void NavigateToConfirmAppointmentControl(object obj)
        {
            OnNavigateToConfirmAppointment();
        }

        private void BackToRoomsControl(object obj)
        {
            OnBackToRooms();
        }

        private void SelectTimeSlot(object obj)
        {
            var timeslot = obj as TimeSlot;

            if (timeslot != null)
            {
                timeslot.isSelected = !timeslot.isSelected;
            }
        }

        private async void OnSelectedDateChanged()
        {
            if (TimeSlotsOfSelectedDate != null)
                TimeSlotsOfSelectedDate.Clear();
            IsLoading = true;

            //Get all timeslots by selected date
            long fromTicks = new DateTime(
                SelectedDate.Year,
                SelectedDate.Month,
                SelectedDate.Day,
                00, 00, 00)
                .ToSingaporeEpochTime();
            long toTicks = fromTicks + 1;

            TimeSlotsOfSelectedDate = await apiDataAccess.GetTimeSlots(
                FacilityID,
                fromTicks,
                toTicks);
            foreach (var ts in TimeSlotsOfSelectedDate)
            {
                ts.status = ts.from.ToSingaporeDateTimeFromEpoch().ToString("yyyy/MM/dd hh:mm:ss");
            }

            if (TimeSlotsOfSelectedDate != null &&
                TimeSlotsOfSelectedDate.Count > 0)
            {
                IsLoading = false;
            }
        }
        #endregion
    }
}
