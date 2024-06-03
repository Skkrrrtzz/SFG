SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CPARs](
	[CPARId] [int] IDENTITY(1,1) NOT NULL,
	[PlanId] [int] NOT NULL,
	[Respondent] [nvarchar](50) NOT NULL,
	[Requestor] [nvarchar](50) NOT NULL,
	[IssueDate] [datetime2](7) NULL,
	[ApprovalDate] [datetime2](7) NULL,
	[ResponseDueDate] [datetime2](7) NOT NULL,
	[ISOClause] [nvarchar](max) NULL,
	[ProblemStatement] [nvarchar](max) NOT NULL,
	[RCMethod] [nvarchar](max) NULL,
	[RCMachine] [nvarchar](max) NULL,
	[RCMaterial] [nvarchar](max) NULL,
	[RCMan] [nvarchar](max) NULL,
	[RCEnvironment] [nvarchar](max) NULL,
	[PreparedBy] [nvarchar](50) NOT NULL,
	[CheckedBy] [nvarchar](50) NULL,
	[ApprovedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_CPARId] PRIMARY KEY CLUSTERED 
(
	[CPARId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
