
using System.Windows;
using StoreApp.ViewModels;

namespace StoreApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow() => InitializeComponent();

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.LoginCommand.Execute(PwdBox.Password);
            }
        }
    }
}
