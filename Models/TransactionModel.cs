

using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public class TransactionModel
    {
        public int      T_ID            { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal  Subtotal        { get; set; }
        public decimal  TaxAmount       { get; set; }
        public decimal  TotalAmount     { get; set; }
        public string   CashierName     { get; set; } = "";

        public List<CartItemModel> Items { get; set; } = new();
    }
}
