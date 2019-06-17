using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public class TimeSlot : ABaseViewModel, IComparable
    {
        private long _from;
        private long _to;
        private bool _available;
        private string _remarks;
        private int _timeSlotConfigId;
        private string _name;
        private int _slotDuration;
        private bool _isSelected;
        private string _status;

        public long from
        {
            get
            {
                return _from;
            }

            set
            {
                _from = value;
                OnPropertyChanged();
            }
        }
        public long to
        {
            get
            {
                return _to;
            }

            set
            {
                _to = value;
                OnPropertyChanged();
            }
        }
        public bool available
        {
            get
            {
                return _available;
            }

            set
            {
                _available = value;
                OnPropertyChanged();
            }
        }
        public string remarks
        {
            get
            {
                return _remarks;
            }

            set
            {
                _remarks = value;
                OnPropertyChanged();
            }
        }
        public int timeSlotConfigId
        {
            get
            {
                return _timeSlotConfigId;
            }

            set
            {
                _timeSlotConfigId = value;
                OnPropertyChanged();
            }
        }
        public string name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public int slotDuration
        {
            get
            {
                return _slotDuration;
            }

            set
            {
                _slotDuration = value;
                OnPropertyChanged();
            }
        }

        public bool isSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public string status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var timeslot = obj as TimeSlot;

            if (this.from > timeslot.from) return 1;
            if (this.from == timeslot.from) return 0;
            return -1;
        }
        #endregion
    }
}
