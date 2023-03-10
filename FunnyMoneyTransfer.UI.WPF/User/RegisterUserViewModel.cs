using FunnyMoneyTransfer.Data;
using FunnyMoneyTransfer.Security;
using System;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Windows.Input;

namespace FunnyMoneyTransfer.UI.WPF.User
{
    class RegisterUserViewModel : INotifyPropertyChanged
    {
        #region ctor
        public RegisterUserViewModel()
        {
            this.User = new();
            this._db = (((MainWindow)App.Current.MainWindow).DataContext as MainWindowViewModel)!._db;
        }
        
        #endregion

        #region consts
        private const string USERNAME_REQUIRED_ERROR_MESSAGE = "Username is required";
        private const string USERNAME_MAX_LENGTH_ERROR_MESSAGE = "Max length for Username is 50";
        private const string USERNAME_UNIQUE_ERROR_MESSAGE = "Username already exists";
        private const string PASSWORD_REQUIRED_ERROR_MESSAGE = "Password is required";
        private const string PASSWORD_MAX_LENGTH_ERROR_MESSAGE = "Max length for Password is 50";
        #endregion

        #region fields
        private readonly FunnyMoneyTransferContext _db = new();
        private Data.User _user = null!;
        private bool _isValid;
        private bool _showUsernameValidationErrorInUI;
        private string? _usernameValidationErrorMessage = null;
        private bool _showPasswordValidationErrorInUI;
        private string? _passwordValidationErrorMessage = null;
        private RelayCommand _registerCommand = null!;
        #endregion

        #region events
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        #endregion

        #region properties
        public Data.User User
        {
            get => _user;
            set
            {
                if (value != _user)
                {
                    _user = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(User)));
                }
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (value != _isValid)
                {
                    _isValid = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(IsValid)));
                }
            }
        }

        public bool ShowUsernameValidationErrorInUI
        {
            get => _showUsernameValidationErrorInUI;
            set
            {
                if (value != _showUsernameValidationErrorInUI)
                {
                    _showUsernameValidationErrorInUI = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ShowUsernameValidationErrorInUI)));
                }
            }
        }

        public string? UsernameValidationErrorMessage
        {
            get => _usernameValidationErrorMessage;
            set
            {
                if (value != _usernameValidationErrorMessage)
                {
                    _usernameValidationErrorMessage = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(UsernameValidationErrorMessage)));
                }
            }
        }

        public bool ShowPasswordValidationErrorInUI
        {
            get => _showPasswordValidationErrorInUI;
            set
            {
                if (value != _showPasswordValidationErrorInUI)
                {
                    _showPasswordValidationErrorInUI = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ShowPasswordValidationErrorInUI)));
                }
            }
        }

        public string? PasswordValidationErrorMessage
        {
            get => _passwordValidationErrorMessage;
            set
            {
                if (value != _passwordValidationErrorMessage)
                {
                    _passwordValidationErrorMessage = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(PasswordValidationErrorMessage)));
                }
            }
        }

        //https://stackoverflow.com/questions/1483892/how-to-bind-to-a-passwordbox-in-mvvm
        public SecureString SecurePassword { private get; set; } = null!;

        //https://stackoverflow.com/questions/862570/how-can-i-use-the-relaycommand-in-wpf
        public ICommand RegisterCommand
        {
            get
            {
                if (_registerCommand == null)
                {
                    _registerCommand = new RelayCommand(param => this.Register(),
                        param => this.CanRegister);
                }
                return _registerCommand;
            }
        }

        private bool CanRegister
        {
            get
            {
                this.Validate();
                return this.IsValid;
            }
        }
        #endregion

        #region methods
        //these validation rules are also enforced in the database users table
        public void Validate()
        {
            if (this.User.Username == null || this.User.Username == string.Empty || string.IsNullOrWhiteSpace(this.User.Username))
            {
                this.IsValid = false;
                this.ShowUsernameValidationErrorInUI = true;
                this.UsernameValidationErrorMessage = USERNAME_REQUIRED_ERROR_MESSAGE;
            }
            else if (this.User.Username!.Length > 50)
            {
                this.IsValid = false;
                this.ShowUsernameValidationErrorInUI = true;
                this.UsernameValidationErrorMessage = USERNAME_MAX_LENGTH_ERROR_MESSAGE;
            }
            else if (_db.Users.Select(u => u.Username).ToList().Contains<string>(this.User.Username))
            {
                this.IsValid = false;
                this.ShowUsernameValidationErrorInUI = true;
                this.UsernameValidationErrorMessage = USERNAME_UNIQUE_ERROR_MESSAGE;
            }
            else if (this.SecurePassword == null || this.SecurePassword.ToString() == string.Empty || string.IsNullOrWhiteSpace(this.SecurePassword.ToString()))
            {
                this.IsValid = false;
                this.ShowPasswordValidationErrorInUI = true;
                this.PasswordValidationErrorMessage = PASSWORD_REQUIRED_ERROR_MESSAGE;
            }
            else if (this.SecurePassword.ToString()!.Length > 50)
            {
                this.IsValid = false;
                this.ShowPasswordValidationErrorInUI = true;
                this.PasswordValidationErrorMessage = PASSWORD_MAX_LENGTH_ERROR_MESSAGE;
            }
            else
            {
                this.IsValid = true;
                this.ShowUsernameValidationErrorInUI = false;
                this.UsernameValidationErrorMessage = null;
                this.ShowPasswordValidationErrorInUI = false;
                this.PasswordValidationErrorMessage = null;
            }
        }

        public void Register()
        {
            this.Validate();
            if (this.IsValid)
            {
                this.User.PasswordHash = PasswordHasher.GetPasswordHash(this.SecurePassword.ToString()!);

                DateTime createDate = DateTime.Now;
                this.User.CreateDate = createDate;

                this.User.Account = new()
                {
                    StartingBalance = new()
                    {
                        Amount = StartingBalance.NEW_ACCOUNT_STARTING_BALANCE,
                        CreateDate = createDate
                    },
                    CreateDate = createDate
                };
                
                this.User.IsLoggedIn = true;
                
                _db.Users.Add(this.User);
                _db.SaveChanges();

                MainWindowViewModel mainWindowContext = (MainWindowViewModel)App.Current.MainWindow.DataContext;

                if (mainWindowContext is MainWindowViewModel)
                {
                    mainWindowContext.LoggedInUser = this.User;
                    mainWindowContext.ShowLoginControl = false;
                    mainWindowContext.ShowRegisterControl = false;
                    mainWindowContext.ShowIntro = false;
                    mainWindowContext.Users = mainWindowContext.GetAllUsersFromDB();
                }
            }
        }
        #endregion
    }
}
