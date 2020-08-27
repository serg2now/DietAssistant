CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(30) NOT NULL, 
    [Surname] NVARCHAR(30) NOT NULL, 
    [MiddleName] NVARCHAR(30) NULL, 
    [BirthDate] DATE NULL, 
    [RoleId] INT NOT NULL, 
    [WeightInKilos] INT NULL , 
    [HeightInMeters] DECIMAL(3, 2) NULL , 
    [BodyTypeId] INT NULL , 
    CONSTRAINT [fk_User_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role]([Id]), 
    CONSTRAINT [fk_User_BodyType] FOREIGN KEY ([BodyTypeId]) REFERENCES [dbo].[BodyType]([Id]) 
)
