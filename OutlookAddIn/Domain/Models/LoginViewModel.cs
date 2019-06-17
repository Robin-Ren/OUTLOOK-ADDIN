using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OutlookAddin.Domain
{
    public delegate void LoginEventHandler(object sender, LoginEventArgs e);

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
        protected void OnLogin(LoginEventArgs args)
        {
            DoLogin?.Invoke(this, args);
        }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; set; }
        #endregion

        #region Command Implementations
        private void Login(object obj)
        {
            var loginModel = (LoginViewModel)obj;

            if (loginModel == null) return;

            LoginEventArgs loginArgs = new LoginEventArgs
            {
                UserName = loginModel.UserName,
                Password = loginModel.Password,
                RememberMe = loginModel.RememberMe
            };

            // Just raise the OnLogin Event
            OnLogin(loginArgs);
        }
        #endregion

        #region Properties
        private string _userName;
        private string _password;
        private bool _rememberMe;
        private bool _isSucceeded;
        private string _loginMessage;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
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

        public bool RememberMe
        {
            get { return _rememberMe; }
            set
            {
                _rememberMe = value;
                OnPropertyChanged();
            }
        }

        public bool IsSucceeded
        {
            get { return _isSucceeded; }
            set
            {
                _isSucceeded = value;
                OnPropertyChanged();
            }
        }

        public string LoginMessage
        {
            get { return _loginMessage; }
            set
            {
                _loginMessage = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
