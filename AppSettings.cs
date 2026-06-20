

namespace StoreApp
{
    public static class AppSettings
    {
      

        public static string ConnectionString =
    @"Server=DESKTOP-H3J7DUP\SQLEXPRESS;Database=Store_Management;Integrated Security=True;TrustServerCertificate=True;";



        // نسبة الضريبة (VAT) — تُستخدم في حسابات السلة
        public const decimal TaxRate = 0.14m;

        // بيانات الجلسة الحالية — تُملأ بعد تسجيل الدخول
        public static int    CurrentUserId   { get; set; }
        public static string CurrentUsername { get; set; } = "";
        public static string CurrentRole     { get; set; } = "";
    }
}
