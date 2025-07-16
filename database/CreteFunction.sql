USE AutoService;
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