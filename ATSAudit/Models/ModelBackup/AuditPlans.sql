SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditPlans](
	[PlanId] [int] IDENTITY(1,1) NOT NULL,
	[Requestor] [nvarchar](max) NOT NULL,
	[Department] [nvarchar](max) NOT NULL,
	[AuditCategory] [nvarchar](max) NOT NULL,
	[TargetDate] [datetime2](7) NOT NULL,
	[ActualAuditDate] [datetime2](7) NULL,
	[Status] [tinyint] NOT NULL,
	[Remark] [nvarchar](max) NULL,
	[AuditeeApproved] [bit] NOT NULL,
	[AuditorApproved] [bit] NOT NULL,
	[TimeEnd] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_AuditPlans] PRIMARY KEY CLUSTERED 
(
	[PlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_TargetDate] UNIQUE NONCLUSTERED 
(
	[TargetDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditPlans] ADD  DEFAULT (CONVERT([bit],(0))) FOR [AuditeeApproved]
GO
ALTER TABLE [dbo].[AuditPlans] ADD  DEFAULT (CONVERT([bit],(0))) FOR [AuditorApproved]
GO
ALTER TABLE [dbo].[AuditPlans] ADD  DEFAULT (N'') FOR [TimeEnd]
GO
