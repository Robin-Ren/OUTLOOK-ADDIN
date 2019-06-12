using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OutlookAddin.Domain;

namespace OutlookAddIn
{
    /// <summary>
    /// Scheduler.xaml
    /// </summary>
    public partial class SchedulerControl : UserControl
    {
        public SchedulerControl(SchedulerViewModel schedulerViewModel)
        {
            InitializeComponent();
            ApplyTemplate();

            DataContext = schedulerViewModel;
        }

        private void Calendar_AddAppointment(object sender, RoutedEventArgs e)
        {
            Appointment appointment = new Appointment();
            appointment.Subject = "Subject?";
            appointment.StartTime = new DateTime(2008, 10, 22, 16, 00, 00);
            appointment.EndTime = new DateTime(2008, 10, 22, 17, 00, 00);

            ((SchedulerViewModel)DataContext).Appointments.Add(appointment);
        }

        private void Child_Close(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private IEnumerable<DependencyObject> Ancestors()
        {
            DependencyObject current = VisualTreeHelper.GetParent(this);
            while (current != null)
            {
                yield return current;
                current = VisualTreeHelper.GetParent(current);
            }
        }
    }
}
