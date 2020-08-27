CREATE TABLE [dbo].[DailyReport]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReportDate] DATE NOT NULL, 
    [UserId] INT NOT NULL, 
    [CarbohydratesAmount] DECIMAL(8, 3) NOT NULL, 
    [FatsAmount] DECIMAL(8, 3) NOT NULL, 
    [ProteinsAmount] DECIMAL(8, 3) NOT NULL, 
    [HasWarnings] BIT NOT NULL, 
    [Warnings] NVARCHAR(200) NOT NULL
)
