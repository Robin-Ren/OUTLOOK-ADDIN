using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OutlookAddIn.CustomScheduler;
using OutlookAddIn.CustomScheduler.Model;

namespace OutlookAddIn
{
    /// <summary>
    /// Scheduler.xaml 的交互逻辑
    /// </summary>
    public partial class Scheduler : UserControl
    {
        private Appointments appointments = new Appointments();

        public Scheduler()
        {
            InitializeComponent();
            ApplyTemplate();

            DataContext = appointments;
        }

        private void Calendar_AddAppointment(object sender, RoutedEventArgs e)
        {
            Appointment appointment = new Appointment();
            appointment.Subject = "Subject?";
            appointment.StartTime = new DateTime(2008, 10, 22, 16, 00, 00);
            appointment.EndTime = new DateTime(2008, 10, 22, 17, 00, 00);

            AddAppointmentWindow aaw = new AddAppointmentWindow();
            aaw.DataContext = appointment;
            aaw.ShowDialog();

            appointments.Add(appointment);
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
