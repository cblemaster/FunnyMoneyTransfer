using System.Windows;
using System.Windows.Controls;

namespace FunnyMoneyTransfer.UI.WPF.User
{
    /// <summary>
    /// Interaction logic for RegisterUserView.xaml
    /// </summary>
    public partial class RegisterUserView : UserControl
    {
        public RegisterUserView() => InitializeComponent();

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword;
        }
    }
}
