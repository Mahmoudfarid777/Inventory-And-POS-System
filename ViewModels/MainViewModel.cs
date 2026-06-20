// 

using System.Windows.Input;
using StoreApp.Commands;

namespace StoreApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
     
        private readonly InventoryViewModel _invVM = new();
        private readonly SalesViewModel     _salVM = new();
        private readonly HistoryViewModel   _hisVM = new();

        private BaseViewModel _currentView;

        public BaseViewModel CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public string WelcomeText => $"مرحباً، {AppSettings.CurrentUsername}";

        public ICommand ShowSalesCommand     { get; }
        public ICommand ShowInventoryCommand { get; }
        public ICommand ShowHistoryCommand   { get; }

        public MainViewModel()
        {
            ShowSalesCommand     = new RelayCommand(_ => CurrentView = _salVM);
            ShowInventoryCommand = new RelayCommand(_ => CurrentView = _invVM);
            ShowHistoryCommand   = new RelayCommand(_ => CurrentView = _hisVM);

            _currentView = _salVM; // الشاشة الافتراضية = نقطة البيع
        }
    }
}
