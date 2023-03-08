using System.ComponentModel;

namespace FunnyMoneyTransfer.UI.WPF.User
{
    public class LoggedInUserViewModel : INotifyPropertyChanged
    {
        #region ctor
        public LoggedInUserViewModel()
        {

        }
        #endregion

        #region fields
        private Data.Models.User _loggedInUser = null!;
        #endregion

        #region events
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        #endregion

        #region properties
        public Data.Models.User LoggedInUser
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

        public bool IsUserLoggedIn
        {
            get => this.LoggedInUser != null && this.LoggedInUser is Data.Models.User;
        }

        public bool IsNoUserLoggedIn
        {
            get => this.LoggedInUser == null;
        }
        #endregion
    }
}
