

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StoreApp.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // [CallerMemberName] يملأ اسم الخاصية تلقائياً
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
