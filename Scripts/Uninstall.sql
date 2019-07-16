IF NOT OBJECT_ID('{databaseOwner}[{objectQualifier}Community_Visits]') IS NULL
    DROP TABLE {databaseOwner}[{objectQualifier}Community_Visits];
GO

IF NOT OBJECT_ID('{databaseOwner}[{objectQualifier}Community_Visitors]') IS NULL
    DROP TABLE {databaseOwner}[{objectQualifier}Community_Visitors];
GO

DELETE FROM {databaseOwner}[{objectQualifier}Schedule] WHERE [TypeFullName] = N'Dnn.WebAnalytics.VisitorJob, Dnn.WebAnalytics';
GO