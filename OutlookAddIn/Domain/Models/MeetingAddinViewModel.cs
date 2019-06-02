using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using OutlookAddIn;
using OutlookAddIn.CustomScheduler.Model;
using OutlookAddIn.WebAPIClient;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookAddin.Domain
{
    public class MeetingAddinViewModel : ABaseViewModel
    {
        #region Private Members
        private static WebAPIDataAccess apiDataAccess;

        private DateTime _dateStart;
        private DateTime _dateEnd;
        private string _mailBody;
        private DateTime? _futureValidatingDate;
        private RoomEntity _selectedRoom;
        private string _selectedRecipient;
        private ObservableCollection<string> _recipients;
        private Appointments _appointments;
        /// <summary>
        /// The current view model being displayed.
        /// This may not be the selected tab as that tab could have sub views.
        /// </summary>
        private ABaseViewModel _currentViewModel;

        private static BookingsViewModel _bookingViewModel;
        private static LoginViewModel _loginViewModel;
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

        public RoomEntity SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
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

        public Appointments Appointments
        {
            get
            { return _appointments; }
            set
            {
                _appointments = value;
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
            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;
            SelectedRoom = null;

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
        private async void InitializeBookingsViewModel(object sender, NagigateToBookingsArgs e)
        {
            _bookingViewModel = new BookingsViewModel();

            _appointments = await apiDataAccess.GetBookingRecords();
            _bookingViewModel.Appointments = _appointments;

            _bookingViewModel.ClearEventInvocations("OpenNewBookingRooms");
            BookingsViewModel.OpenNewBookingRooms += new OpenNewBookingRoomsEventHandler(OnOpenNewBookingRoomsDialog);

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

                meetingItem.Location = SelectedRoom.RoomName;

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

        private async void OnOpenNewBookingRoomsDialog(object sender, EventArgs e)
        {
            RoomsViewModel roomsModel = new RoomsViewModel();

            roomsModel.AvailableFacilities = await apiDataAccess.GetFacilitiesVenue();

            roomsModel.ClearEventInvocations("NagigateToBookingsEvent");
            roomsModel.ClearEventInvocations("OpenSchedulerDialogEvent");
            RoomsViewModel.NagigateToBookingsEvent += InitializeBookingsViewModel;
            RoomsViewModel.OpenSchedulerDialogEvent += OpenSchedulerDialog;


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

        private void OpenSchedulerDialog(object obj, OpenSchedulerDialogArgs e)
        {
            SchedulerViewModel schedulerViewModel = new SchedulerViewModel(_appointments);

            SchedulerViewModel.NavigateToAddAppointmentEvent += new NavigateToAddAppointmentEventHandler(OnOpenNewAppointmentDialog);

            ((RoomsViewModel)CurrentViewModel).SchedulerContent = new SchedulerControl(schedulerViewModel);
            ((RoomsViewModel)CurrentViewModel).IsSchedulerDialogOpen = true;

            _selectedRoom = e.SelectedRoom;
        }

        private void OnOpenNewAppointmentDialog(object sender, NavigateToAddAppointmentEventArgs e)
        {
            AppointmentViewModel appointmentModel = new AppointmentViewModel();

            appointmentModel.ClearEventInvocations("NavigateToBookingRooms");
            AppointmentViewModel.NavigateToBookingRooms += new OpenNewBookingRoomsEventHandler(OnOpenNewBookingRoomsDialog);

            // Set the rooms as the current view model
            CurrentViewModel = appointmentModel;
        }
        #endregion Command Implementations

    }
}
