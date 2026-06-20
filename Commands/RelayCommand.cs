

using System;
using System.Windows.Input;

namespace StoreApp.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?>     _execute;
        private readonly Func<object?,bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?,bool>? canExecute = null)
        {
            _execute    = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? p) => _canExecute == null || _canExecute(p);
        public void Execute(object? p)    => _execute(p);

        // WPF يستدعي CanExecute تلقائياً عند أي حدث UI
        // فتتحدث حالة تفعيل/تعطيل الأزرار بدون كود إضافي
        public event EventHandler? CanExecuteChanged
        {
            add    => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
