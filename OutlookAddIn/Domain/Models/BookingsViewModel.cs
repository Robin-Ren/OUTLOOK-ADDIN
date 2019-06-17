using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OutlookAddin.Domain
{
    public delegate void OpenNewBookingRoomsEventHandler(object sender);

    public class BookingsViewModel : ABaseViewModel
    {
        public BookingsViewModel()
        {
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
            OpenNewBookingRooms?.Invoke(this);
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

        private Appointments _parentAppointments;

        public Appointments ParentAppointments
        {
            get
            {
                return _parentAppointments;
            }

            set
            {
                _parentAppointments = value;
                OnPropertyChanged();
            }
        }

        private Appointments _childAppointments;

        public Appointments ChildAppointments
        {
            get
            {
                return _childAppointments;
            }

            set
            {
                _childAppointments = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
