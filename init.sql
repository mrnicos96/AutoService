WAITFOR DELAY '00:00:30';
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
-- Функция для вычисления загруженности мастера
CREATE FUNCTION GetMasterWorkloadPercentage(
    @MasterId INT,
    @Month INT,
    @Year INT
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @MasterTotal DECIMAL(10,2);
    DECLARE @AllMastersTotal DECIMAL(10,2);
    DECLARE @Result DECIMAL(5,2);
    
    SELECT @MasterTotal = SUM(WorkCost)
    FROM Repairs
    WHERE MasterId = @MasterId
    AND MONTH(StartDate) = @Month
    AND YEAR(StartDate) = @Year;
    
    SELECT @AllMastersTotal = SUM(WorkCost)
    FROM Repairs
    WHERE MONTH(StartDate) = @Month
    AND YEAR(StartDate) = @Year;
    
    IF @AllMastersTotal = 0 OR @AllMastersTotal IS NULL
        SET @Result = 0;
    ELSE
        SET @Result = ROUND((@MasterTotal * 100.0 / @AllMastersTotal), 2);
    
    RETURN @Result;
END;
GO
-- Представление для автомобилей в мае (с промежуточными итогами)
CREATE VIEW MayInProgressRepairsView AS
WITH MasterRepairStats AS (
    SELECT 
        m.MasterId,
        CONCAT(m.LastName, ' ', m.FirstName, ' ', ISNULL(m.MiddleName, '')) AS MasterFullName,
        COUNT(r.RepairId) OVER (PARTITION BY m.MasterId) AS MasterRepairCount,
        SUM(r.WorkCost) OVER (PARTITION BY m.MasterId) AS MasterTotalCost,
        SUM(r.WorkCost) OVER () AS GrandTotalCost
    FROM Masters m
    JOIN Repairs r ON m.MasterId = r.MasterId
    WHERE r.Status = 'InProgress'
    AND MONTH(r.StartDate) = 5
    AND YEAR(r.StartDate) = YEAR(GETDATE())
)
SELECT 
    r.StartDate AS RepairStartDate,
    c.Make AS CarMake,
    c.Model AS CarModel,
    c.LicensePlate AS LicensePlate,
    CONCAT(co.LastName, ' ', co.FirstName, ' ', ISNULL(co.MiddleName, '')) AS OwnerFullName,
    co.Phone AS OwnerPhone,
    r.ProblemDescription AS Problem,
    r.WorkCost AS Cost,
    mrs.MasterFullName,
    m.Phone AS MasterPhone,
    m.ExperienceYears AS MasterExperience,
    mrs.MasterTotalCost AS MasterTotalCost,
    mrs.GrandTotalCost AS AllMastersTotalCost,
    CAST(ROUND((mrs.MasterTotalCost * 100.0 / NULLIF(mrs.GrandTotalCost, 0)), 2) AS DECIMAL(5,2)) AS MasterWorkloadPercentage
FROM Repairs r
JOIN Cars c ON r.CarId = c.CarId
JOIN CarOwners co ON c.OwnerId = co.OwnerId
JOIN Masters m ON r.MasterId = m.MasterId
JOIN MasterRepairStats mrs ON m.MasterId = mrs.MasterId
WHERE r.Status = 'InProgress'
AND MONTH(r.StartDate) = 5
AND YEAR(r.StartDate) = YEAR(GETDATE());

GO

-- Тестовые данные
INSERT INTO Masters (LastName, FirstName, MiddleName, Phone, ExperienceYears) VALUES
(N'Иванов', N'Иван', N'Иванович', '+79101112233', 5),
(N'Петров', N'Петр', N'Петрович', '+79112223344', 3),
(N'Сидоров', N'Сергей', N'Сергеевич', '+79123334455', 7);
GO
INSERT INTO CarOwners (LastName, FirstName, MiddleName, Phone, Address, PassportData) VALUES
(N'Кузнецов', N'Алексей', N'Николаевич', '+79031112233', N'ул. Ленина, 10', '4510 123456'),
(N'Смирнова', N'Ольга', N'Владимировна', '+79042223344', N'пр. Мира, 25', '4511 654321'),
(N'Федоров', N'Дмитрий', N'Александрович', '+79053334455', N'ул. Гагарина, 7', '4512 789012');
GO
INSERT INTO Cars (Make, Model, LicensePlate, EngineVolume, Mileage, OwnerId) VALUES
('Toyota', 'Camry', N'А123БВ777', 2.5, 45000, 1),
('Honda', 'Accord', N'В456ГД777', 2.0, 60000, 2),
('BMW', 'X5', N'Е789ЖК777', 3.0, 30000, 3);
GO
INSERT INTO Repairs (CarId, MasterId, StartDate, EndDate, ProblemDescription, WorkCost, Status) VALUES
(1, 1, '2023-05-01', NULL, N'Замена масла и фильтров', 5000.00, 'InProgress'),
(2, 2, '2023-05-10', NULL, N'Ремонт тормозной системы', 12000.00, 'InProgress'),
(3, 1, '2023-05-15', '2023-05-20', N'Замена свечей зажигания', 3000.00, 'Completed'),
(1, 3, '2023-04-25', '2023-05-05', N'Диагностика двигателя', 8000.00, 'Completed');
GO
PRINT 'Database initialization completed successfully!';