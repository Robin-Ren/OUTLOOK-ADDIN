using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OutlookAddIn.CustomScheduler.Controls;
using OutlookAddIn.CustomScheduler.Model;

namespace OutlookAddin.Domain
{
    public delegate void NavigateToAddAppointmentEventHandler(object sender, NavigateToAddAppointmentEventArgs e);

    public class SchedulerViewModel : ABaseViewModel
    {
        public SchedulerViewModel(Appointments appointments)
        {
            Appointments = appointments;

            OpenAddAppointmentDialogCommand = new RelayCommand(NavigateToAddAppointment);
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
        public static event NavigateToAddAppointmentEventHandler NavigateToAddAppointmentEvent;

        /// <summary>
        /// Raises the NagigateToBookings event
        /// </summary>
        protected void OnNavigateToAddAppointment(NavigateToAddAppointmentEventArgs args)
        {
            NavigateToAddAppointmentEvent?.Invoke(this, args);
        }
        #endregion

        #region Commands
        public ICommand OpenAddAppointmentDialogCommand { get; set; }

        #endregion

        #region Command Implementations

        private void NavigateToAddAppointment(object obj)
        {
            var calendarObj = obj as Calendar;
            var timeslots = calendarObj.GetSelectedTimeslots();

            if (timeslots == null ||
               !timeslots.Item1.HasValue ||
               !timeslots.Item2.HasValue)
                return;

            var args = new NavigateToAddAppointmentEventArgs
            {
                StartTimeslot = timeslots.Item1.Value,
                EndTimeslot = timeslots.Item2.Value
            };
            OnNavigateToAddAppointment(args);
        }
        #endregion
    }
}
