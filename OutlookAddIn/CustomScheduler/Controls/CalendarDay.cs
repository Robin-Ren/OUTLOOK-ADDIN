using System;
using System.Windows;
using System.Windows.Controls;

namespace OutlookAddIn.CustomScheduler.Controls
{
    public class CalendarDay : ItemsControl
    {
        static CalendarDay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarDay), new FrameworkPropertyMetadata(typeof(CalendarDay)));
        }

        #region ItemsControl Container Override

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CalendarAppointmentItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is CalendarAppointmentItem);
        }

        #endregion

        #region Day

        public static readonly DependencyProperty DayProperty =
            DependencyProperty.Register("Day", typeof(DateTime), typeof(CalendarDay),
                new FrameworkPropertyMetadata(default(DateTime)));

        public DateTime Day
        {
            get { return (DateTime)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        #endregion
    }
}
