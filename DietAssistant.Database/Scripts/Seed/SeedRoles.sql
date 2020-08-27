MERGE INTO [dbo].[Role] AS [Target]
	USING (
	VALUES
	(1, 'User'),
	(2, 'Admin')
   ) AS [Source]
   (
        [Id],
		[Name]
   )
    ON ([Target].[Id] = [Source].[Id])
   WHEN NOT MATCHED BY TARGET THEN
   INSERT  
   (
        [Id],
		[Name]
   )
   VALUES
   (
        [Source].[Id],
		[Source].[Name]
   )
   OUTPUT N'[dbo].[Role]' AS [Table]
	 , $action AS MergeAction
	 , ISNULL( [inserted].[Id], [deleted].[Id] ) AS [Id]
	 , ISNULL( [inserted].[Name], [deleted].[Name] ) AS [Name];