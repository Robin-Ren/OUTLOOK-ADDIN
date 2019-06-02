using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    /// <summary>
    /// Base View Model class which implements the INotifyPropertyChanged interface. 
    /// It also provides a Refresh method to refresh the data in a child class.
    /// </summary>
    public abstract class ABaseViewModel : INotifyPropertyChanged
    {
        protected IUIWindowDialogService _winDialogService = new WindowDialogService();

        /// <summary>
        /// Refreshes the view model state
        /// </summary>
        public virtual void Refresh()
        {
        }

        /// <summary>
        /// Logs an exception to the log file.
        /// </summary>
        /// <param name="e">Exception to log.</param>
        /// <summary>
        /// Logs an exception to the log file.
        /// </summary>
        /// <param name="e">Exception to log.</param>
        protected void LogExceptionDetails(Exception e)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalConstants.ErrorLogFile);

            StringBuilder errorMessage = new StringBuilder();
            errorMessage.Append("Exception occurred at: ");
            errorMessage.AppendLine(DateTime.Now.ToString());
            errorMessage.AppendLine(e.Message);
            errorMessage.AppendLine("Stack trace:");
            errorMessage.AppendLine(e.StackTrace);
            errorMessage.AppendLine();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
            {
                file.WriteLine(errorMessage.ToString());
            }

            WindowDialogViewModel dialog = new WindowDialogViewModel()
            {
                ShowPositiveButton = true,
                PositiveButtonName = "Ok",
                Title = "Error Occurred",
                Text = "An error occurred. Please see the error log file for details."
            };

            _winDialogService.ShowDialog(dialog);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChanged(string propertyName)
        //{
        //    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        protected Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }

    /// <summary>
    /// A details view model which provides events for saving a record, canceling an
    /// edit on a record, and deleting a record. All will raise events for parent view
    /// models to handle.
    /// </summary>
    public abstract class ADetailsBaseViewModel : ABaseViewModel
    {
        #region Events
        #region Cancel Edit Event
        /// <summary>
        /// Event handler for when editing is canceled.
        /// </summary>
        public event EventHandler CancelEdit;

        /// <summary>
        /// Raises the CancelEdit event.
        /// </summary>
        protected void OnCancel()
        {
            CancelEdit?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Delete Record Event
        /// <summary>
        /// Event handler for when the record is deleted.
        /// </summary>
        public event EventHandler DeleteRecord;

        /// <summary>
        /// Raises the DeleteRecord event.
        /// </summary>
        protected void OnDelete()
        {
            DeleteRecord?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Save Record Event
        /// <summary>
        /// Event handler for when the record is saved
        /// </summary>
        public event EventHandler SaveRecord;

        /// <summary>
        /// Raises the SaveRecord event
        /// </summary>
        protected void OnSave()
        {
            SaveRecord?.Invoke(this, new EventArgs());
        }
        #endregion
        #endregion
    }
}

