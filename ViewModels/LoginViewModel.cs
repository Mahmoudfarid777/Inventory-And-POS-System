


using System.Windows;
using System.Windows.Input;
using StoreApp.Commands;
using StoreApp.Repositories;
using StoreApp.Views;

namespace StoreApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly UserRepository _repo = new();
        private string _username = "";
        private string _error = "";

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            // p = كلمة السر القادمة من CommandParameter
            LoginCommand = new RelayCommand(p => DoLogin(p as string));
        }

        private void DoLogin(string? password)
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "أدخل اسم المستخدم وكلمة السر.";
                return;
            }

            var result = _repo.Authenticate(Username, password);
            if (result == null)
            {
                ErrorMessage = "بيانات الدخول غير صحيحة.";
                return;
            }

            // حفظ بيانات الجلسة
            AppSettings.CurrentUserId = result.Value.UserId;
            AppSettings.CurrentUsername = Username;
            AppSettings.CurrentRole = result.Value.Role;

            // ✅ الحل الصح: افتح MainWindow الأول، وبعدين اقفل LoginWindow
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // إغلاق LoginWindow بالظبط مش أي نافذة تانية
            foreach (Window w in Application.Current.Windows)
            {
                if (w is LoginWindow)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}

