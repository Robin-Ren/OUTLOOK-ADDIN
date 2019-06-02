using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OutlookAddin.Domain
{
    public class WindowDialogViewModel : INotifyPropertyChanged
    {
        private bool? m_dialogResult;

        #region Public Properties
        /// <summary>
        /// Gets and sets the title of the dialog
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets and sets the text to display in the dialog box
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets and sets the name to display for the button that returns true
        /// </summary>
        public string PositiveButtonName { get; set; }

        /// <summary>
        /// Gets and sets the name to display for the button that returns false
        /// </summary>
        public string NegativeButtonName { get; set; }

        /// <summary>
        /// Gets and sets the name to display for the button that returns null
        /// </summary>
        public string CancelButtonName { get; set; }

        /// <summary>
        /// Gets and sets if the true button should be shown
        /// </summary>
        public bool ShowPositiveButton { get; set; }

        /// <summary>
        /// Gets and sets if the false button should be shown
        /// </summary>
        public bool ShowNegativeButton { get; set; }

        /// <summary>
        /// Gets and sets if the cancel / null button should be shown
        /// </summary>
        public bool ShowCancelButton { get; set; }

        public bool? DialogResult
        {
            get { return m_dialogResult; }
            set
            {
                m_dialogResult = value;
                OnPropertyChanged("DialogResult");
            }

        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor. Does not set any defaults for names to display or buttons to show
        /// </summary>
        public WindowDialogViewModel()
        {
            // Create new commands to launch when the user clicks on a button
            this.TrueCommand = new RelayCommand(RunTrueAction);
            this.FalseCommand = new RelayCommand(RunFalseAction);
            this.CancelCommand = new RelayCommand(RunCancelAction);
        }
        #endregion

        #region Commands
        /// <summary>
        /// Gets and sets the Command to run when the positive button is clicked
        /// </summary>
        public ICommand TrueCommand { get; set; }

        /// <summary>
        /// Gets and sets the Command to run when the negative button is clicked
        /// </summary>
        public ICommand FalseCommand { get; set; }

        /// <summary>
        /// Gets and sets the Command to run when the cancel / null button is clicked
        /// </summary>
        public ICommand CancelCommand { get; set; }

        private void RunTrueAction(object obj)
        {
            DialogResult = true;
        }

        private void RunFalseAction(object obj)
        {
            DialogResult = false;
        }

        private void RunCancelAction(object obj)
        {
            DialogResult = null;
        }
        #endregion



        #region INotifyPropertyChanged Members
        /// <summary>
        /// Occurs when a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
