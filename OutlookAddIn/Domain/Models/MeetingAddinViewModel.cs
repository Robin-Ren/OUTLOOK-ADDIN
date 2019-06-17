using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using OutlookAddIn;
using OutlookAddIn.WebAPIClient;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookAddin.Domain
{
    public class MeetingAddinViewModel : ABaseViewModel
    {
        #region Private Members
        private static WebAPIDataAccess apiDataAccess;

        private DateTime _selectedDate;
        private string _remarks;
        private Facility _selectedFacility;
        private string _selectedRecipient;
        private List<TimeSlot> _selectedTimeslots;
        private Appointments _parentAppointments;
        private Appointments _childAppointments;
        private static List<BookingDetail> _parentAppointmentDetails;
        private static List<BookingDetail> _childAppointmentDetails;
        /// <summary>
        /// The current view model being displayed.
        /// This may not be the selected tab as that tab could have sub views.
        /// </summary>
        private ABaseViewModel _currentViewModel;

        private static BookingsViewModel _bookingViewModel;
        private static LoginViewModel _loginViewModel;
        private static AppointmentViewModel _appointmentViewModel;
        #endregion

        #region Public Properties
        /// <summary>
        /// The current view model.
        /// </summary>
        public ABaseViewModel CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        public string Remarks
        {
            get { return _remarks; }
            set
            {
                _remarks = value;
                OnPropertyChanged();
            }
        }

        public Facility SelectedFacility
        {
            get { return _selectedFacility; }
            set
            {
                _selectedFacility = value;
                OnPropertyChanged();
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

        public List<TimeSlot> SelectedTimeslots
        {
            get
            { return _selectedTimeslots; }
            set
            {
                _selectedTimeslots = value;
                OnPropertyChanged();
            }
        }

        public Appointments ParentAppointments
        {
            get
            { return _parentAppointments; }
            set
            {
                _parentAppointments = value;
                OnPropertyChanged();
            }
        }

        public Appointments ChildAppointments
        {
            get
            { return _childAppointments; }
            set
            {
                _childAppointments = value;
                OnPropertyChanged();
            }
        }
        #endregion Properties

        #region Commands
        public ICommand AcceptCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        public MeetingAddinViewModel()
        {
            SelectedDate = DateTime.Now;
            SelectedFacility = null;

            // Initialize login view model
            if (_loginViewModel == null)
            {
                InitializeLoginViewModel(null, null);
            }

            // Commands
            AcceptCommand = new RelayCommand(OnAccept);
            CancelCommand = new RelayCommand(OnCancel);

            // Initialize WebAPI Client
            apiDataAccess = new WebAPIDataAccess();
        }

        /// <summary>
        /// Initialize Bookings view model object
        /// </summary>
        private async void InitializeBookingsViewModel(object sender, BackToBookingsArgs e)
        {
            if (_bookingViewModel == null)
                _bookingViewModel = new BookingsViewModel();

            _parentAppointmentDetails = await apiDataAccess.GetBookingRecords(true);
            _parentAppointments = Utils.ConvertBookingDetailsToAppointments(_parentAppointmentDetails);
            _bookingViewModel.ParentAppointments = _parentAppointments;

           _childAppointmentDetails = await apiDataAccess.GetBookingRecords(false);
            _childAppointments = Utils.ConvertBookingDetailsToAppointments(_childAppointmentDetails);
            _bookingViewModel.ChildAppointments = _childAppointments;

            _bookingViewModel.ClearEventInvocations("OpenNewBookingRooms");
            BookingsViewModel.OpenNewBookingRooms += new OpenNewBookingRoomsEventHandler(OnOpenRoomsDialog);

            CurrentViewModel = _bookingViewModel;
        }

        /// <summary>
        /// Initialize Login view model object
        /// </summary>
        private void InitializeLoginViewModel(object sender, EventArgs e)
        {
            _loginViewModel = new LoginViewModel();
            _loginViewModel.ClearEventInvocations("DoLogin");
            LoginViewModel.DoLogin += new LoginEventHandler(OnDoLogin);
            CurrentViewModel = _loginViewModel;
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

                meetingItem.Location = SelectedFacility.name;

                meetingItem.MeetingStatus = Outlook.OlMeetingStatus.olMeeting;
                meetingItem.Body = Remarks;

                // Meeting dates
                //meetingItem.Start = this.SelectedDate;
                //meetingItem.End = this.DateEnd;
                //meetingItem.Subject = "Time slot to discuss Outlook Addin";

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

        private async void OnOpenRoomsDialog(object sender)
        {
            RoomsViewModel roomsModel = new RoomsViewModel();

            roomsModel.AvailableFacilities = await apiDataAccess.GetFacilitiesVenue();

            roomsModel.ClearEventInvocations("BackToBookingsEvent");
            roomsModel.ClearEventInvocations("NavigateToSelectDateDialogEvent");
            RoomsViewModel.BackToBookingsEvent += InitializeBookingsViewModel;
            RoomsViewModel.NavigateToSelectDateDialogEvent += NavigateToSelectDateDialog;

            // Set the rooms as the current view model
            CurrentViewModel = roomsModel;
        }

        private async void OnDoLogin(object sender, LoginEventArgs e)
        {
            _loginViewModel.IsSucceeded = await apiDataAccess.DoLogin(e);

            if (_loginViewModel.IsSucceeded)
            {
                _loginViewModel.LoginMessage = string.Empty;
                // Initialize booking view model
                if (_bookingViewModel == null)
                {
                    InitializeBookingsViewModel(null, null);
                }
            }
            else
            {
                _loginViewModel.LoginMessage = GlobalConstants.LoginFailedMessage;
            }
        }

        private void NavigateToSelectDateDialog(object obj, NavigateToSelectDateDialogArgs e)
        {
            if (e != null && e.SelectedFacility != null)
                _selectedFacility = e.SelectedFacility;

            SelectMeetingDateViewModel selectDateViewModel = new SelectMeetingDateViewModel(_selectedFacility.id);

            if (this.SelectedDate != default(DateTime))
            {
                selectDateViewModel.SelectedDate = this.SelectedDate;
            }
            else
            {
                selectDateViewModel.SelectedDate = DateTime.Now;
            }

            selectDateViewModel.ParentAppointmentDetails = _parentAppointmentDetails;
            selectDateViewModel.ChildAppointmentDetails = _childAppointmentDetails;

            selectDateViewModel.ClearEventInvocations("BackToBookingsEvent");
            selectDateViewModel.ClearEventInvocations("NavigateToConfirmAppointmentEvent");
            SelectMeetingDateViewModel.BackToRoomsEvent += new BackToRoomsEventHandler(OnOpenRoomsDialog);
            SelectMeetingDateViewModel.NavigateToConfirmAppointmentEvent += NavigateToConfirmAppointmentDialog;

            selectDateViewModel.ClearSelectedTimeSlots();
            // Set the select date view model as the current view model
            CurrentViewModel = selectDateViewModel;
        }

        private void NavigateToConfirmAppointmentDialog(object obj, NavigateToConfirmAppointmentArgs e)
        {
            SelectMeetingDateViewModel selectDateModel = CurrentViewModel as SelectMeetingDateViewModel;
            this._selectedTimeslots = selectDateModel.GetSelectedTimeSlots();
            if (_selectedTimeslots == null || _selectedTimeslots.Count == 0)
                return;

            this._selectedDate = selectDateModel.SelectedDate;

            if(_appointmentViewModel == null)
                _appointmentViewModel = new AppointmentViewModel();
            _appointmentViewModel.SelectedFacility = _selectedFacility;
            _appointmentViewModel.Remarks = string.Empty;
            var selectedEnds = selectDateModel.GetSelectedStartEndTimeslots();
            if (selectedEnds.Item1 != null)
            {
                _appointmentViewModel.StartTime = selectedEnds.Item1.from.ToSingaporeDateTimeFromEpoch();
            }
            if (selectedEnds.Item2 != null)
            {
                _appointmentViewModel.EndTime = selectedEnds.Item2.to.ToSingaporeDateTimeFromEpoch();
            }
            _appointmentViewModel.ErrorMessage = string.Empty;

            _appointmentViewModel.ClearEventInvocations("AddAppointmentEvent");
            _appointmentViewModel.ClearEventInvocations("BackToSelectDate");
            AppointmentViewModel.AddAppointmentEvent += new AddAppointmentEventHandler(OnAddAppointment);
            AppointmentViewModel.BackToSelectDate += new BackToSelectDateEventHandler(BackToSelectDate);

            // Set the select date view model as the current view model
            CurrentViewModel = _appointmentViewModel;
        }
        private async void OnAddAppointment(object sender, SaveBookingRequestArgs e)
        {
            e.facility = new SaveBookingParamFacility
            {
                id = _selectedFacility.id
            };
            e.bookingDetails = new List<SaveBookingParamBookingDetail>
            {
                 new SaveBookingParamBookingDetail
                {
                    selectedDate = _selectedDate.ToSingaporeEpochTime(),
                    fromTimeSlotConfigid = _selectedTimeslots[0].timeSlotConfigId,
                    toTimeSlotConfigid = _selectedTimeslots[_selectedTimeslots.Count - 1].timeSlotConfigId
                }
            };

            var errorMessage = await apiDataAccess.SaveBookingRequest(e);
            if (string.IsNullOrEmpty(errorMessage))
            {
                // Go to main view
                InitializeBookingsViewModel(null, null);
            }
            else
            {
                _appointmentViewModel.ErrorMessage = errorMessage;
            }
        }

        private void BackToSelectDate(object sender)
        {
            NavigateToSelectDateDialog(null, null);
        }
        #endregion Command Implementations

    }
}
