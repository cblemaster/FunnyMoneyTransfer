using FunnyMoneyTransfer.UI.WPF.User;
using System.Windows;
using System.Windows.Controls;

namespace FunnyMoneyTransfer.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void lvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Data.User selectedUser)
            {
                MainWindowViewModel mainContext = ((App.Current.MainWindow.DataContext) as MainWindowViewModel)!;
                if (mainContext is MainWindowViewModel)
                {
                    foreach (Data.User user in mainContext.Users)
                    {
                        user.ShowSendAndRequestButtons = user.Id == selectedUser.Id && user.Id != mainContext.LoggedInUser.Id;
                    }
                    this.lvUsers.Items.Refresh();
                }                
            }
        }
    }
}
