using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace OutlookAddIn.CustomScheduler.Controls
{
    public class CalendarTimeslotItem : ToggleButton
    {
        #region TimeslotStart

        public static readonly DependencyProperty TimeslotStartProperty =
            DependencyProperty.Register("TimeslotStart", typeof(string), typeof(CalendarTimeslotItem),
                new FrameworkPropertyMetadata((string)string.Empty));

        public string TimeslotStart
        {
            get { return (string)GetValue(TimeslotStartProperty); }
            set { SetValue(TimeslotStartProperty, value); }
        }

        #endregion

        #region TimeslotEnd
        public string TimeslotEnd
        {
            get
            {
                if (string.IsNullOrEmpty(TimeslotStart))
                    return string.Empty;

                if (TimeslotStart.EndsWith(":00"))
                    return TimeslotStart.Replace(":00", ":30");

                if (TimeslotStart.EndsWith(":30"))
                {
                    string min = "00";
                    int hour = 1 + Convert.ToInt32(TimeslotStart.Substring(0, 2));
                    return hour.ToString().PadLeft(2, '0') + ":" + min;
                }

                return string.Empty;
            }
        }

        #endregion

        #region TimeslotDate

        public static readonly DependencyProperty TimeslotDateProperty =
            DependencyProperty.Register("TimeslotDate", typeof(DateTime), typeof(CalendarTimeslotItem),
                new FrameworkPropertyMetadata(default(DateTime)));

        public DateTime TimeslotDate
        {
            get { return (DateTime)GetValue(TimeslotDateProperty); }
            set { SetValue(TimeslotDateProperty, value); }
        }

        #endregion

        static CalendarTimeslotItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarTimeslotItem), new FrameworkPropertyMetadata(typeof(CalendarTimeslotItem)));
        }

        #region AddAppointment

        private void RaiseAddAppointmentEvent()
        {
            OnAddAppointment(new RoutedEventArgs(AddAppointmentEvent, this));
        }

        public static readonly RoutedEvent AddAppointmentEvent =
            EventManager.RegisterRoutedEvent("AddAppointment", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(CalendarTimeslotItem));

        public event RoutedEventHandler AddAppointment
        {
            add
            {
                AddHandler(AddAppointmentEvent, value);
            }
            remove
            {
                RemoveHandler(AddAppointmentEvent, value);
            }
        }

        protected virtual void OnAddAppointment(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        #endregion

        protected override void OnClick()
        {
            this.IsChecked = !this.IsChecked;
        }
    }
}
