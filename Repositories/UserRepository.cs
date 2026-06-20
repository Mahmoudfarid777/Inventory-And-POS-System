

using Microsoft.Data.SqlClient;

namespace StoreApp.Repositories
{
    public class UserRepository
    {
        private readonly string _conn = AppSettings.ConnectionString;

        // يُعيد (UserId, Role) إذا صحّت البيانات، أو null إذا لم تصح
        public (int UserId, string Role)? Authenticate(string username, string password)
        {
            using var conn = new SqlConnection(_conn);
            conn.Open();

            const string sql = "SELECT UserId, Role FROM Users WHERE Username=@u AND Password=@p";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);

            using var r = cmd.ExecuteReader();
            if (r.Read())
                return ((int)r["UserId"], (string)r["Role"]);

            return null;
        }
    }
}
