using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OutlookAddIn.CustomScheduler.Model;

namespace OutlookAddIn.Domain
{
    public delegate void OpenNewBookingRoomsEventHandler(object sender, EventArgs e);

    public class BookingsViewModel : ABaseViewModel
    {
        public BookingsViewModel()
        {
            _appointments = new Appointments();
            OpenNewBookingRoomsDialogCommand = new RelayCommand(OpenNewBookingRoomsControl);
        }

        #region Events
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

        #region Commands
        public ICommand OpenNewBookingRoomsDialogCommand { get; set; }
        #endregion

        #region Command Implementations
        private void OpenNewBookingRoomsControl(object obj)
        {
            // Just raise the OnOpenNewBookingRooms Event
            OnOpenNewBookingRooms();
        }
        #endregion

        #region Properties

        private Appointments _appointments;

        public Appointments Appointments
        {
            get
            {
                return _appointments;
            }

            set
            {
                _appointments = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
