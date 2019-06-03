using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OutlookAddIn.CustomScheduler.Model;

namespace OutlookAddin.Domain
{
    public delegate void NagigateToBookingsEventHandler(object sender, NagigateToBookingsArgs e);
    public delegate void OpenSchedulerDialogEventHandler(object sender, OpenSchedulerDialogArgs e);

    public class RoomsViewModel : ABaseViewModel
    {
        public RoomsViewModel()
        {
            _facilities = new ObservableCollectionWrapper<Facility>();

            NagigateToBookingsDialogCommand = new RelayCommand(NagigateToBookingsControl);
            OpenSchedulerDialogCommand = new RelayCommand(OpenSchedulerDialog);
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
        public static event NagigateToBookingsEventHandler NagigateToBookingsEvent;

        /// <summary>
        /// Raises the NagigateToBookings event
        /// </summary>
        protected void OnNagigateToBookings()
        {
            NagigateToBookingsEvent?.Invoke(this, new NagigateToBookingsArgs());
        }

        /// <summary>
        /// Raised when a meeting room is selected.
        /// </summary>
        public static event OpenSchedulerDialogEventHandler OpenSchedulerDialogEvent;

        /// <summary>
        /// Raises the OpenSchedulerDialogEventHandler event
        /// </summary>
        protected void OnOpenSchedulerDialog(OpenSchedulerDialogArgs e)
        {
            OpenSchedulerDialogEvent?.Invoke(this, e);
        }
        #endregion

        #region Commands
        public ICommand NagigateToBookingsDialogCommand { get; set; }
        public ICommand OpenSchedulerDialogCommand { get; }
        public ICommand AcceptSchedulerDialogCommand { get; }
        public ICommand CancelSchedulerDialogCommand { get; }
        #endregion

        #region Command Implementations
        private void NagigateToBookingsControl(object obj)
        {
            // Just raise the OnNagigateToBookings Event
            OnNagigateToBookings();
        }

        private void OpenSchedulerDialog(object obj)
        {
            OpenSchedulerDialogArgs e = new OpenSchedulerDialogArgs
            {
                SelectedRoom = obj as Facility
            };

            OnOpenSchedulerDialog(e);
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
