

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StoreApp.Models
{
    public class ProductModel : INotifyPropertyChanged
    {
        private decimal _price;
        private int     _stockQuantity;

        public int    P_ID              { get; set; }
        public string P_Name            { get; set; } = "";
        public string SKU               { get; set; } = "";
        public int    LowStockThreshold { get; set; } = 5;

        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                _stockQuantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLowStock)); // يُحدّث لون الصف فوراً
            }
        }

        public bool IsLowStock => StockQuantity <= LowStockThreshold;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
