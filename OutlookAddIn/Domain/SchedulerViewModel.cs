using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OutlookAddIn.CustomScheduler.Model;

namespace OutlookAddIn.Domain
{
    public class SchedulerViewModel : ABaseViewModel
    {
        public SchedulerViewModel(Appointments appointments)
        {
            Appointments = appointments;

            AddAppointmentCommand = new RelayCommand(AddAppointment);
        }

        #region Properties
        private Appointments _appointments;

        public Appointments Appointments
        {
            get { return _appointments; }
            set
            {
                if (_appointments == value) return;
                _appointments = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Raised Add new appointment button is pressed.
        /// </summary>
        public static event EventHandler AddAppointmentEventHandler;

        /// <summary>
        /// Raises the NagigateToBookings event
        /// </summary>
        protected void OnAddAppointment()
        {
            AddAppointmentEventHandler?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Commands
        public ICommand AddAppointmentCommand { get; set; }

        #endregion

        #region Command Implementations

        private void AddAppointment(object obj)
        {
            OnAddAppointment();
        }
        #endregion
    }
}
