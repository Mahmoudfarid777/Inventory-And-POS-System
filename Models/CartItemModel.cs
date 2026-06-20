

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StoreApp.Models
{
    public class CartItemModel : INotifyPropertyChanged
    {
        private int _quantity;

        public int     P_ID        { get; set; }
        public string  ProductName { get; set; } = "";
        public decimal UnitPrice   { get; set; }

        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); OnPropertyChanged(nameof(LineTotal)); }
        }

        public decimal LineTotal => UnitPrice * Quantity;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
