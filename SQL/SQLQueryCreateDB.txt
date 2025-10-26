-- Создание базы данных (если не существует)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SaltWholesaleDB')
BEGIN
    CREATE DATABASE SaltWholesaleDB;
END
GO

USE SaltWholesaleDB;
GO

-- 1. Таблица Поставщики (Suppliers)
CREATE TABLE Suppliers (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(255) NOT NULL,
    ContactPerson NVARCHAR(255),
    Phone NVARCHAR(50),
    Email NVARCHAR(255),
    Address NVARCHAR(500)
);
CREATE INDEX IX_Suppliers_CompanyName ON Suppliers(CompanyName);

-- 2. Таблица Товары (Products)
CREATE TABLE Products (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    SupplierID INT NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Category NVARCHAR(100),
    UnitOfMeasure NVARCHAR(50),
    PricePerUnit DECIMAL(10,2) NOT NULL CHECK (PricePerUnit >= 0),
    Description NVARCHAR(MAX),
    CONSTRAINT FK_Products_Suppliers FOREIGN KEY (SupplierID) REFERENCES Suppliers(ID) ON DELETE CASCADE ON UPDATE CASCADE
);
CREATE INDEX IX_Products_SupplierID ON Products(SupplierID);
CREATE INDEX IX_Products_Name ON Products(Name);

-- 3. Таблица Склад (Stock)
CREATE TABLE Stock (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    QuantityOnStock DECIMAL(12,2) NOT NULL CHECK (QuantityOnStock >= 0),
    LastUpdated DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Stock_Products FOREIGN KEY (ProductID) REFERENCES Products(ID) ON DELETE CASCADE ON UPDATE CASCADE
);
CREATE INDEX IX_Stock_ProductID ON Stock(ProductID);
CREATE INDEX IX_Stock_LastUpdated ON Stock(LastUpdated);

-- 4. Таблица Клиенты (Clients)
CREATE TABLE Clients (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    ContactPerson NVARCHAR(255),
    Phone NVARCHAR(50),
    Email NVARCHAR(255),
    Address NVARCHAR(500)
);
CREATE INDEX IX_Clients_FullName ON Clients(FullName);
CREATE INDEX IX_Clients_Email ON Clients(Email);

-- 5. Таблица Сотрудники (Employees)
CREATE TABLE Employees (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    Position NVARCHAR(100),
    Phone NVARCHAR(50),
    Email NVARCHAR(255),
    HireDate DATE NOT NULL DEFAULT GETDATE()
);
CREATE INDEX IX_Employees_FullName ON Employees(FullName);
CREATE INDEX IX_Employees_Position ON Employees(Position);

-- 6. Таблица Авторизация (Authorization)
CREATE TABLE Auth (
    EmployeeID INT PRIMARY KEY,
    Login NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL, -- Хранить хеш пароля, а не сам пароль!
    AccessLevel INT NOT NULL DEFAULT 0 CHECK (AccessLevel >= -1), -- -1 = заблокирован, 0+ = роли
    CONSTRAINT FK_Auth_Employees FOREIGN KEY (EmployeeID) REFERENCES Employees(ID) ON DELETE CASCADE ON UPDATE CASCADE
);
CREATE INDEX IX_Auth_Login ON Auth(Login);
CREATE INDEX IX_Auth_AccessLevel ON Auth(AccessLevel);

-- 7. Таблица Заказы (Orders)
CREATE TABLE Orders (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ClientID INT NOT NULL,
    EmployeeID INT NOT NULL,
    OrderDateTime DATETIME2 NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(12,2) NOT NULL CHECK (TotalAmount >= 0),
    Status NVARCHAR(50) NOT NULL DEFAULT 'Новый',
    CONSTRAINT FK_Orders_Clients FOREIGN KEY (ClientID) REFERENCES Clients(ID) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_Orders_Employees FOREIGN KEY (EmployeeID) REFERENCES Employees(ID) ON DELETE CASCADE ON UPDATE CASCADE
);
CREATE INDEX IX_Orders_ClientID ON Orders(ClientID);
CREATE INDEX IX_Orders_EmployeeID ON Orders(EmployeeID);
CREATE INDEX IX_Orders_OrderDateTime ON Orders(OrderDateTime);
CREATE INDEX IX_Orders_Status ON Orders(Status);

-- 8. Таблица Товары в заказе (OrderItems)
CREATE TABLE OrderItems (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity DECIMAL(12,2) NOT NULL CHECK (Quantity > 0),
    PriceAtOrderTime DECIMAL(10,2) NOT NULL CHECK (PriceAtOrderTime >= 0),
    Subtotal AS (Quantity * PriceAtOrderTime) PERSISTED, -- Вычисляемое поле
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderID) REFERENCES Orders(ID) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductID) REFERENCES Products(ID) ON DELETE NO ACTION ON UPDATE CASCADE -- Не удалять товар при удалении позиции заказа
);
CREATE INDEX IX_OrderItems_OrderID ON OrderItems(OrderID);
CREATE INDEX IX_OrderItems_ProductID ON OrderItems(ProductID);

-- 9. Таблица Услуги (Services)
CREATE TABLE Services (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Cost DECIMAL(10,2) NOT NULL CHECK (Cost >= 0)
);
CREATE INDEX IX_Services_Name ON Services(Name);

-- 10. Таблица Оказанные услуги (ProvidedServices)
CREATE TABLE ProvidedServices (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ClientID INT NOT NULL,
    EmployeeID INT NOT NULL,
    ServiceID INT NOT NULL,
    ServiceDateTime DATETIME2 NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(50) NOT NULL DEFAULT 'В процессе',
    CONSTRAINT FK_ProvidedServices_Clients FOREIGN KEY (ClientID) REFERENCES Clients(ID) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_ProvidedServices_Employees FOREIGN KEY (EmployeeID) REFERENCES Employees(ID) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_ProvidedServices_Services FOREIGN KEY (ServiceID) REFERENCES Services(ID) ON DELETE NO ACTION ON UPDATE CASCADE -- Не удалять услугу при удалении записи
);
CREATE INDEX IX_ProvidedServices_ClientID ON ProvidedServices(ClientID);
CREATE INDEX IX_ProvidedServices_EmployeeID ON ProvidedServices(EmployeeID);
CREATE INDEX IX_ProvidedServices_ServiceID ON ProvidedServices(ServiceID);
CREATE INDEX IX_ProvidedServices_ServiceDateTime ON ProvidedServices(ServiceDateTime);
CREATE INDEX IX_ProvidedServices_Status ON ProvidedServices(Status);

-- Дополнительные проверки (CHECK) и комментарии:
-- В OrderItems: ON DELETE NO ACTION для ProductID — чтобы не удалять товар из системы при удалении позиции заказа.
-- В ProvidedServices: аналогично — услуга должна существовать независимо от того, оказывалась ли она.
-- Поле AccessLevel в Authorization: -1 означает блокировку, остальные значения — уровни доступа (можно расширять по мере необходимости).
-- Использованы PERSISTED вычисляемые поля (Subtotal) для оптимизации.

-- Готово. База данных создана и готова к использованию.