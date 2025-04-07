-- Benutzer-Tabelle
CREATE TABLE Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    Email TEXT UNIQUE,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Einnahmen-Tabelle
CREATE TABLE Income (
    IncomeID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    Amount REAL NOT NULL,
    Category TEXT NOT NULL,
    Description TEXT,
    Date DATE NOT NULL DEFAULT CURRENT_DATE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Ausgaben-Tabelle
CREATE TABLE Expenses (
    ExpenseID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    Amount REAL NOT NULL,
    Category TEXT NOT NULL,
    Description TEXT,
    Date DATE NOT NULL DEFAULT CURRENT_DATE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Fixkosten-Tabelle
CREATE TABLE FixedCosts (
    FixedCostID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    Amount REAL NOT NULL,
    Category TEXT NOT NULL,
    Description TEXT NOT NULL,
    Interval TEXT CHECK (Interval IN ('monatlich', 'jährlich', 'wöchentlich')) NOT NULL,
    StartDate DATE NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Anlagevermögen-Tabelle
CREATE TABLE Assets (
    AssetID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    Name TEXT NOT NULL,
    Value REAL NOT NULL,
    PurchaseDate DATE,
    Description TEXT,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Umlaufvermögen-Tabelle
CREATE TABLE CurrentAssets (
    CurrentAssetID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    Name TEXT NOT NULL,
    Value REAL NOT NULL,
    LastUpdated DATE NOT NULL DEFAULT CURRENT_DATE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Monatsübersicht-Tabelle
CREATE TABLE MonthlySummary (
    SummaryID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    Month INTEGER NOT NULL,
    Year INTEGER NOT NULL,
    TotalIncome REAL NOT NULL,
    TotalExpenses REAL NOT NULL,
    Balance REAL NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    UNIQUE(UserID, Month, Year)
);