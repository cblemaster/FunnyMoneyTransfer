using System.Windows.Controls;
using System.Collections;
using System.Linq;

namespace FunnyMoneyTransfer.UI.WPF.User
{
    /// <summary>
    /// Interaction logic for ListUsersView.xaml
    /// </summary>
    public partial class ListUsersView : UserControl
    {
        public ListUsersView() => InitializeComponent();

        private void lvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListUsersViewModel vm = (ListUsersViewModel)this.DataContext;
            MainWindowViewModel mwvm = ((MainWindowViewModel)(App.Current.MainWindow).DataContext);

            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Data.User selectedUser)
            {
                foreach (Data.User user in vm.Users)
                {
                    if (user.Id == selectedUser.Id && user.Id != mwvm.LoggedInUser.Id)
                        user.ShowSendAndRequestButtons = true;
                    else
                        user.ShowSendAndRequestButtons = false;
                }
            }
        }
    }
}
