

CREATE DATABASE Store_Management;
GO
USE Store_Management;
GO

-- ============================================================
-- جدول Users — تسجيل الدخول (Login Window)
-- ============================================================
CREATE TABLE Users
(
    UserId   INT           PRIMARY KEY IDENTITY,
    Username NVARCHAR(50)  NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Role     NVARCHAR(20)  NOT NULL DEFAULT 'Cashier'   -- Admin / Cashier
);
GO

-- ============================================================
-- جدول Products — المخزون (Module A)
-- ============================================================
CREATE TABLE Products
(
    P_ID              INT           PRIMARY KEY IDENTITY,
    P_Name            VARCHAR(50)   NOT NULL,
    Price             DECIMAL(18,2) NOT NULL CHECK (Price >= 0),
    SKU               VARCHAR(50)   NOT NULL UNIQUE,
    StockQuantity     INT           NOT NULL CHECK (StockQuantity >= 0),
    LowStockThreshold INT           NOT NULL DEFAULT 5  -- لتنبيهات Low Stock
);
GO

-- ============================================================
-- جدول Transactions — رأس الفاتورة (Module B)
-- ============================================================
CREATE TABLE Transactions
(
    T_ID            INT           PRIMARY KEY IDENTITY,
    Subtotal        DECIMAL(18,2) NOT NULL,
    TaxAmount       DECIMAL(18,2) NOT NULL,
    TotalAmount     DECIMAL(18,2) NOT NULL,
    TransactionDate DATETIME      NOT NULL DEFAULT GETDATE(),
    UserId          INT           NOT NULL REFERENCES Users(UserId)
);
GO

-- ============================================================
-- جدول TransactionDetails — بنود كل فاتورة (Module C)
-- ============================================================
CREATE TABLE TransactionDetails
(
    TD_ID       INT           PRIMARY KEY IDENTITY,
    Quantity    INT           NOT NULL CHECK (Quantity >= 0),
    UnitPrice   DECIMAL(18,2) NOT NULL,
    ProductName VARCHAR(50)   NOT NULL,   -- تبقى مخزّنة حتى لو حُذف المنتج
    LineTotal   DECIMAL(18,2) NOT NULL,   -- UnitPrice * Quantity
    P_ID        INT           FOREIGN KEY REFERENCES Products(P_ID),
    T_ID        INT           FOREIGN KEY REFERENCES Transactions(T_ID)
);
GO

-- ============================================================
-- بيانات تجريبية
-- ============================================================
INSERT INTO Users (Username, Password) VALUES
('admin',   'admin123');


INSERT INTO Products (P_Name, Price, SKU, StockQuantity, LowStockThreshold) VALUES
('لابتوب Dell XPS',     4500.00, 'DELL-001', 12, 5),
('ماوس Logitech MX3',    250.00, 'LOG-002',   3, 5),   -- Low Stock
('شاشة Samsung 27"',    1800.00, 'SAM-003',  20, 5),
('كيبورد Mechanical',    350.00, 'KB-004',    2, 5),   -- Low Stock
('هيدفون Sony WH-1000',  900.00, 'SONY-005',  8, 5),
('فلاشة USB 64GB',        45.00, 'USB-006',  50, 10);
GO
