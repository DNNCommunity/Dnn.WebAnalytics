IF NOT OBJECT_ID('{databaseOwner}[{objectQualifier}Community_Visits]') IS NULL
    DROP TABLE {databaseOwner}[{objectQualifier}Community_Visits];
GO
IF NOT OBJECT_ID('{databaseOwner}[{objectQualifier}Community_Visitors]') IS NULL
    DROP TABLE {databaseOwner}[{objectQualifier}Community_Visitors];
GO

IF EXISTS(SELECT 1 FROM {databaseOwner}[{objectQualifier}Schedule] WHERE [TypeFullName] = N'Dnn.WebAnalytics.VisitorJob, Dnn.WebAnalytics')
BEGIN
	DELETE FROM {databaseOwner}[{objectQualifier}Schedule] 
	WHERE 
		[TypeFullName] = N'Dnn.WebAnalytics.VisitorJob, Dnn.WebAnalytics';
END

