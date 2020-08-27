MERGE INTO [dbo].[ConsumeTimeType] AS [Target]
	USING (
	VALUES
	(1, 'Breakfast'),
    (2, 'Lunch'),
    (3, 'Dinner'),
    (4, 'Other')
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
   OUTPUT N'[dbo].[ConsumeTimeType]' AS [Table]
	 , $action AS MergeAction
	 , ISNULL( [inserted].[Id], [deleted].[Id] ) AS [Id]
	 , ISNULL( [inserted].[Name], [deleted].[Name] ) AS [Name];