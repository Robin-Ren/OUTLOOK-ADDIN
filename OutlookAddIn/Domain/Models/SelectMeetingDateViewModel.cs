using System;
using System.Collections.Generic;
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

            NavigateToConfirmAppointmentDialogCommand = new RelayCommand(NavigateToConfirmAppointmentControl);
            BackToRoomsCommand = new RelayCommand(BackToRoomsControl);
            SelectTimeslotCommand = new RelayCommand(SelectTimeSlot);

            OnSelectedDateChanged();
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

        private List<BookingDetail> _parentAppointmentDetails;

        public List<BookingDetail> ParentAppointmentDetails
        {
            get
            {
                return _parentAppointmentDetails;
            }
            set
            {
                _parentAppointmentDetails = value;
            }
        }

        private List<BookingDetail> _childAppointmentDetails;
        public List<BookingDetail> ChildAppointmentDetails
        {
            get
            {
                return _childAppointmentDetails;
            }
            set
            {
                _childAppointmentDetails = value;
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
            if (obj is TimeSlot timeslot)
            {
                OnSelectTimeSlot(timeslot);
            }
        }

        private void OnSelectTimeSlot(TimeSlot timeslot)
        {
            if (timeslot.isSelected)
            {
                var selectedTimeslots = GetSelectedTimeSlots();

                foreach (var selectedTimeslot in selectedTimeslots)
                {
                    if (selectedTimeslot.CompareTo(timeslot) >= 0)
                    {
                        selectedTimeslot.isSelected = false;
                    }
                }
            }
            else
            {
                // Check continuance
                var selectedTimeslotsStartEnd = GetSelectedStartEndTimeslots();
                if (selectedTimeslotsStartEnd.Item1 == null ||
                    timeslot.IsAdjacentAfter(selectedTimeslotsStartEnd.Item2) ||
                    selectedTimeslotsStartEnd.Item1.IsAdjacentAfter(timeslot))
                {
                    timeslot.isSelected = !timeslot.isSelected;
                }
            }
        }

        private async void OnSelectedDateChanged()
        {
            if (TimeSlotsOfSelectedDate != null)
                TimeSlotsOfSelectedDate.Clear();
            IsLoading = true;

            ClearSelectedTimeSlots();

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
                ts.status = ts.available
                    ? "Available"
                    : "Inavailable";

                if (ParentAppointmentDetails != null && ParentAppointmentDetails.Count > 0)
                {
                    var foundAptmnt = ParentAppointmentDetails
                        .Find(x => x.facilityBooking.facility.id == this.FacilityID &&
                        x.from.Value == ts.from);
                    if (foundAptmnt != null && foundAptmnt.status.ToUpper() == "APPROVED")
                    {
                        ts.available = false;
                        ts.status = string.Format("Ocuppied by {0}", foundAptmnt.createdBy);
                    }
                }
            }

            if (TimeSlotsOfSelectedDate != null &&
                TimeSlotsOfSelectedDate.Count > 0)
            {
                IsLoading = false;
            }
        }
        #endregion

        public List<TimeSlot> GetSelectedTimeSlots()
        {
            var listTimeSlotsOfSelectedDate = new List<TimeSlot>(_timeSlotsOfSelectedDate);
            var checkedTimeslots = listTimeSlotsOfSelectedDate
                .FindAll(x => x.available
                && x.isSelected);
            checkedTimeslots.Sort(new TimeslotComparer());

            return checkedTimeslots;
        }

        public void ClearSelectedTimeSlots()
        {
            if (_timeSlotsOfSelectedDate == null || _timeSlotsOfSelectedDate.Count == 0)
                return;

            var listTimeSlotsOfSelectedDate = new List<TimeSlot>(_timeSlotsOfSelectedDate);
            var selectedTimeslots = listTimeSlotsOfSelectedDate
                .FindAll(x => x.available
                && x.isSelected);

            if (selectedTimeslots != null && selectedTimeslots.Count > 0)
            {
                foreach (var ts in selectedTimeslots)
                {
                    ts.isSelected = false;
                }
            }
        }

        public Tuple<TimeSlot, TimeSlot> GetSelectedStartEndTimeslots()
        {
            var selectedTimeslots = GetSelectedTimeSlots();

            if (selectedTimeslots != null && selectedTimeslots.Count > 0)
            {
                TimeSlot startTimeSlot = null;
                TimeSlot endTimeSlot = null;
                TimeSlot previousTimeslot = null;

                var last = selectedTimeslots[selectedTimeslots.Count - 1];
                foreach (var timeslot in selectedTimeslots)
                {
                    var dtTimeslot = timeslot.from.ToSingaporeDateTimeFromEpoch();
                    if (previousTimeslot == null)
                    {
                        previousTimeslot = timeslot;
                        startTimeSlot = timeslot;
                        endTimeSlot = timeslot;
                    }
                    else if (timeslot.Equals(last))
                    {
                        endTimeSlot = timeslot;
                    }
                    else
                    {
                        if (timeslot.IsAdjacentAfter(previousTimeslot))
                        {
                            previousTimeslot = timeslot;
                        }
                        else
                        {
                            endTimeSlot = timeslot;

                            break;
                        }
                    }
                }

                return new Tuple<TimeSlot, TimeSlot>(startTimeSlot, endTimeSlot);
            }

            return new Tuple<TimeSlot, TimeSlot>(null, null);
        }
    }
}
