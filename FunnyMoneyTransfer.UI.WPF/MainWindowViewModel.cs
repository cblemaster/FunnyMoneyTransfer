using FunnyMoneyTransfer.Data;
using FunnyMoneyTransfer.UI.WPF.Transfer;
using FunnyMoneyTransfer.UI.WPF.User;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FunnyMoneyTransfer.UI.WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region ctor
        public MainWindowViewModel() => this.ShowIntro = true;
        #endregion

        #region fields
        public readonly FunnyMoneyTransferContext _db = new();
        private bool _isUserLoggedIn;
        private bool _isNoUserLoggedIn;
        private RelayCommand _navToLoginCommand = null!;
        private RelayCommand _navToRegisterCommand = null!;
        private RelayCommand _navToLogoutCommand = null!;
        private bool _showLoginControl;
        private bool _showRegisterControl;
        private bool _showIntro;        
        private ObservableCollection<Data.User> _users = null!;
        private Data.User _loggedInUser = null!;       
        #endregion

        #region events
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        #endregion

        #region properties
        public bool IsUserLoggedIn
        {
            get => this.GetLoggedInUserFromDB() is Data.User;
            
            set
            {
                if (value != _isUserLoggedIn)
                {
                    _isUserLoggedIn = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(IsUserLoggedIn)));
                }
                
            }
        }

        public bool IsNoUserLoggedIn
        {
            get => !this.IsUserLoggedIn;

            set
            {
                if (value != _isNoUserLoggedIn)
                {
                    _isNoUserLoggedIn = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(IsNoUserLoggedIn)));
                }

            }
        }

        public bool CanNavigate => true;

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

        public Data.User LoggedInUser
        {
            get => _loggedInUser;
            set
            {
                if (value != _loggedInUser)
                {
                    _loggedInUser = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(LoggedInUser)));
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(IsUserLoggedIn)));
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(IsNoUserLoggedIn)));
                }
            }
        }

        public ObservableCollection<Data.User> Users
        {
            get => _users;
            set
            {
                if (value != _users)
                {
                    _users = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(Users)));
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

        #endregion

        #region methods
        public Data.User GetLoggedInUserFromDB() =>
            this._db.Users.SingleOrDefault(u => u.IsLoggedIn!.Value)!;

        public ObservableCollection<Data.User> GetAllUsersFromDB() =>
            new ObservableCollection<Data.User>
                (this._db.Users.OrderBy(u => u.Username).ToList());

        private void NavToRegister()
        {
            this.ShowLoginControl = false;
            this.ShowRegisterControl = true;
            this.ShowIntro = false;
        } //TODO: Get the nav commands combined into one command with a param for destination

        private void NavToLogin()
        {
            this.ShowLoginControl = true;
            this.ShowRegisterControl = false;
            this.ShowIntro = false;
        }

        private void NavToLogout()
        {
            this.ShowLoginControl = false;
            this.ShowRegisterControl = false;
            this.ShowIntro = true;

            MainWindow m = (MainWindow)App.Current.MainWindow;

            if (m is MainWindow)
            {
                m.loginUserView.tbUsername.Text = null;
                m.loginUserView.pbPassword.Password = null;
                m.registerUserView.tbUsername.Text = null;
                m.registerUserView.pbPassword.Password = null;

                if (this.LoggedInUser is Data.User)
                {
                    this.LoggedInUser.IsLoggedIn = false;
                    this._db.SaveChanges();
                }              

                this.LoggedInUser = null!;
            }
        }
        #endregion        
    }
}
