using FunnyMoneyTransfer.UI.WPF.User;
using System.ComponentModel;
using System.Windows.Input;

namespace FunnyMoneyTransfer.UI.WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region ctor
        public MainWindowViewModel()
        {
            this.ShowLoginAndRegisterMenuItems = true;
            this.ShowNonLoginAndRegisterMenuItems = false;
            this.ShowIntro = true;
            this.ShowBalanceControl = false;
            this.ShowLoginControl = false;
            this.ShowRegisterControl = false;
            this.ShowUserListControl = false;
        }
        #endregion

        #region fields
        private Data.User _loggedInUser = null!;
        private RelayCommand _navToLoginCommand = null!;
        private RelayCommand _navToRegisterCommand = null!;
        private RelayCommand _navToLogoutCommand = null!;
        private ICommand _navToUsersListCommand = null!;
        private bool _showLoginAndRegisterMenuItems;
        private bool _showNonLoginAndRegisterMenuItems;
        private bool _showBalanceControl;
        private bool _showIntro;
        private bool _showLoginControl;
        private bool _showRegisterControl;
        private bool _showUserListControl;
        #endregion

        #region events
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        #endregion

        #region properties       
        public Data.User LoggedInUser
        {
            get => _loggedInUser;
            set
            {
                if (value != _loggedInUser)
                {
                    _loggedInUser = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(LoggedInUser)));
                }
            }
        }

        public bool ShowLoginAndRegisterMenuItems
        {
            get => _showLoginAndRegisterMenuItems;
            set
            {
                if (value != _showLoginAndRegisterMenuItems)
                {
                    _showLoginAndRegisterMenuItems = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ShowLoginAndRegisterMenuItems)));
                }
            }
        }

        public bool ShowNonLoginAndRegisterMenuItems
        {
            get => _showNonLoginAndRegisterMenuItems;
            set
            {
                if (value != _showNonLoginAndRegisterMenuItems)
                {
                    _showNonLoginAndRegisterMenuItems = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ShowNonLoginAndRegisterMenuItems)));
                }
            }
        }

        public bool ShowBalanceControl
        {
            get => _showBalanceControl;
            set
            {
                if (value != _showBalanceControl)
                {
                    _showBalanceControl = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ShowBalanceControl)));
                }
            }
        }

        public bool ShowIntro
        {
            get => _showIntro;
            set
            {
                if (value != _showIntro)
                {
                    _showIntro = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ShowIntro)));
                }

            }
        }

        public bool ShowLoginControl
        {
            get => _showLoginControl;
            set
            {
                if (value != _showLoginControl)
                {
                    _showLoginControl = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ShowLoginControl)));
                }
                _showLoginControl = value;
            }
        }

        public bool ShowUserListControl
        {
            get => _showUserListControl;
            set
            {
                if (value != _showUserListControl)
                {
                    _showUserListControl = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ShowUserListControl)));
                }
                _showUserListControl = value;
            }
        }

        public bool ShowRegisterControl
        {
            get => _showRegisterControl;
            set
            {
                if (value != _showRegisterControl)
                {
                    _showRegisterControl = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ShowRegisterControl)));
                }
                _showRegisterControl = value;
            }
        }

        public ICommand NavToRegisterCommand
        {
            get
            {
                if (_navToRegisterCommand == null)
                {
                    _navToRegisterCommand = new RelayCommand(param => this.NavToRegister(),
                        param => this.CanNavigate);
                }
                return _navToRegisterCommand;
            }
        }

        public ICommand NavToLoginCommand
        {
            get
            {
                if (_navToLoginCommand == null)
                {
                    _navToLoginCommand = new RelayCommand(param => this.NavToLogin(),
                        param => this.CanNavigate);
                }
                return _navToLoginCommand;
            }
        }

        public ICommand NavToLogoutCommand
        {
            get
            {
                if (_navToLogoutCommand == null)
                {
                    _navToLogoutCommand = new RelayCommand(param => this.NavToLogout(),
                        param => this.CanNavigate);
                }
                return _navToLogoutCommand;
            }
        }

        public ICommand NavToUsersListCommand
        {
            get
            {
                if (_navToUsersListCommand == null)
                {
                    _navToUsersListCommand = new RelayCommand(param => this.NavToUsersList(),
                        param => this.CanNavigate);
                }
                return _navToUsersListCommand;
            }
        }

        public bool CanNavigate => true;
        #endregion

        #region methods
        private void NavToRegister()
        {
            this.ShowLoginAndRegisterMenuItems = !(this.LoggedInUser is Data.User);
            this.ShowNonLoginAndRegisterMenuItems = !ShowLoginAndRegisterMenuItems;
            this.ShowIntro = false;
            this.ShowBalanceControl = false;
            this.ShowLoginControl = false;
            this.ShowRegisterControl = true;
            this.ShowUserListControl = false;
        } //TODO: Get the nav commands combined into one command with a param for destination

        private void NavToLogin()
        {
            this.ShowLoginAndRegisterMenuItems = !(this.LoggedInUser is Data.User);
            this.ShowNonLoginAndRegisterMenuItems = !ShowLoginAndRegisterMenuItems;
            this.ShowIntro = false;
            this.ShowBalanceControl = false;
            this.ShowLoginControl = true;
            this.ShowRegisterControl = false;
            this.ShowUserListControl = false;
        }

        private void NavToLogout()
        {
            this.LoggedInUser = null!;
            this.ShowLoginAndRegisterMenuItems = !(this.LoggedInUser is Data.User);
            this.ShowNonLoginAndRegisterMenuItems = !ShowLoginAndRegisterMenuItems;
            this.ShowIntro = true;
            this.ShowBalanceControl = false;
            this.ShowLoginControl = false;
            this.ShowRegisterControl = false;
            this.ShowUserListControl = false;

            MainWindow m = (MainWindow)App.Current.MainWindow;
            if (m != null)
            {
                m.loginUserView.tbUsername.Text = null;
                m.loginUserView.pbPassword.Password = null;
                m.registerUserView.tbUsername.Text = null;
                m.registerUserView.pbPassword.Password = null;
            }
        }

        private void NavToUsersList()
        {
            this.ShowLoginAndRegisterMenuItems = !(this.LoggedInUser is Data.User);
            this.ShowNonLoginAndRegisterMenuItems = !ShowLoginAndRegisterMenuItems;
            this.ShowIntro = false;
            this.ShowBalanceControl = true;
            this.ShowLoginControl = false;
            this.ShowRegisterControl = false;
            this.ShowUserListControl = true;
        }
        #endregion
    }
}
