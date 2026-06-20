

using System.Windows;

namespace StoreApp.Views
{
    public partial class CheckoutConfirmDialog : Window
    {
        public CheckoutConfirmDialog(decimal subtotal, decimal tax, decimal total)
        {
            InitializeComponent();
            SubtotalText.Text = subtotal.ToString("C2");
            TaxText.Text      = tax.ToString("C2");
            TotalText.Text    = total.ToString("C2");
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
            => DialogResult = true;
    }
}
