

using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using StoreApp.Models;

namespace StoreApp.Repositories
{
    public class InventoryRepository
    {
        private readonly string _conn = AppSettings.ConnectionString;

        // READ: كل المنتجات مرتبة بالاسم
        public List<ProductModel> GetAll() => Query(null);

        // READ: بحث بالاسم أو SKU
        public List<ProductModel> Search(string kw) => Query(kw);

        private List<ProductModel> Query(string? kw)
        {
            var list = new List<ProductModel>();
            using var conn = new SqlConnection(_conn);
            conn.Open();

            var sql = kw == null
                ? "SELECT * FROM Products ORDER BY P_Name"
                : "SELECT * FROM Products WHERE P_Name LIKE @kw OR SKU LIKE @kw ORDER BY P_Name";

            using var cmd = new SqlCommand(sql, conn);
            if (kw != null) cmd.Parameters.AddWithValue("@kw", $"%{kw}%");

            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        // CREATE
        public void Add(ProductModel p)
        {
            using var conn = new SqlConnection(_conn); conn.Open();
            const string sql = @"INSERT INTO Products(P_Name,Price,SKU,StockQuantity,LowStockThreshold)
                                  VALUES(@n,@p,@s,@q,@t)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", p.P_Name);
            cmd.Parameters.AddWithValue("@p", p.Price);
            cmd.Parameters.AddWithValue("@s", p.SKU);
            cmd.Parameters.AddWithValue("@q", p.StockQuantity);
            cmd.Parameters.AddWithValue("@t", p.LowStockThreshold);
            cmd.ExecuteNonQuery();
        }

        // UPDATE
        public void Update(ProductModel p)
        {
            using var conn = new SqlConnection(_conn); conn.Open();
            const string sql = @"UPDATE Products SET P_Name=@n,Price=@p,SKU=@s,
                                  StockQuantity=@q,LowStockThreshold=@t WHERE P_ID=@id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n",  p.P_Name);
            cmd.Parameters.AddWithValue("@p",  p.Price);
            cmd.Parameters.AddWithValue("@s",  p.SKU);
            cmd.Parameters.AddWithValue("@q",  p.StockQuantity);
            cmd.Parameters.AddWithValue("@t",  p.LowStockThreshold);
            cmd.Parameters.AddWithValue("@id", p.P_ID);
            cmd.ExecuteNonQuery();
        }

        // DELETE
        public void Delete(int id)
        {
            using var conn = new SqlConnection(_conn); conn.Open();
            using var cmd = new SqlCommand("DELETE FROM Products WHERE P_ID=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        // Helper: تحويل صف DataReader إلى ProductModel
        private static ProductModel Map(SqlDataReader r) => new()
        {
            P_ID              = (int)     r["P_ID"],
            P_Name            = (string)  r["P_Name"],
            Price             = (decimal) r["Price"],
            SKU               = (string)  r["SKU"],
            StockQuantity     = (int)     r["StockQuantity"],
            LowStockThreshold = (int)     r["LowStockThreshold"]
        };
    }
}
