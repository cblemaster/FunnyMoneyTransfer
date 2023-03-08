using System.Windows.Controls;

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
            ListUsersViewModel listUsersContext = (ListUsersViewModel)this.DataContext;
            LoggedInUserViewModel loggedInUserContext = (((((App.Current.MainWindow as MainWindow)!).loggedInUserView).DataContext) as LoggedInUserViewModel)!;

            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Data.Models.User selectedUser)
            {
                foreach (Data.Models.User user in listUsersContext.Users)
                {
                    user.ShowSendAndRequestButtons = user.Id == selectedUser.Id && user.Id != loggedInUserContext.LoggedInUser.Id;
                }
                this.lvUsers.Items.Refresh();
            }
        }
    }
}
