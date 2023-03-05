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
            this.ShowIntro = true;
        }
        #endregion

        #region fields
        private RelayCommand _navToLoginCommand = null!;
        private RelayCommand _navToRegisterCommand = null!;
        private RelayCommand _navToLogoutCommand = null!;
        private bool _showLoginControl;
        private bool _showRegisterControl;
        private bool _showIntro;
        #endregion

        #region events
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        #endregion

        #region properties
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

        public bool CanNavigate => true;

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
            if (m != null)
            {
                m.loginUserView.tbUsername.Text = null;
                m.loginUserView.pbPassword.Password = null;
                m.registerUserView.tbUsername.Text = null;
                m.registerUserView.pbPassword.Password = null;
            }
        }
        #endregion
    }
}
