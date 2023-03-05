using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Data.User _loggedInUser = null!;
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
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(IsUserLoggedIn)));
                    this.PropertyChanged!(this, new PropertyChangedEventArgs(nameof(IsNoUserLoggedIn)));
                }
            }
        }

        public bool IsUserLoggedIn
        {
            get => this.LoggedInUser != null && this.LoggedInUser is Data.User;
        }

        public bool IsNoUserLoggedIn
        {
            get => this.LoggedInUser == null;
        }
        #endregion
    }
}
