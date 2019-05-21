using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OutlookAddIn.CustomScheduler.Model;

namespace OutlookAddIn.Domain
{
    public class RoomsViewModel : ABaseViewModel
    {
        public RoomsViewModel()
        {
            _rooms = new ObservableCollectionWrapper<Room>();

            NagigateToBookingsDialogCommand = new RelayCommand(NagigateToBookingsControl);
            OpenSchedulerDialogCommand = new RelayCommand(OpenSchedulerDialog);
            AcceptSchedulerDialogCommand = new RelayCommand(AcceptSchedulerDialog);
            CancelSchedulerDialogCommand = new RelayCommand(CancelSchedulerDialog);
        }

        #region Properties
        private bool _isSchedulerDialogOpen;
        private object _SchedulerContent;
        private ObservableCollectionWrapper<Room> _rooms;

        public ObservableCollectionWrapper<Room> AvailableRooms
        {
            get
            {
                return _rooms;
            }

            set
            {
                _rooms = value;
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
        public static event EventHandler NagigateToBookings;

        /// <summary>
        /// Raises the NagigateToBookings event
        /// </summary>
        protected void OnNagigateToBookings()
        {
            NagigateToBookings?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Raised Add new appointment button is pressed.
        /// </summary>
        public static event EventHandler OpenSchedulerDialogEventHandler;

        /// <summary>
        /// Raises the OpenSchedulerDialogEventHandler event
        /// </summary>
        protected void OnOpenSchedulerDialog()
        {
            OpenSchedulerDialogEventHandler?.Invoke(this, new EventArgs());
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
            OnOpenSchedulerDialog();
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
