
/*
	Remove constraints
*/

IF EXISTS (SELECT 1 
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
    WHERE CONSTRAINT_NAME = N'FK_{objectQualifier}Visits_Visitors' 
    AND TABLE_NAME = N'{objectQualifier}Visits')
    ALTER TABLE {databaseOwner}[{objectQualifier}Visits] DROP CONSTRAINT [FK_{objectQualifier}Visits_Visitors];
GO
	
IF EXISTS (SELECT 1 
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
    WHERE CONSTRAINT_NAME = N'FK_{objectQualifier}Visits_Tabs' 
    AND TABLE_NAME = N'{objectQualifier}Visits')
    ALTER TABLE {databaseOwner}[{objectQualifier}Visits] DROP CONSTRAINT [FK_{objectQualifier}Visits_Tabs];
GO

IF EXISTS (SELECT 1 
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
    WHERE CONSTRAINT_NAME = N'FK_{objectQualifier}Visitors_Users' 
    AND TABLE_NAME = N'{objectQualifier}Visitors')
    ALTER TABLE {databaseOwner}[{objectQualifier}Visitors] DROP CONSTRAINT [FK_{objectQualifier}Visitors_Users];
GO

IF EXISTS (SELECT 1 
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
    WHERE CONSTRAINT_NAME = N'FK_{objectQualifier}Visitors_Portals' 
    AND TABLE_NAME = N'{objectQualifier}Visitors')
    ALTER TABLE {databaseOwner}[{objectQualifier}Visitors] DROP CONSTRAINT [FK_{objectQualifier}Visitors_Portals];
GO

/*
	Rename community tables & PK's to prevent conflicts and group the data together
*/

sp_rename N'{objectQualifier}Visitors', N'{objectQualifier}Community_Visitors';
GO

sp_rename @objname = N'[PK_{objectQualifier}Visitors]', @newname = N'PK_{objectQualifier}Community_Visitors';
GO

sp_rename N'{objectQualifier}Visits', N'{objectQualifier}Community_Visits';
GO

sp_rename @objname = N'[PK_{objectQualifier}Visits]', @newname = N'PK_{objectQualifier}Community_Visits';
GO

/*
	Add constraints back
*/

ALTER TABLE {databaseOwner}[{objectQualifier}Community_Visitors]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}Community_Visitors_Portals] FOREIGN KEY([portal_id])
	REFERENCES {databaseOwner}[{objectQualifier}Portals] ([PortalID])
	ON DELETE CASCADE;
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Community_Visitors] CHECK CONSTRAINT [FK_{objectQualifier}Community_Visitors_Portals];
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Community_Visitors]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}Community_Visitors_Users] FOREIGN KEY([user_id])
	REFERENCES {databaseOwner}[{objectQualifier}Users] ([UserID])
	ON UPDATE CASCADE 
	ON DELETE CASCADE;
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Community_Visitors] CHECK CONSTRAINT [FK_{objectQualifier}Community_Visitors_Users];
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Community_Visits]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}Community_Visits_Tabs] FOREIGN KEY([tab_id])
	REFERENCES {databaseOwner}[{objectQualifier}Tabs] ([TabID]);
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Community_Visits] CHECK CONSTRAINT [FK_{objectQualifier}Community_Visits_Tabs];
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Community_Visits]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}Community_Visits_Visitors] FOREIGN KEY([visitor_id])
	REFERENCES {databaseOwner}[{objectQualifier}Community_Visitors] ([id])
	ON DELETE CASCADE;
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Community_Visits] CHECK CONSTRAINT [FK_{objectQualifier}Community_Visits_Visitors];
GO
