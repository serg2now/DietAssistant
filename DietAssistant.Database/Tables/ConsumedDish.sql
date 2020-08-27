CREATE TABLE [dbo].[ConsumedDish]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(30) NOT NULL, 
    [Description] NVARCHAR(150) NULL, 
    [DateOfConsume] DATE NOT NULL, 
    [ConsumeTimeTypeId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    [CarbohydratesAmount] DECIMAL(7, 3) NOT NULL, 
    [FatsAmount] DECIMAL(7, 3) NOT NULL, 
    [ProteinsAmount] DECIMAL(7, 3) NOT NULL, 
    [IsFoodStuff] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_ConsumedDish_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id]), 
    CONSTRAINT [FK_ConsumedDish_ConsumeTimeType] FOREIGN KEY ([ConsumeTimeTypeId]) REFERENCES [dbo].[ConsumeTimeType]([Id]) 
)
