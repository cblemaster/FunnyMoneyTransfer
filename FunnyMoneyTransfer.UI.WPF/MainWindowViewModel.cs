using FunnyMoneyTransfer.UI.WPF.User;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FunnyMoneyTransfer.UI.WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region ctor
        public MainWindowViewModel()
        {
        }
        #endregion

        #region fields
        private Data.User _loggedInUser = null!;
        private RelayCommand _navToLoginCommand = null!;
        private RelayCommand _navToRegisterCommand = null!;
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

        public bool IsUserLoggedIn => !this.IsNoUserLoggedIn;

        public bool IsNoUserLoggedIn => !(this.LoggedInUser is Data.User);

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

        public bool CanNavigate => true;
        #endregion

        #region methods
        private void NavToRegister()
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.tbIntro.Visibility = Visibility.Collapsed;
            mainWindow.registerUserView.Visibility = Visibility.Visible;
            mainWindow.balanceView.Visibility = Visibility.Collapsed;
            mainWindow.loginUserView.Visibility = Visibility.Collapsed;
        } //TODO: Get the nav commands combined into one command with a param for destination
        private void NavToLogin()
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.tbIntro.Visibility = Visibility.Collapsed;
            mainWindow.registerUserView.Visibility = Visibility.Collapsed;
            mainWindow.balanceView.Visibility = Visibility.Collapsed;
            mainWindow.loginUserView.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
