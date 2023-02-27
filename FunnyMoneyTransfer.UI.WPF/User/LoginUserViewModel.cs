using FunnyMoneyTransfer.Data;
using FunnyMoneyTransfer.Security;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Windows.Input;

namespace FunnyMoneyTransfer.UI.WPF.User
{
    class LoginUserViewModel : INotifyPropertyChanged
    {
        #region ctor
        public LoginUserViewModel()
        {
            this.User = new();
        }
        #endregion

        #region consts
        private const string USERNAME_REQUIRED_ERROR_MESSAGE = "Username is required";
        private const string USERNAME_MAX_LENGTH_ERROR_MESSAGE = "Max length for Username is 50";
        private const string USERNAME_UNIQUE_ERROR_MESSAGE = "Username already exists";
        #endregion

        #region fields
        private readonly FunnyMoneyTransferContext _db = new();
        private Data.User _user = null!;
        private Data.User _loggedInUser = null!;
        private bool _isValid;
        private bool _showValidationErrorInUI;
        private string? _validationErrorMessage = null;
        private RelayCommand _loginCommand = null!;        
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

        public bool ShowValidationErrorInUI
        {
            get => _showValidationErrorInUI;
            set
            {
                if (value != _showValidationErrorInUI)
                {
                    _showValidationErrorInUI = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ShowValidationErrorInUI)));
                }
            }
        }

        public string? ValidationErrorMessage
        {
            get => _validationErrorMessage;
            set
            {
                if (value != _validationErrorMessage)
                {
                    _validationErrorMessage = value;
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(ValidationErrorMessage)));
                }
            }
        }

        //https://stackoverflow.com/questions/1483892/how-to-bind-to-a-passwordbox-in-mvvm
        public SecureString SecurePassword { private get; set; } = null!;

        //https://stackoverflow.com/questions/862570/how-can-i-use-the-relaycommand-in-wpf
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand(param => this.Login(),
                        param => this.CanLogin);
                }
                return _loginCommand;
            }
        }

        private bool CanLogin
        {
            get
            {
                this.Validate();
                return this.IsValid;
            }
        }
        #endregion region

        #region methods
        //these validation rules are also enforced in the database users table
        public void Validate()
        {
            if (this.User.Username == null || this.User.Username == string.Empty || string.IsNullOrWhiteSpace(this.User.Username))
            {
                this.IsValid = false;
                this.ShowValidationErrorInUI = true;
                this.ValidationErrorMessage = USERNAME_REQUIRED_ERROR_MESSAGE;
            }
            else if (this.User.Username!.Length > 50)
            {
                this.IsValid = false;
                this.ShowValidationErrorInUI = true;
                this.ValidationErrorMessage = USERNAME_MAX_LENGTH_ERROR_MESSAGE;
            }
            else
            {
                this.IsValid = true;
                this.ShowValidationErrorInUI = false;
                this.ValidationErrorMessage = null;
            }
        }

        public void Login()
        {
            this.Validate();
            if (this.IsValid)
            {
                this.LoggedInUser = _db.Users.FirstOrDefault(u => u.Username == this.User.Username)!; //usernames are unique, so ok to search by username
                if (!(PasswordHasher.IsPasswordValid(this.SecurePassword.ToString()!, this.LoggedInUser.PasswordHash)))
                    this.LoggedInUser = null!;
                this.User = null!;
            }
        }
        #endregion
    }
}
