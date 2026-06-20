

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using StoreApp.Models;

namespace StoreApp.Repositories
{
    public class TransactionRepository
    {
        private readonly string _conn = AppSettings.ConnectionString;

        public async Task SaveAsync(TransactionModel t, List<CartItemModel> items)
        {
            await Task.Run(() =>
            {
                using var conn = new SqlConnection(_conn);
                conn.Open();
                using var tx = conn.BeginTransaction();
                try
                {
                    // 1) رأس الفاتورة
                    const string headSql = @"
                        INSERT INTO Transactions(Subtotal,TaxAmount,TotalAmount,TransactionDate,UserId)
                        VALUES(@sub,@tax,@tot,@d,@uid);
                        SELECT SCOPE_IDENTITY();";

                    using var headCmd = new SqlCommand(headSql, conn, tx);
                    headCmd.Parameters.AddWithValue("@sub", t.Subtotal);
                    headCmd.Parameters.AddWithValue("@tax", t.TaxAmount);
                    headCmd.Parameters.AddWithValue("@tot", t.TotalAmount);
                    headCmd.Parameters.AddWithValue("@d",   t.TransactionDate);
                    headCmd.Parameters.AddWithValue("@uid", AppSettings.CurrentUserId);
                    int newId = Convert.ToInt32(headCmd.ExecuteScalar());

                    // 2) كل بند: حفظ + خصم المخزون
                    foreach (var item in items)
                    {
                        const string itemSql = @"
                            INSERT INTO TransactionDetails(Quantity,UnitPrice,ProductName,LineTotal,P_ID,T_ID)
                            VALUES(@q,@up,@pn,@lt,@pid,@tid)";
                        using var itemCmd = new SqlCommand(itemSql, conn, tx);
                        itemCmd.Parameters.AddWithValue("@q",   item.Quantity);
                        itemCmd.Parameters.AddWithValue("@up",  item.UnitPrice);
                        itemCmd.Parameters.AddWithValue("@pn",  item.ProductName);
                        itemCmd.Parameters.AddWithValue("@lt",  item.LineTotal);
                        itemCmd.Parameters.AddWithValue("@pid", item.P_ID);
                        itemCmd.Parameters.AddWithValue("@tid", newId);
                        itemCmd.ExecuteNonQuery();

                        const string deductSql =
                            "UPDATE Products SET StockQuantity=StockQuantity-@q WHERE P_ID=@pid";
                        using var deductCmd = new SqlCommand(deductSql, conn, tx);
                        deductCmd.Parameters.AddWithValue("@q",   item.Quantity);
                        deductCmd.Parameters.AddWithValue("@pid", item.P_ID);
                        deductCmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            });
        }

        // جلب آخر 200 فاتورة
        public List<TransactionModel> GetAll()
        {
            var list = new List<TransactionModel>();
            using var conn = new SqlConnection(_conn); conn.Open();
            const string sql = @"
                SELECT TOP 200 t.T_ID, t.TransactionDate, t.Subtotal,
                       t.TaxAmount, t.TotalAmount, u.Username
                FROM   Transactions t JOIN Users u ON u.UserId = t.UserId
                ORDER  BY t.TransactionDate DESC";
            using var cmd = new SqlCommand(sql, conn);
            using var r   = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new TransactionModel {
                    T_ID            = (int)      r["T_ID"],
                    TransactionDate = (DateTime) r["TransactionDate"],
                    Subtotal        = (decimal)  r["Subtotal"],
                    TaxAmount       = (decimal)  r["TaxAmount"],
                    TotalAmount     = (decimal)  r["TotalAmount"],
                    CashierName     = (string)   r["Username"]
                });
            return list;
        }

        // بنود فاتورة محددة
        public List<CartItemModel> GetItems(int transactionId)
        {
            var list = new List<CartItemModel>();
            using var conn = new SqlConnection(_conn); conn.Open();
            const string sql = @"
                SELECT P_ID, ProductName, UnitPrice, Quantity
                FROM   TransactionDetails WHERE T_ID=@id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", transactionId);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new CartItemModel {
                    P_ID        = (int)     r["P_ID"],
                    ProductName = (string)  r["ProductName"],
                    UnitPrice   = (decimal) r["UnitPrice"],
                    Quantity    = (int)     r["Quantity"]
                });
            return list;
        }
    }
}
