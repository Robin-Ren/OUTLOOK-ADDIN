using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using OutlookAddin.Domain;

namespace OutlookAddIn.CustomScheduler.Model
{
    public class Appointments : ObservableCollectionWrapper<Appointment>
    {
        public Appointments()
        {
            Add(new Appointment() { Subject = "Dummy Appointment #1", StartTime = new DateTime(2019, 5, 11, 12, 00, 00), EndTime = new DateTime(2019, 5, 11, 14, 00, 00) });
            Add(new Appointment() { Subject = "Dummy Appointment #2", StartTime = new DateTime(2019, 5, 12, 11, 30, 00), EndTime = new DateTime(2019, 5, 12, 12, 00, 00) });
            Add(new Appointment() { Subject = "Dummy Appointment #3", StartTime = new DateTime(2019, 5, 13, 16, 00, 00), EndTime = new DateTime(2019, 5, 13, 17, 30, 00) });
            Add(new Appointment() { Subject = "Dummy Appointment #4", StartTime = new DateTime(2019, 5, 15, 12, 00, 00), EndTime = new DateTime(2019, 5, 15, 14, 00, 00) });
            Add(new Appointment() { Subject = "Dummy Appointment #5", StartTime = new DateTime(2019, 5, 16, 11, 30, 00), EndTime = new DateTime(2019, 5, 16, 12, 00, 00) });
            Add(new Appointment() { Subject = "Dummy Appointment #6", StartTime = new DateTime(2019, 5, 17, 16, 00, 00), EndTime = new DateTime(2019, 5, 17, 17, 30, 00) });
            Add(new Appointment() { Subject = "Dummy Appointment #7", StartTime = new DateTime(2019, 5, 18, 12, 00, 00), EndTime = new DateTime(2019, 5, 18, 14, 00, 00) });
            Add(new Appointment() { Subject = "Dummy Appointment #8", StartTime = new DateTime(2019, 5, 19, 11, 30, 00), EndTime = new DateTime(2019, 5, 19, 12, 00, 00) });
            Add(new Appointment() { Subject = "Dummy Appointment #9", StartTime = new DateTime(2019, 5, 19, 16, 00, 00), EndTime = new DateTime(2019, 5, 19, 17, 30, 00) });
        }
    }
}
