using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OutlookAddIn.Domain
{
    public class MeetingAddinViewModel : INotifyPropertyChanged
    {
        private DateTime _dateStart;
        private DateTime _timeStart;
        private DateTime _dateEnd;
        private DateTime _timeEnd;
        private string _validatingTime;
        private DateTime? _futureValidatingDate;
        private string _selectedTextTwo;

        #region Commands
        public ICommand AcceptCommand { get; }
        #endregion
        public MeetingAddinViewModel()
        {
            DateStart = DateTime.Now;
            TimeStart = DateTime.Now;
            DateEnd = DateTime.Now;
            TimeEnd = DateTime.Now;
            SelectedTextTwo = null;
        }

        public DateTime DateStart
        {
            get { return _dateStart; }
            set
            {
                _dateStart = value;
                OnPropertyChanged();
            }
        }

        public DateTime TimeStart
        {
            get { return _timeStart; }
            set
            {
                _timeStart = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set
            {
                _dateEnd = value;
                OnPropertyChanged();
            }
        }

        public DateTime TimeEnd
        {
            get { return _timeEnd; }
            set
            {
                _timeEnd = value;
                OnPropertyChanged();
            }
        }

        public string ValidatingTime
        {
            get { return _validatingTime; }
            set
            {
                _validatingTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime? FutureValidatingDate
        {
            get { return _futureValidatingDate; }
            set
            {
                _futureValidatingDate = value;
                OnPropertyChanged();
            }
        }

        public string SelectedTextTwo
        {
            get { return _selectedTextTwo; }
            set
            {
                this.MutateVerbose(ref _selectedTextTwo, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
