

using System.Collections.ObjectModel;
using System.Windows.Input;
using StoreApp.Commands;
using StoreApp.Models;
using StoreApp.Repositories;

namespace StoreApp.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        private readonly TransactionRepository _repo = new();
        private TransactionModel? _selected;

        public ObservableCollection<TransactionModel>  Transactions  { get; } = new();
        public ObservableCollection<CartItemModel>     SelectedItems { get; } = new();

        public TransactionModel? SelectedTransaction
        {
            get => _selected;
            set { _selected = value; OnPropertyChanged(); LoadItems(); }
        }

        public ICommand RefreshCommand { get; }

        public HistoryViewModel()
        {
            RefreshCommand = new RelayCommand(_ => Load());
            Load();
        }

        private void Load()
        {
            Transactions.Clear();
            SelectedItems.Clear();
            foreach (var t in _repo.GetAll()) Transactions.Add(t);
        }

        private void LoadItems()
        {
            SelectedItems.Clear();
            if (_selected == null) return;
            foreach (var i in _repo.GetItems(_selected.T_ID))
                SelectedItems.Add(i);
        }
    }
}
