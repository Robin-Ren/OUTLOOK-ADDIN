using System.Windows.Input;

namespace OutlookAddin.Domain
{
    public delegate void BackToBookingsEventHandler(object sender, BackToBookingsArgs e);
    public delegate void NavigateToSelectDateDialogEventHandler(object sender, NavigateToSelectDateDialogArgs e);

    public class RoomsViewModel : ABaseViewModel
    {
        public RoomsViewModel()
        {
            _facilities = new ObservableCollectionWrapper<Facility>();

            BackToBookingsDialogCommand = new RelayCommand(BackToBookingsControl);
            NavigateToSelectDateDialogCommand = new RelayCommand(NavigateToSelectDateDialog);
            AcceptSchedulerDialogCommand = new RelayCommand(AcceptSchedulerDialog);
            CancelSchedulerDialogCommand = new RelayCommand(CancelSchedulerDialog);
        }

        #region Properties
        private bool _isSchedulerDialogOpen;
        private object _SchedulerContent;
        private ObservableCollectionWrapper<Facility> _facilities;

        public ObservableCollectionWrapper<Facility> AvailableFacilities
        {
            get
            {
                return _facilities;
            }

            set
            {
                _facilities = value;
                OnPropertyChanged();
            }
        }

        public bool IsSchedulerDialogOpen
        {
            get { return _isSchedulerDialogOpen; }
            set
            {
                if (_isSchedulerDialogOpen == value) return;
                _isSchedulerDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public object SchedulerContent
        {
            get { return _SchedulerContent; }
            set
            {
                if (_SchedulerContent == value) return;
                _SchedulerContent = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// Raised opening new booking rooms button is pressed.
        /// </summary>
        public static event BackToBookingsEventHandler BackToBookingsEvent;

        /// <summary>
        /// Raises the NavigateToBookings event
        /// </summary>
        protected void OnBackToBookings()
        {
            BackToBookingsEvent?.Invoke(this, new BackToBookingsArgs());
        }

        /// <summary>
        /// Raised when a meeting room is selected.
        /// </summary>
        public static event NavigateToSelectDateDialogEventHandler NavigateToSelectDateDialogEvent;

        /// <summary>
        /// Raises the OpenSchedulerDialogEventHandler event
        /// </summary>
        protected void OnNavigateToSelectDateDialog(NavigateToSelectDateDialogArgs e)
        {
            NavigateToSelectDateDialogEvent?.Invoke(this, e);
        }
        #endregion

        #region Commands
        public ICommand BackToBookingsDialogCommand { get; set; }
        public ICommand NavigateToSelectDateDialogCommand { get; }
        public ICommand AcceptSchedulerDialogCommand { get; }
        public ICommand CancelSchedulerDialogCommand { get; }
        #endregion

        #region Command Implementations
        private void BackToBookingsControl(object obj)
        {
            OnBackToBookings();
        }

        private void NavigateToSelectDateDialog(object obj)
        {
            NavigateToSelectDateDialogArgs e = new NavigateToSelectDateDialogArgs
            {
                SelectedFacility = obj as Facility
            };

            OnNavigateToSelectDateDialog(e);
        }

        private void CancelSchedulerDialog(object obj)
        {
            IsSchedulerDialogOpen = false;
        }

        private void AcceptSchedulerDialog(object obj)
        {

        }

        #endregion
    }
}
