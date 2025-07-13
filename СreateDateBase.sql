CREATE DATABASE AutoService;
GO


USE AutoService;
GO

-- Таблица мастеров
CREATE TABLE Masters (
    MasterId INT IDENTITY(1,1) PRIMARY KEY,
    LastName NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50),
    Phone NVARCHAR(20) NOT NULL UNIQUE,
    ExperienceYears INT NOT NULL CHECK (ExperienceYears >= 0),
    CONSTRAINT UQ_MasterFullName UNIQUE (LastName, FirstName, MiddleName)
);
GO
-- Таблица владельцев авто
CREATE TABLE CarOwners (
    OwnerId INT IDENTITY(1,1) PRIMARY KEY,
    LastName NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50),
    Phone NVARCHAR(20) NOT NULL UNIQUE,
    Address NVARCHAR(200) NOT NULL,
    PassportData NVARCHAR(100) NOT NULL UNIQUE,
    CONSTRAINT UQ_OwnerFullName UNIQUE (LastName, FirstName, MiddleName)
);
GO
-- Таблица автомобилей
CREATE TABLE Cars (
    CarId INT IDENTITY(1,1) PRIMARY KEY,
    Make NVARCHAR(50) NOT NULL,
    Model NVARCHAR(50) NOT NULL,
    LicensePlate NVARCHAR(15) NOT NULL UNIQUE,
    EngineVolume DECIMAL(5,2) NOT NULL CHECK (EngineVolume > 0),
    Mileage INT NOT NULL CHECK (Mileage >= 0),
    OwnerId INT NOT NULL,
    CONSTRAINT FK_Cars_Owners FOREIGN KEY (OwnerId) REFERENCES CarOwners(OwnerId),
    CONSTRAINT UQ_MakeModelLicense UNIQUE (Make, Model, LicensePlate)
);
GO
-- Таблица ремонтов
CREATE TABLE Repairs (
    RepairId INT IDENTITY(1,1) PRIMARY KEY,
    CarId INT NOT NULL,
    MasterId INT NOT NULL,
    StartDate DATETIME NOT NULL DEFAULT GETDATE(),
    EndDate DATETIME,
    ProblemDescription NVARCHAR(500) NOT NULL,
    WorkCost DECIMAL(10,2) NOT NULL CHECK (WorkCost >= 0),
    Status NVARCHAR(20) NOT NULL DEFAULT 'InProgress' CHECK (Status IN ('InProgress', 'Completed', 'Cancelled')),
    CONSTRAINT FK_Repairs_Cars FOREIGN KEY (CarId) REFERENCES Cars(CarId),
    CONSTRAINT FK_Repairs_Masters FOREIGN KEY (MasterId) REFERENCES Masters(MasterId),
    CONSTRAINT CHK_RepairDates CHECK (EndDate IS NULL OR EndDate >= StartDate)
);
GO