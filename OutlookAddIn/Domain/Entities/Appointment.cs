using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
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

    public class Appointments : ObservableCollectionWrapper<Appointment>
    {
        public Appointments()
        {
            //Add(new Appointment() { Subject = "Dummy Appointment #1", StartTime = new DateTime(2019, 5, 11, 12, 00, 00), EndTime = new DateTime(2019, 5, 11, 14, 00, 00) });
            //Add(new Appointment() { Subject = "Dummy Appointment #2", StartTime = new DateTime(2019, 5, 12, 11, 30, 00), EndTime = new DateTime(2019, 5, 12, 12, 00, 00) });
            //Add(new Appointment() { Subject = "Dummy Appointment #3", StartTime = new DateTime(2019, 5, 13, 16, 00, 00), EndTime = new DateTime(2019, 5, 13, 17, 30, 00) });
            //Add(new Appointment() { Subject = "Dummy Appointment #4", StartTime = new DateTime(2019, 5, 15, 12, 00, 00), EndTime = new DateTime(2019, 5, 15, 14, 00, 00) });
            //Add(new Appointment() { Subject = "Dummy Appointment #5", StartTime = new DateTime(2019, 5, 16, 11, 30, 00), EndTime = new DateTime(2019, 5, 16, 12, 00, 00) });
            //Add(new Appointment() { Subject = "Dummy Appointment #6", StartTime = new DateTime(2019, 5, 17, 16, 00, 00), EndTime = new DateTime(2019, 5, 17, 17, 30, 00) });
            //Add(new Appointment() { Subject = "Dummy Appointment #7", StartTime = new DateTime(2019, 5, 18, 12, 00, 00), EndTime = new DateTime(2019, 5, 18, 14, 00, 00) });
            //Add(new Appointment() { Subject = "Dummy Appointment #8", StartTime = new DateTime(2019, 5, 19, 11, 30, 00), EndTime = new DateTime(2019, 5, 19, 12, 00, 00) });
            //Add(new Appointment() { Subject = "Dummy Appointment #9", StartTime = new DateTime(2019, 5, 19, 16, 00, 00), EndTime = new DateTime(2019, 5, 19, 17, 30, 00) });
        }
    }
}
