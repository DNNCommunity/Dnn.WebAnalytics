
IF NOT OBJECT_ID('{databaseOwner}[{objectQualifier}Visitors]') IS NULL
    DROP TABLE {databaseOwner}[{objectQualifier}Visitors];
GO

CREATE TABLE {databaseOwner}[{objectQualifier}Visitors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[portal_id] [int] NOT NULL,
	[user_id] [int] NULL,
	[created_on_date] [datetime] NOT NULL,
 CONSTRAINT [PK_{objectQualifier}Visitors] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
) 
GO

IF NOT OBJECT_ID('{databaseOwner}[{objectQualifier}Visitors]') IS NULL
    DROP TABLE {databaseOwner}[{objectQualifier}Visits];
GO

CREATE TABLE {databaseOwner}[{objectQualifier}Visits](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[date] [datetime] NOT NULL,
	[visitor_id] [int] NOT NULL,
	[tab_id] [int] NOT NULL,
	[ip] [nvarchar](50) NOT NULL,
	[country] [nvarchar](50) NOT NULL,
	[region] [nvarchar](50) NOT NULL,
	[city] [nvarchar](50) NOT NULL,
	[latitude] [nvarchar](50) NOT NULL,
	[longitude] [nvarchar](50) NOT NULL,
	[language] [nvarchar](50) NOT NULL,
	[domain] [nvarchar](255) NOT NULL,
	[url] [nvarchar](2048) NOT NULL,
	[user_agent] [nvarchar](512) NOT NULL,
	[device_type] [nvarchar](50) NOT NULL,
	[device] [nvarchar](255) NOT NULL,
	[platform] [nvarchar](255) NOT NULL,
	[browser] [nvarchar](255) NOT NULL,
	[referrer_domain] [nvarchar](255) NOT NULL,
	[referrer_url] [nvarchar](2048) NOT NULL,
	[server] [nvarchar](50) NOT NULL,
	[campaign] [nvarchar](50) NOT NULL,
	[session_id] [uniqueidentifier] NULL,
	[request_id] [uniqueidentifier] NULL,
	[last_request_id] [uniqueidentifier] NULL,
 CONSTRAINT [PK_{objectQualifier}Visits] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
) 
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Visitors]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}Visitors_Portals] FOREIGN KEY([portal_id])
REFERENCES {databaseOwner}[{objectQualifier}Portals] ([PortalID])
ON DELETE CASCADE
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Visitors] CHECK CONSTRAINT [FK_Visitors_Portals]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Visitors]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}Visitors_Users] FOREIGN KEY([user_id])
REFERENCES {databaseOwner}[{objectQualifier}Users] ([UserID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Visitors] CHECK CONSTRAINT [FK_{objectQualifier}Visitors_Users]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Visits]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}Visits_Tabs] FOREIGN KEY([tab_id])
REFERENCES {databaseOwner}[{objectQualifier}Tabs] ([TabID])
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Visits] CHECK CONSTRAINT [FK_{objectQualifier}Visits_Tabs]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}Visits]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}Visits_Visitors] FOREIGN KEY([visitor_id])
REFERENCES {databaseOwner}[{objectQualifier}Visitors] ([id])
ON DELETE CASCADE

GO
ALTER TABLE {databaseOwner}[{objectQualifier}Visits] CHECK CONSTRAINT [FK_{objectQualifier}Visits_Visitors]
GO

IF (SELECT 1 FROM {databaseOwner}[{objectQualifier}Schedule] WHERE [TypeFullName] = N'Dnn.WebAnalytics.VisitorJob, Dnn.WebAnalytics')
BEGIN
	UPDATE {databaseOwner}[{objectQualifier}Schedule] 
	SET 
		[TimeLapse] = 1, 
		[TimeLapseMeasurement] = N'd', 
		[RetryTimeLapse] = 1, 
		[RetryTimeLapseMeasurement] = N'd', 
		[RetainHistoryNum] = 10, 
		[AttachToEvent] = N'', 
		[CatchUpEnabled] = 0, 
		[Enabled] = 1, 
		[ObjectDependencies] = N'', 
		[Servers] = null, 
		[FriendlyName] = 'Visitor Tracking Job' 
	WHERE 
		[TypeFullName] = N'Dnn.WebAnalytics.VisitorJob, Dnn.WebAnalytics';
END
ELSE
BEGIN
	INSERT INTO {databaseOwner}[{objectQualifier}Schedule]
		( [TypeFullName], [TimeLapse], [TimeLapseMeasurement], [RetryTimeLapse], [RetryTimeLapseMeasurement], [RetainHistoryNum], [AttachToEvent], [CatchUpEnabled], [Enabled], [ObjectDependencies], [Servers], [FriendlyName])
	VALUES ( 'Dnn.WebAnalytics.VisitorJob, Dnn.WebAnalytics', 1, 'd', 1, 'd', 10, '', 0, 1, '', null, 'Visitor Tracking Job' );
END
GO
