SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conformities](
	[PlanId] [int] NOT NULL,
	[ConformityDescription] [nvarchar](max) NOT NULL,
	[ConformityAreaSection] [nvarchar](max) NULL,
	[ConformityId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ConformityId] PRIMARY KEY CLUSTERED 
(
	[ConformityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Conformities]  WITH CHECK ADD  CONSTRAINT [FK_AuditPlans_Conformities] FOREIGN KEY([PlanId])
REFERENCES [dbo].[AuditPlans] ([PlanId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Conformities] CHECK CONSTRAINT [FK_AuditPlans_Conformities]
GO
