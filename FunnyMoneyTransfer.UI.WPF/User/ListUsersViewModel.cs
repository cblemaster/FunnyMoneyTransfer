using FunnyMoneyTransfer.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FunnyMoneyTransfer.UI.WPF.User
{
    public class ListUsersViewModel : INotifyPropertyChanged
    {
        #region ctor
        public ListUsersViewModel() =>
            this.Users = new ObservableCollection<Data.User>(_db.Users.OrderBy(u => u.Username).ToList());
        #endregion

        #region fields
        private readonly FunnyMoneyTransferContext _db = new();
        private ObservableCollection<Data.User> _users = null!;
        #endregion

        #region events
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        #endregion

        #region properties
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
        #endregion
    }
}
