using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookAddIn.Domain
{
    public class AppointmentViewModel : ABaseViewModel
    {
        #region Properties

        private string subject;
        public string Subject 
        {
            get { return subject; }
            set
            {
                if (subject != value)
                {
                    subject = value;
                    OnPropertyChanged();
                }
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime endTime;
        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string body;
        public string Body
        {
            get { return body; }
            set
            {
                if (body != value)
                {
                    body = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DateFormatted
        {
            get { return StartTime.ToLongDateString(); }
        }

        public string TimeSlot
        {
            get
            {
                return string.Format("{0} - {1}",
                StartTime.ToShortTimeString(),
                EndTime.ToShortTimeString());
            }
        }

        public override string ToString()
        {
            return Subject;
        }
        #endregion


    }
}
