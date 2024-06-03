SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CorrectiveActions](
	[ActionDescription] [int] NULL,
	[TargetDate] [int] NULL,
	[Responsible] [int] NULL,
	[CorrectiveActionId] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO
