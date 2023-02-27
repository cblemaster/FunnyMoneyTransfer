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
        public RegisterUserViewModel() => this.User = new();
        #endregion

        #region consts
        internal const string USERNAME_REQUIRED_ERROR_MESSAGE = "Username is required";
        internal const string USERNAME_MAX_LENGTH_ERROR_MESSAGE = "Max length for Username is 50";
        internal const string USERNAME_UNIQUE_ERROR_MESSAGE = "Username already exists";
        #endregion

        #region fields
        private Data.User _user = null!;
        private readonly FunnyMoneyTransferContext _db = new();
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

        private bool CanRegister => this.ValidationResult.IsValid;
        #endregion

        #region methods
        public void Register()
        {
            if (this.ValidationResult.IsValid)
            {
                this.User.PasswordHash = PasswordHasher.GetPasswordHash(this.SecurePassword.ToString()!);
                
                DateTime createDate = DateTime.Now;                
                this.User.CreateDate = createDate;
                
                this.User.Account = new()
                    { StartingBalance = new()
                        { Amount = StartingBalance.NEW_ACCOUNT_STARTING_BALANCE,
                        CreateDate = createDate },
                    CreateDate = createDate };
                _db.Users.Add(this.User);
                _db.SaveChanges();
            }
        }        

        //these validation rules are also enforced in the database users table
        public UserValidationResult ValidationResult
        {
            get
            {
                if (this.User.Username == null || this.User.Username == string.Empty || string.IsNullOrWhiteSpace(this.User.Username))
                    return new() { IsValid = false, ShowValidationErrorInUI = true, ErrorMessage = USERNAME_REQUIRED_ERROR_MESSAGE };
                if (this.User.Username.Length > 50)
                    return new() { IsValid = false, ShowValidationErrorInUI = true, ErrorMessage = USERNAME_MAX_LENGTH_ERROR_MESSAGE };
                if (_db.Users.Select(u => u.Username).ToList().Contains<string>(this.User.Username))
                    return new() { IsValid = false, ShowValidationErrorInUI = true, ErrorMessage = USERNAME_UNIQUE_ERROR_MESSAGE };
                return new() { IsValid = true, ShowValidationErrorInUI = false, ErrorMessage = null! };
            }
        }
        #endregion
    }
}
