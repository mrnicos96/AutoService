USE AutoService;
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
(1, 1, '2025-05-01', NULL, N'Замена масла и фильтров', 5000.00, 'InProgress'),
(2, 2, '2025-05-10', NULL, N'Ремонт тормозной системы', 12000.00, 'InProgress'),
(3, 1, '2025-05-15', '2025-05-20', N'Замена свечей зажигания', 3000.00, 'Completed'),
(1, 3, '2025-04-25', '2025-05-05', N'Диагностика двигателя', 8000.00, 'Completed');
GO