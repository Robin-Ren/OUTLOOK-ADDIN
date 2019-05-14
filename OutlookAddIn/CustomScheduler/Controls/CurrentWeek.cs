using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookAddIn.CustomScheduler.Controls
{
    public class CurrentWeek
    {
        public CurrentWeek(DateTime dateStart)
        {
            _day1 = dateStart;
            _day2 = _day1.AddDays(1);
            _day3 = _day1.AddDays(2);
            _day4 = _day1.AddDays(3);
            _day5 = _day1.AddDays(4);
            _day6 = _day1.AddDays(5);
            _day7 = _day1.AddDays(6);
        }

        public CurrentWeek() : this(DateTime.Now)
        { }

        #region Private Members
        private DateTime _day1;
        private DateTime _day2;
        private DateTime _day3;
        private DateTime _day4;
        private DateTime _day5;
        private DateTime _day6;
        private DateTime _day7;
        #endregion

        public DateTime Day1 { get => _day1; set => _day1 = value; }
        public DateTime Day2 { get => _day2; set => _day2 = value; }
        public DateTime Day3 { get => _day3; set => _day3 = value; }
        public DateTime Day4 { get => _day4; set => _day4 = value; }
        public DateTime Day5 { get => _day5; set => _day5 = value; }
        public DateTime Day6 { get => _day6; set => _day6 = value; }
        public DateTime Day7 { get => _day7; set => _day7 = value; }

    }
}
