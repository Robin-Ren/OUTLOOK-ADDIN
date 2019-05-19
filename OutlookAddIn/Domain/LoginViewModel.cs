using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OutlookAddIn.CustomScheduler.Model;

namespace OutlookAddIn.Domain
{
    public delegate void LoginEventHandler(object sender, EventArgs e);

    public class LoginViewModel : ABaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        #region Events
        /// <summary>
        /// Raised opening new booking rooms button is pressed.
        /// </summary>
        public static event LoginEventHandler DoLogin;

        /// <summary>
        /// Raises the OpenNewBookingRooms event
        /// </summary>
        protected void OnLogin()
        {
            DoLogin?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; set; }
        #endregion

        #region Command Implementations
        private void Login(object obj)
        {
            // Just raise the OnLogin Event
            OnLogin();
        }
        #endregion

        #region Properties
        private string _userId;
        private string _password;

        public string UserID
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
