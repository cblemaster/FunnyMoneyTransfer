using System.Windows;
using System.Windows.Controls;

namespace FunnyMoneyTransfer.UI.WPF.User
{
    /// <summary>
    /// Interaction logic for LoginUserView.xaml
    /// </summary>
    public partial class LoginUserView : UserControl
    {
        public LoginUserView() => InitializeComponent();

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword;
        }
    }
}
