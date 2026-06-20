

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using StoreApp.Commands;
using StoreApp.Models;
using StoreApp.Repositories;
using StoreApp.Views;

namespace StoreApp.ViewModels
{
    public class InventoryViewModel : BaseViewModel
    {
        private readonly InventoryRepository _repo = new();
        private ProductModel? _selected;
        private string        _search = "";

        public ObservableCollection<ProductModel> Products { get; } = new();

        public ProductModel? SelectedProduct
        {
            get => _selected;
            set { _selected = value; OnPropertyChanged(); }
        }

        // كل حرف يكتبه المستخدم يُشغّل بحثاً فورياً
        public string SearchText
        {
            get => _search;
            set { _search = value; OnPropertyChanged(); LoadProducts(); }
        }

        public ICommand AddCommand    { get; }
        public ICommand EditCommand   { get; }
        public ICommand DeleteCommand { get; }

        public InventoryViewModel()
        {
            AddCommand    = new RelayCommand(_ => Add());
            EditCommand   = new RelayCommand(_ => Edit(),   _ => SelectedProduct != null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedProduct != null);
            LoadProducts();
        }

        private void LoadProducts()
        {
            Products.Clear();
            var items = string.IsNullOrWhiteSpace(SearchText)
                ? _repo.GetAll() : _repo.Search(SearchText);
            foreach (var p in items) Products.Add(p);
        }

        private void Add()
        {
            var dlg = new AddEditProductDialog(new ProductModel());
            if (dlg.ShowDialog() == true) { _repo.Add(dlg.Product); LoadProducts(); }
        }

        private void Edit()
        {
            // نمرر نسخة (Clone) حتى لا يتأثر الجدول لو ألغى المستخدم
            var copy = Clone(SelectedProduct!);
            var dlg  = new AddEditProductDialog(copy);
            if (dlg.ShowDialog() == true) { _repo.Update(dlg.Product); LoadProducts(); }
        }

        private void Delete()
        {
            var r = MessageBox.Show(
                $"هل تريد حذف المنتج '{SelectedProduct!.P_Name}'؟",
                "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (r == MessageBoxResult.Yes)
            {
                _repo.Delete(SelectedProduct.P_ID);
                LoadProducts();
            }
        }

        private static ProductModel Clone(ProductModel p) => new()
        {
            P_ID = p.P_ID, P_Name = p.P_Name, SKU = p.SKU,
            Price = p.Price, StockQuantity = p.StockQuantity,
            LowStockThreshold = p.LowStockThreshold
        };
    }
}
