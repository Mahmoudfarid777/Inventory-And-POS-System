# 🏪 Store Management & Transaction Processing System

<div align="center">
![WPF](https://img.shields.io/badge/WPF-.NET%2010-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Latest-239120?style=for-the-badge&logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019%2B-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![MVVM](https://img.shields.io/badge/Architecture-MVVM-blue?style=for-the-badge)
![License](https://img.shields.io/badge/License-Academic-orange?style=for-the-badge)

**نظام متكامل لإدارة المخزون ومعالجة المبيعات**
مبني بـ C# / WPF مع معمارية MVVM صارمة



</div>

---

## 📋 نظرة عامة

**Store Management & TPS** هو تطبيق سطح مكتب (Desktop Application) مزدوج الغرض:

- 🗄️ **Back-Office** — إدارة المخزون، التسعير، التقارير
- 🛒 **Front-End POS** — نقطة بيع لمعالجة معاملات العملاء بشكل لحظي

يُبرهن المشروع على تطبيق **معمارية MVVM** الكاملة في بيئة WPF، مع التكامل مع قاعدة بيانات SQL Server خارجية وتطبيق مفاهيم **Async/Await** و**SQL Transactions** لضمان سلامة البيانات.

---

## ✨ المميزات الرئيسية

### 📦 Module A — إدارة المخزون
- ✅ عمليات **CRUD** كاملة على المنتجات (إضافة، عرض، تعديل، حذف)
- ✅ **تنبيهات بصرية تلقائية** (لون أحمر) عند وصول الكمية للحد الأدنى `LowStockThreshold`
- ✅ **بحث فوري** بالاسم أو رقم SKU مع كل حرف يُكتب
- ✅ Input Validation لمنع إدخال بيانات غير صحيحة (سعر سالب، كمية سالبة...)

### 🛒 Module B — نقطة البيع (POS)
- ✅ **سلة مشتريات ديناميكية** مع إضافة وحذف المنتجات
- ✅ حساب **Subtotal / TAX (14%) / Total** بشكل لحظي عند كل تغيير
- ✅ **Checkout** يحفظ الفاتورة ويخصم المخزون في عملية واحدة (SQL Transaction)
- ✅ نافذة تأكيد قبل إتمام الدفع النهائي

### 📋 Module C — السجل والمراجعة
- ✅ سجل قابل للبحث لجميع الفواتير السابقة
- ✅ عرض تفاصيل كل فاتورة بنقرة واحدة (الأصناف، الكميات، الأسعار)

### 🔐 نظام تسجيل الدخول
- ✅ نافذة Login منفصلة بمستويين: **Admin** و **Cashier**
- ✅ التحقق من البيانات عبر قاعدة البيانات مباشرة

---

## 🏗️ معمارية المشروع (MVVM)

```
┌──────────────────────────────────────────────────────┐
│                      VIEW (.xaml)                    │
│   LoginWindow │ MainWindow │ InventoryView │ SalesView│
│                HistoryView │ Dialogs                  │
└──────────────────────┬───────────────────────────────┘
                       │ Data Binding + Commands
┌──────────────────────▼───────────────────────────────┐
│                   VIEWMODEL (.cs)                     │
│  LoginVM │ MainVM │ InventoryVM │ SalesVM │ HistoryVM │
│              BaseViewModel + RelayCommand             │
└──────────────────────┬───────────────────────────────┘
                       │ Calls
┌──────────────────────▼───────────────────────────────┐
│                 REPOSITORY (.cs)                      │
│     UserRepo │ InventoryRepo │ TransactionRepo        │
└──────────────────────┬───────────────────────────────┘
                       │ SQL Queries
┌──────────────────────▼───────────────────────────────┐
│              SQL SERVER DATABASE                      │
│   Users │ Products │ Transactions │ TransactionDetails│
└──────────────────────────────────────────────────────┘
```

### مبادئ MVVM المطبّقة

| المبدأ | التطبيق |
|---|---|
| **Zero Code-Behind** | ملفات `.xaml.cs` تحتوي `InitializeComponent()` فقط |
| **ICommand / RelayCommand** | كل زر مربوط بـ Command في ViewModel |
| **INotifyPropertyChanged** | `BaseViewModel` يوفّرها لكل الـ ViewModels |
| **ObservableCollection** | كل القوائم تتحدث تلقائياً عند التغيير |
| **DataTemplate** | MainWindow يختار الـ View الصحيح تلقائياً |

---

## 🗄️ تصميم قاعدة البيانات

```sql
Users               Products
─────────           ──────────────────
UserId (PK)         P_ID (PK)
Username            P_Name
Password            SKU
Role                Price
                    StockQuantity
                    LowStockThreshold
        │                   │
        │                   │
        ▼                   ▼
Transactions        TransactionDetails
────────────────    ──────────────────
T_ID (PK)           TD_ID (PK)
Subtotal            T_ID (FK)
TaxAmount           P_ID (FK)
TotalAmount         ProductName
TransactionDate     UnitPrice
UserId (FK)         Quantity
                    LineTotal
```

---

## 🛠️ التقنيات المستخدمة

| التقنية | الاستخدام |
|---|---|
| **C# / .NET 10** | لغة البرمجة الرئيسية |
| **WPF (Windows Presentation Foundation)** | إطار بناء واجهة المستخدم |
| **MVVM Architecture** | نمط معماري لفصل المنطق عن الواجهة |
| **SQL Server** | قاعدة البيانات الخارجية |
| **Microsoft.Data.SqlClient** | مكتبة الاتصال بـ SQL Server |
| **Async / Await** | لحفظ الفواتير دون تجميد الواجهة |
| **SQL Transactions** | لضمان سلامة البيانات عند الـ Checkout |
| **Resource Dictionaries** | لتوحيد التصميم في كل المشروع |

---

## 📁 هيكل الملفات

```
StoreApp/
│
├── 📄 StoreApp.csproj
├── 📄 App.xaml / App.xaml.cs
├── 📄 AppSettings.cs          ← ⚙ عدّل ConnectionString هنا
├── 📄 CreateDatabase.sql      ← نفّذه في SSMS أولاً
│
├── 📂 Commands/
│   └── RelayCommand.cs
│
├── 📂 Models/
│   ├── ProductModel.cs
│   ├── CartItemModel.cs
│   └── TransactionModel.cs
│
├── 📂 Repositories/
│   ├── UserRepository.cs
│   ├── InventoryRepository.cs
│   └── TransactionRepository.cs
│
├── 📂 ViewModels/
│   ├── BaseViewModel.cs
│   ├── LoginViewModel.cs
│   ├── MainViewModel.cs
│   ├── InventoryViewModel.cs
│   ├── SalesViewModel.cs
│   └── HistoryViewModel.cs
│
├── 📂 Views/
│   ├── LoginWindow.xaml(.cs)
│   ├── MainWindow.xaml(.cs)
│   ├── InventoryView.xaml(.cs)
│   ├── AddEditProductDialog.xaml(.cs)
│   ├── SalesView.xaml(.cs)
│   ├── CheckoutConfirmDialog.xaml(.cs)
│   └── HistoryView.xaml(.cs)
│
└── 📂 Resources/
    └── Styles.xaml
```

---

## 🚀 خطوات التشغيل

### المتطلبات
- Visual Studio 2022 أو أحدث مع workload **.NET desktop development**
- SQL Server أو SQL Server Express
- .NET 10 SDK
- SQL Server Management Studio (SSMS)

### 1️⃣ إنشاء قاعدة البيانات
```
1. افتح SQL Server Management Studio (SSMS)
2. افتح ملف CreateDatabase.sql
3. اضغط F5 لتنفيذه
```



```csharp
// SQL Server Express:
public static string ConnectionString =
    @"Server=.\SQLEXPRESS;Database=Store_Management;Integrated Security=True;";

// LocalDB:
public static string ConnectionString =
    @"Server=(localdb)\MSSQLLocalDB;Database=Store_Management;Integrated Security=True;";
```

### 2 تشغيل المشروع
```
1. افتح StoreApp.csproj في Visual Studio
2. انتظر استعادة NuGet Packages
3. اضغط F5
```

