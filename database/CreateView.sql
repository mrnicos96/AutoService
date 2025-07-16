USE AutoService;
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