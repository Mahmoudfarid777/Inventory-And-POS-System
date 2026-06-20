

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StoreApp.Commands;
using StoreApp.Models;
using StoreApp.Repositories;
using StoreApp.Views;

namespace StoreApp.ViewModels
{
    public class SalesViewModel : BaseViewModel
    {
        private readonly InventoryRepository   _invRepo = new();
        private readonly TransactionRepository _txRepo  = new();

        private string        _searchText = "";
        private ProductModel? _selectedResult;
        private bool          _isSaving;

        public ObservableCollection<ProductModel>  SearchResults { get; } = new();
        public ObservableCollection<CartItemModel> CartItems     { get; } = new();

        public bool IsSaving
        {
            get => _isSaving;
            set { _isSaving = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); SearchProducts(); }
        }

        public ProductModel? SelectedSearchResult
        {
            get => _selectedResult;
            set { _selectedResult = value; OnPropertyChanged(); }
        }

        // ===== الإجماليات (Calculated Fields) =====
        public decimal Subtotal  => CartItems.Sum(i => i.LineTotal);
        public decimal Tax       => Math.Round(Subtotal * AppSettings.TaxRate, 2);
        public decimal Total     => Subtotal + Tax;
        public int     ItemCount => CartItems.Sum(i => i.Quantity);

        public ICommand AddToCartCommand  { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand CheckoutCommand   { get; }
        public ICommand ClearCartCommand  { get; }

        public SalesViewModel()
        {
            // === العضو 3 ===
            AddToCartCommand  = new RelayCommand(_ => AddToCart(),
                                    _ => SelectedSearchResult != null);
            RemoveItemCommand = new RelayCommand(p => RemoveItem(p as CartItemModel));
            ClearCartCommand  = new RelayCommand(_ => ClearCart(), _ => CartItems.Any());

            // === العضو 4 ===
            CheckoutCommand = new RelayCommand(
                async _ => await Checkout(),
                _ => CartItems.Any() && !IsSaving);
        }

        // ----- العضو 3: البحث والسلة -----

        private void SearchProducts()
        {
            SearchResults.Clear();
            if (string.IsNullOrWhiteSpace(SearchText)) return;
            foreach (var p in _invRepo.Search(SearchText)) SearchResults.Add(p);
        }

        private void AddToCart()
        {
            var product = SelectedSearchResult!;

            if (product.StockQuantity <= 0)
            {
                MessageBox.Show("هذا المنتج غير متوفر في المخزون.",
                    "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var existing = CartItems.FirstOrDefault(i => i.P_ID == product.P_ID);
            if (existing != null)
            {
                if (existing.Quantity >= product.StockQuantity)
                {
                    MessageBox.Show("لا يمكن إضافة أكثر من الكمية المتاحة.",
                        "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                existing.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItemModel {
                    P_ID        = product.P_ID,
                    ProductName = product.P_Name,
                    UnitPrice   = product.Price,
                    Quantity    = 1
                });
            }

            NotifyTotals();
        }

        private void RemoveItem(CartItemModel? item)
        {
            if (item == null) return;
            CartItems.Remove(item);
            NotifyTotals();
        }

        private void ClearCart()
        {
            CartItems.Clear();
            NotifyTotals();
        }

        // ----- العضو 4: الدفع (Checkout) -----

        private async System.Threading.Tasks.Task Checkout()
        {
            var dlg = new CheckoutConfirmDialog(Subtotal, Tax, Total);
            if (dlg.ShowDialog() != true) return;

            IsSaving = true;
            try
            {
                var tx = new TransactionModel {
                    TransactionDate = DateTime.Now,
                    Subtotal        = Subtotal,
                    TaxAmount       = Tax,
                    TotalAmount     = Total
                };

                await _txRepo.SaveAsync(tx, CartItems.ToList());

                MessageBox.Show($"✅ تمت العملية بنجاح!\nالإجمالي: {Total:C2}",
                    "تم الدفع", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearCart();
                SearchResults.Clear();
                SearchText = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ أثناء الحفظ:\n{ex.Message}",
                    "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsSaving = false;
            }
        }

        private void NotifyTotals()
        {
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(ItemCount));
        }
    }
}
