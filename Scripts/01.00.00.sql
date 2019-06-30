
CREATE TABLE [dbo].[Visitors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[portal_id] [int] NOT NULL,
	[user_id] [int] NULL,
	[created_on_date] [datetime] NOT NULL,
 CONSTRAINT [PK_Visitors] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Visits](
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
 CONSTRAINT [PK_Visits] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Visitors]  WITH CHECK ADD  CONSTRAINT [FK_Visitors_Portals] FOREIGN KEY([portal_id])
REFERENCES [dbo].[Portals] ([PortalID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Visitors] CHECK CONSTRAINT [FK_Visitors_Portals]
GO
ALTER TABLE [dbo].[Visitors]  WITH CHECK ADD  CONSTRAINT [FK_Visitors_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([UserID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Visitors] CHECK CONSTRAINT [FK_Visitors_Users]
GO
ALTER TABLE [dbo].[Visits]  WITH CHECK ADD  CONSTRAINT [FK_Visits_Tabs] FOREIGN KEY([tab_id])
REFERENCES [dbo].[Tabs] ([TabID])
GO
ALTER TABLE [dbo].[Visits] CHECK CONSTRAINT [FK_Visits_Tabs]
GO
ALTER TABLE [dbo].[Visits]  WITH CHECK ADD  CONSTRAINT [FK_Visits_Visitors] FOREIGN KEY([visitor_id])
REFERENCES [dbo].[Visitors] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Visits] CHECK CONSTRAINT [FK_Visits_Visitors]
GO


INSERT INTO dbo.Schedule
	( TypeFullName, [TimeLapse], [TimeLapseMeasurement], [RetryTimeLapse], [RetryTimeLapseMeasurement], [RetainHistoryNum], [AttachToEvent], [CatchUpEnabled], [Enabled], [ObjectDependencies], [Servers], [FriendlyName])
VALUES ( 'Dnn.WebAnalytics.VisitorJob, Dnn.WebAnalytics', 1, 'd', 1, 'd', 10, '', 0, 1, '', null, 'Visitor Tracking Job' )
GO
