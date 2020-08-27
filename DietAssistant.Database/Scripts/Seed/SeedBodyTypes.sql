MERGE INTO [dbo].[BodyType] AS [Target]
	USING (
	VALUES
	(1, 'Ectomorph'),
	(2, 'Endomorph'),
	(3, 'Mesomorph')
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
   OUTPUT N'[dbo].[BodyType]' AS [Table]
	 , $action AS MergeAction
	 , ISNULL( [inserted].[Id], [deleted].[Id] ) AS [Id]
	 , ISNULL( [inserted].[Name], [deleted].[Name] ) AS [Name];