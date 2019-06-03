using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookAddIn.CustomScheduler.Model
{
    public class Appointment : BindableObject
    {
        private string subject;
        public string Subject 
        {
            get { return subject; }
            set
            {
                if (subject != value)
                {
                    subject = value;
                    RaisePropertyChanged("Subject");
                }
            }
        }

        private int _facilityId;
        public int FacilityID
        {
            get { return _facilityId; }
            set
            {
                if (_facilityId != value)
                {
                    _facilityId = value;
                    RaisePropertyChanged("FacilityID");
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
                    RaisePropertyChanged("StartTime");
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
                    RaisePropertyChanged("EndTime");
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
                    RaisePropertyChanged("Body");
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
    }
}
