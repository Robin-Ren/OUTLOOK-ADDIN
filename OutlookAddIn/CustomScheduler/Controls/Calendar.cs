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
using OutlookAddin.Domain;
using OutlookAddIn.CustomScheduler.Model;
using OutlookAddIn.WebAPIClient;

namespace OutlookAddIn.CustomScheduler.Controls
{
    public class Calendar : Control
    {
        private static WebAPIDataAccess apiDataAccess;

        static Calendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(typeof(Calendar)));

            CommandManager.RegisterClassCommandBinding(typeof(Calendar), new CommandBinding(NextDay, new ExecutedRoutedEventHandler(OnExecutedNextDay), new CanExecuteRoutedEventHandler(OnCanExecuteNextDay)));
            CommandManager.RegisterClassCommandBinding(typeof(Calendar), new CommandBinding(PreviousDay, new ExecutedRoutedEventHandler(OnExecutedPreviousDay), new CanExecuteRoutedEventHandler(OnCanExecutePreviousDay)));

            CommandManager.RegisterClassCommandBinding(typeof(Calendar), new CommandBinding(CloseDialog, new ExecutedRoutedEventHandler(OnExecutedCloseDialog), new CanExecuteRoutedEventHandler(OnCanCloseDialog)));

            // Initialize WebAPI Client
            apiDataAccess = new WebAPIDataAccess();
        }

        #region AddAppointment

        public static readonly RoutedEvent AddAppointmentEvent =
            CalendarTimeslotItem.AddAppointmentEvent.AddOwner(typeof(CalendarDay));

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

        #endregion

        #region Appointments

        public static readonly DependencyProperty AppointmentsProperty =
            DependencyProperty.Register("Appointments", typeof(IEnumerable<Appointment>), typeof(Calendar),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Calendar.OnAppointmentsChanged)));

        public IEnumerable<Appointment> Appointments
        {
            get { return (IEnumerable<Appointment>)GetValue(AppointmentsProperty); }
            set { SetValue(AppointmentsProperty, value); }
        }

        private static void OnAppointmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnAppointmentsChanged(e);
        }

        protected virtual void OnAppointmentsChanged(DependencyPropertyChangedEventArgs e)
        {
            ApplyTemplate();
        }

        #endregion

        #region FacilityID

        public static readonly DependencyProperty FacilityIDProperty =
            DependencyProperty.Register("FacilityID", typeof(int), typeof(Calendar),
            new FrameworkPropertyMetadata(0, new PropertyChangedCallback(Calendar.OnFacilityIDChanged)));

        public int FacilityID
        {
            get { return (int)GetValue(FacilityIDProperty); }
            set
            {
                SetValue(FacilityIDProperty, value);
            }
        }

        private static void OnFacilityIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnFacilityIDChanged(e);
        }

        protected virtual void OnFacilityIDChanged(DependencyPropertyChangedEventArgs e)
        {
            ApplyTemplate();
        }

        #endregion

        #region TimeSlots

        public static readonly DependencyProperty TimeSlotsProperty =
            DependencyProperty.Register("TimeSlots", typeof(IEnumerable<TimeSlot>), typeof(Calendar),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Calendar.OnTimeSlotsChanged)));

        public IEnumerable<TimeSlot> TimeSlots
        {
            get { return (IEnumerable<TimeSlot>)GetValue(TimeSlotsProperty); }
            set { SetValue(TimeSlotsProperty, value); }
        }

        private static void OnTimeSlotsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnTimeSlotsChanged(e);
        }

        protected virtual void OnTimeSlotsChanged(DependencyPropertyChangedEventArgs e)
        {
            ApplyTemplate();
        }
        #endregion

        #region CurrentDate

        /// <summary>
        /// CurrentDate Dependency Property
        /// </summary>
        public static readonly DependencyProperty CurrentDateProperty =
            DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(Calendar),
                new FrameworkPropertyMetadata(DateTime.Now,
                    new PropertyChangedCallback(OnCurrentDateChanged)));

        /// <summary>
        /// Gets or sets the CurrentDate property.  This dependency property 
        /// indicates ....
        /// </summary>
        public DateTime CurrentDate
        {
            get { return (DateTime)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CurrentDate property.
        /// </summary>
        private static void OnCurrentDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnCurrentDateChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CurrentDate property.
        /// </summary>
        protected virtual void OnCurrentDateChanged(DependencyPropertyChangedEventArgs e)
        {
            FilterAppointments();
        }

        #endregion

        #region Current Week
        /// <summary>
        /// CurrentWeek Dependency Property
        /// </summary>
        public static readonly DependencyProperty CurrentWeekProperty =
            DependencyProperty.Register("CurrentWeek", typeof(CurrentWeek), typeof(Calendar),
                new FrameworkPropertyMetadata(new CurrentWeek(),
                    new PropertyChangedCallback(OnCurrentWeekChanged)));

        /// <summary>
        /// Gets or sets the CurrentWeek property.  This dependency property 
        /// indicates ....
        /// </summary>
        public CurrentWeek CurrentWeek
        {
            get { return (CurrentWeek)GetValue(CurrentWeekProperty); }
            set { SetValue(CurrentWeekProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CurrentWeek property.
        /// </summary>
        private static void OnCurrentWeekChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Calendar)d).OnCurrentWeekChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CurrentWeek property.
        /// </summary>
        protected virtual void OnCurrentWeekChanged(DependencyPropertyChangedEventArgs e)
        {
            FilterAppointments();
        }
        #endregion

        #region NextDay/PreviousDay

        public static readonly RoutedCommand NextDay = new RoutedCommand("NextDay", typeof(Calendar));
        public static readonly RoutedCommand PreviousDay = new RoutedCommand("PreviousDay", typeof(Calendar));

        private static void OnCanExecuteNextDay(object sender, CanExecuteRoutedEventArgs e)
        {
            ((Calendar)sender).OnCanExecuteNextDay(e);
        }

        private static void OnExecutedNextDay(object sender, ExecutedRoutedEventArgs e)
        {
            ((Calendar)sender).OnExecutedNextDay(e);
        }

        protected virtual void OnCanExecuteNextDay(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = false;
        }

        protected virtual async void OnExecutedNextDay(ExecutedRoutedEventArgs e)
        {
            ClearCheckedCalendarTimeslots();

            CurrentDate += TimeSpan.FromDays(7);
            CurrentWeek = new CurrentWeek(CurrentDate);

            //Get all timeslots by dates
            long fromTicks = Utils.ConvertDateTimeToUnixTicks(new DateTime(
                CurrentDate.Year,
                CurrentDate.Month,
                CurrentDate.Day,
                00, 00, 00));
            long toTicks = Utils.ConvertDateTimeToUnixTicks(new DateTime(
                CurrentDate.AddDays(6).Year,
                CurrentDate.AddDays(6).Month,
                CurrentDate.AddDays(6).Day,
                23, 59, 59));

            this.TimeSlots = await apiDataAccess.GetTimeSlots(
                FacilityID,
                fromTicks,
                toTicks);

            CheckTimeslotsAvailability();

            e.Handled = true;
        }

        private static void OnCanExecutePreviousDay(object sender, CanExecuteRoutedEventArgs e)
        {
            ((Calendar)sender).OnCanExecutePreviousDay(e);
        }

        private static void OnExecutedPreviousDay(object sender, ExecutedRoutedEventArgs e)
        {
            ((Calendar)sender).OnExecutedPreviousDay(e);
        }

        protected virtual void OnCanExecutePreviousDay(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = false;
        }

        protected virtual async void OnExecutedPreviousDay(ExecutedRoutedEventArgs e)
        {
            ClearCheckedCalendarTimeslots();

            CurrentDate -= TimeSpan.FromDays(7);
            CurrentWeek = new CurrentWeek(CurrentDate);

            //Get all timeslots by dates
            long fromTicks = Utils.ConvertDateTimeToUnixTicks(new DateTime(
                CurrentDate.Year,
                CurrentDate.Month,
                CurrentDate.Day,
                00, 00, 00));
            long toTicks = Utils.ConvertDateTimeToUnixTicks(new DateTime(
                CurrentDate.AddDays(6).Year,
                CurrentDate.AddDays(6).Month,
                CurrentDate.AddDays(6).Day,
                23, 59, 59));

            this.TimeSlots = await apiDataAccess.GetTimeSlots(
                FacilityID,
                fromTicks,
                toTicks);

            CheckTimeslotsAvailability();

            e.Handled = true;
        }

        private void ClearCheckedCalendarTimeslots()
        {
            var checkedTimeslots = DependencyObjectHelper
                .FindVisualChildren<CalendarTimeslotItem>(this)
                .Where(x => x.IsChecked == true)
                .OrderBy(x => x.TimeslotDate)
                .ThenBy(x => x.TimeslotStart)
                .ToList();

            if (checkedTimeslots != null)
            {
                foreach (var timeslot in checkedTimeslots)
                {
                    timeslot.IsChecked = false;
                }
            }
        }

        public Tuple<DateTime?, DateTime?> GetSelectedTimeslots()
        {
            var checkedTimeslots = DependencyObjectHelper
                .FindVisualChildren<CalendarTimeslotItem>(this)
                .Where(x => x.IsChecked == true)
                .OrderBy(x => x.TimeslotDate)
                .ThenBy(x => x.TimeslotStart)
                .ToList();

            if (checkedTimeslots != null)
            {
                DateTime startTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                CalendarTimeslotItem previousTimeslot = null;

                var last = checkedTimeslots.Last();
                foreach (var timeslot in checkedTimeslots)
                {
                    if (previousTimeslot == null)
                    {
                        previousTimeslot = timeslot;
                        startTime = new DateTime(
                            timeslot.TimeslotDate.Year,
                            timeslot.TimeslotDate.Month,
                            timeslot.TimeslotDate.Day,
                            Convert.ToInt32(timeslot.TimeslotStart.Substring(0, 2)),
                            Convert.ToInt32(timeslot.TimeslotStart.Substring(3, 2)),
                            00);
                        endTime = new DateTime(
                            timeslot.TimeslotDate.Year,
                            timeslot.TimeslotDate.Month,
                            timeslot.TimeslotDate.Day,
                            Convert.ToInt32(timeslot.TimeslotEnd.Substring(0, 2)),
                            Convert.ToInt32(timeslot.TimeslotEnd.Substring(3, 2)),
                            00);
                    }
                    else if (timeslot.Equals(last))
                    {
                        endTime = new DateTime(
                            timeslot.TimeslotDate.Year,
                            timeslot.TimeslotDate.Month,
                            timeslot.TimeslotDate.Day,
                            Convert.ToInt32(timeslot.TimeslotEnd.Substring(0, 2)),
                            Convert.ToInt32(timeslot.TimeslotEnd.Substring(3, 2)),
                            00);
                    }
                    else
                    {
                        if (timeslot.IsAdjacentAfter(previousTimeslot))
                        {
                            previousTimeslot = timeslot;
                        }
                        else
                        {
                            endTime = new DateTime(
                            previousTimeslot.TimeslotDate.Year,
                            previousTimeslot.TimeslotDate.Month,
                            previousTimeslot.TimeslotDate.Day,
                            Convert.ToInt32(previousTimeslot.TimeslotEnd.Substring(0, 2)),
                            Convert.ToInt32(previousTimeslot.TimeslotEnd.Substring(3, 2)),
                            00);

                            break;
                        }
                    }
                }

                return new Tuple<DateTime?, DateTime?>(startTime, endTime);
            }

            return new Tuple<DateTime?, DateTime?>(null, null);
        }

        public void CheckTimeslotsAvailability()
        {
            if (this.TimeSlots == null) return;

            var allTimeslots = DependencyObjectHelper
                .FindVisualChildren<CalendarTimeslotItem>(this)
                .OrderBy(x => x.TimeslotDate)
                .ThenBy(x => x.TimeslotStart)
                .ToList();

            if (allTimeslots != null)
            {
                foreach (var timeslot in allTimeslots)
                {
                    var timeslotDate = new DateTime(
                        timeslot.TimeslotDate.Year,
                        timeslot.TimeslotDate.Month,
                        timeslot.TimeslotDate.Day,
                        Convert.ToInt32(timeslot.TimeslotStart.Substring(0, 2)),
                        Convert.ToInt32(timeslot.TimeslotStart.Substring(3, 2)),
                        00);
                    var sysTimeslot = this.TimeSlots
                        .Where(x => Utils.ConvertUnixTicksToDateTime(x.from).Equals(timeslotDate))
                        .FirstOrDefault();

                    //if (sysTimeslot == null ||
                    //   !sysTimeslot.available)
                    {
                        timeslot.IsEnabled = false;
                    }
                    //else
                    //{
                    //    timeslot.IsAvailable = true;
                    //timeslot.Focusable = false;
                    //}
                }
            }
        }
        #endregion

        public static readonly RoutedCommand CloseDialog = new RoutedCommand("CloseDialog", typeof(Calendar));

        private static void OnCanCloseDialog(object sender, CanExecuteRoutedEventArgs e)
        {
            ((Calendar)sender).OnCanCloseDialog(e);
        }

        private static void OnExecutedCloseDialog(object sender, ExecutedRoutedEventArgs e)
        {
            ((Calendar)sender).OnExecutedCloseDialog(e);
        }

        protected virtual void OnCanCloseDialog(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = false;
        }

        protected virtual void OnExecutedCloseDialog(ExecutedRoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void FilterAppointments()
        {
            CalendarDay day1 = this.GetTemplateChild("day1") as CalendarDay;
            day1.ItemsSource = Appointments.ByDate(CurrentWeek.Day1);
            day1.Day = CurrentWeek.Day1;
            TextBlock dayHeader1 = this.GetTemplateChild("dayHeader1") as TextBlock;
            dayHeader1.Text = CurrentDate.ToShortDateString();

            CalendarDay day2 = this.GetTemplateChild("day2") as CalendarDay;
            day2.ItemsSource = Appointments.ByDate(CurrentWeek.Day2);
            day2.Day = CurrentWeek.Day2;
            TextBlock dayHeader2 = this.GetTemplateChild("dayHeader2") as TextBlock;
            dayHeader2.Text = CurrentWeek.Day2.ToShortDateString();

            CalendarDay day3 = this.GetTemplateChild("day3") as CalendarDay;
            day3.ItemsSource = Appointments.ByDate(CurrentWeek.Day3);
            day3.Day = CurrentWeek.Day3;
            TextBlock dayHeader3 = this.GetTemplateChild("dayHeader3") as TextBlock;
            dayHeader3.Text = CurrentWeek.Day3.ToShortDateString();

            CalendarDay day4 = this.GetTemplateChild("day4") as CalendarDay;
            day4.ItemsSource = Appointments.ByDate(CurrentWeek.Day4);
            day4.Day = CurrentWeek.Day4;
            TextBlock dayHeader4 = this.GetTemplateChild("dayHeader4") as TextBlock;
            dayHeader4.Text = CurrentWeek.Day4.ToShortDateString();

            CalendarDay day5 = this.GetTemplateChild("day5") as CalendarDay;
            day5.ItemsSource = Appointments.ByDate(CurrentWeek.Day5);
            day5.Day = CurrentWeek.Day5;
            TextBlock dayHeader5 = this.GetTemplateChild("dayHeader5") as TextBlock;
            dayHeader5.Text = CurrentWeek.Day5.ToShortDateString();

            CalendarDay day6 = this.GetTemplateChild("day6") as CalendarDay;
            day6.ItemsSource = Appointments.ByDate(CurrentWeek.Day6);
            day6.Day = CurrentWeek.Day6;
            TextBlock dayHeader6 = this.GetTemplateChild("dayHeader6") as TextBlock;
            dayHeader6.Text = CurrentWeek.Day6.ToShortDateString();

            CalendarDay day7 = this.GetTemplateChild("day7") as CalendarDay;
            day7.ItemsSource = Appointments.ByDate(CurrentWeek.Day7);
            day7.Day = CurrentWeek.Day7;
            TextBlock dayHeader7 = this.GetTemplateChild("dayHeader7") as TextBlock;
            dayHeader7.Text = CurrentWeek.Day7.ToShortDateString();
        }

        public override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            FilterAppointments();

            if (this.TimeSlots == null && FacilityID > 0)
            {
                //Get all timeslots by dates
                long fromTicks = Utils.ConvertDateTimeToUnixTicks(new DateTime(
                    CurrentDate.Year,
                    CurrentDate.Month,
                    CurrentDate.Day,
                    00, 00, 00));
                long toTicks = Utils.ConvertDateTimeToUnixTicks(new DateTime(
                    CurrentDate.AddDays(6).Year,
                    CurrentDate.AddDays(6).Month,
                    CurrentDate.AddDays(6).Day,
                    23, 59, 59));

                this.TimeSlots = await apiDataAccess.GetTimeSlots(
                    FacilityID,
                    fromTicks,
                    toTicks);
            }

            if (this.TimeSlots != null)
            {
                CheckTimeslotsAvailability();
            }
        }
    }
}
