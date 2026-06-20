

using System.Windows;
using StoreApp.Models;

namespace StoreApp.Views
{
    public partial class AddEditProductDialog : Window
    {
 
        public ProductModel Product { get; private set; }

        public AddEditProductDialog(ProductModel product)
        {
            InitializeComponent();
            Product     = product;
            DataContext = product; 
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Product.P_Name))
            {
                MessageBox.Show("اسم المنتج مطلوب.", "بيانات ناقصة",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Product.SKU))
            {
                MessageBox.Show("رقم SKU مطلوب.", "بيانات ناقصة",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Product.Price < 0)
            {
                MessageBox.Show("السعر لا يمكن أن يكون سالباً.", "قيمة غير صحيحة",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Product.StockQuantity < 0)
            {
                MessageBox.Show("الكمية لا يمكن أن تكون سالبة.", "قيمة غير صحيحة",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true; 
        }
    }
}
