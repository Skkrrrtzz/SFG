USE [QA_AUDIT]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 06/03/2024 5:24:53 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditPlans]    Script Date: 06/03/2024 5:24:53 pm ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UC_TargetDate] UNIQUE NONCLUSTERED 
(
	[TargetDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditPlansCopy]    Script Date: 06/03/2024 5:24:53 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditPlansCopy](
	[PlanId] [int] NOT NULL,
	[Requestor] [nvarchar](max) NOT NULL,
	[Department] [nvarchar](max) NOT NULL,
	[AuditCategory] [nvarchar](max) NOT NULL,
	[TargetDate] [datetime2](7) NOT NULL,
	[ActualAuditDate] [datetime2](7) NULL,
	[Status] [nvarchar](max) NOT NULL,
	[Remark] [nvarchar](max) NULL,
	[AuditeeApproved] [bit] NOT NULL,
	[AuditorApproved] [bit] NOT NULL,
	[TimeEnd] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Conformities]    Script Date: 06/03/2024 5:24:53 pm ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Corrections]    Script Date: 06/03/2024 5:24:53 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Corrections](
	[CorrectionDescription] [nvarchar](max) NOT NULL,
	[EscapeCause] [nvarchar](max) NOT NULL,
	[Action] [nvarchar](max) NOT NULL,
	[ConformityId] [int] IDENTITY(1,1) NOT NULL,
	[CPARId] [int] NOT NULL,
UNIQUE NONCLUSTERED 
(
	[CPARId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CorrectiveActions]    Script Date: 06/03/2024 5:24:53 pm ******/
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
/****** Object:  Table [dbo].[CPARs]    Script Date: 06/03/2024 5:24:53 pm ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments]    Script Date: 06/03/2024 5:24:53 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[DepartmentID] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [nvarchar](max) NOT NULL,
	[DepartmentManager] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PreventiveActions]    Script Date: 06/03/2024 5:24:53 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PreventiveActions](
	[PreventiveActionId] [int] NULL,
	[ActionDescription] [int] NULL,
	[TargetDate] [int] NULL,
	[Responsible] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestTable]    Script Date: 06/03/2024 5:24:53 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestTable](
	[TestColumn] [int] NULL,
	[Email] [varchar](30) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditPlans] ADD  DEFAULT (CONVERT([bit],(0))) FOR [AuditeeApproved]
GO
ALTER TABLE [dbo].[AuditPlans] ADD  DEFAULT (CONVERT([bit],(0))) FOR [AuditorApproved]
GO
ALTER TABLE [dbo].[AuditPlans] ADD  DEFAULT (N'') FOR [TimeEnd]
GO
ALTER TABLE [dbo].[AuditPlansCopy] ADD  DEFAULT (CONVERT([bit],(0))) FOR [AuditeeApproved]
GO
ALTER TABLE [dbo].[AuditPlansCopy] ADD  DEFAULT (CONVERT([bit],(0))) FOR [AuditorApproved]
GO
ALTER TABLE [dbo].[AuditPlansCopy] ADD  DEFAULT (N'') FOR [TimeEnd]
GO
ALTER TABLE [dbo].[Conformities]  WITH CHECK ADD  CONSTRAINT [FK_AuditPlans_Conformities] FOREIGN KEY([PlanId])
REFERENCES [dbo].[AuditPlans] ([PlanId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Conformities] CHECK CONSTRAINT [FK_AuditPlans_Conformities]
GO
/****** Object:  StoredProcedure [dbo].[sp_AddConformityToAuditPlan]    Script Date: 06/03/2024 5:24:53 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_AddConformityToAuditPlan]
	@PlanId int,
	@Description nvarchar(max),
	@AreaSection nvarchar(max)
as 
begin
--	declare @LatestConformity int; 
--	declare @seq nvarchar(max);

--	select @LatestConformity = MAX(ConformityId) from [dbo].[Conformities] where PlanId = @PlanId;
--	set @seq = N'create sequence dbo.seq_PlanConformities start with' + cast(@LatestConformity as nvarchar(10)) + 'increment by 1;';
--	EXEC sp_executesql @seq;

	drop table if exists #temp_PlanConformities
	create table #temp_PlanConformities (
		ConformityId int identity(1,1) primary key,
--		PlanId int
		ConformityDescription varchar(max),
		ConformityAreaSection varchar(max)
	)

	-- seed with values
	insert into #temp_PlanConformities
	select ConformityId, ConformityDescription, ConformityAreaSection
	from [dbo].[Conformities] conformities
	where PlanId = @PlanId;

	-- add conformity
--	insert into #temp_PlanConformities (
--		PlanId, ConformityDescription, ConformityAreaSection
--	) values (
--		@PlanId, @Description, @AreaSection
--	);

	-- insert into main table
--	insert into [dbo].[Conformities]
--	select * from #temp_PlanConformities where ConformityId = (select max(ConformityId) from #temp_PlanConformities); 

	select * from #temp_PlanConformities;

--	drop sequence [dbo].[seq_PlanConformities];
end;

--AddConformityToAuditPlan()
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertAuditPlan]    Script Date: 06/03/2024 5:24:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_InsertAuditPlan]
    @Requestor nvarchar(30),
    @Department nvarchar(30),
    @AuditCategory nvarchar(30),
    @TargetDate datetime2(7),
    @TimeEnd datetime2(7),
    @AuditorApproved bit,
    @AuditeeApproved bit,
    @Status tinyint
AS
BEGIN
    DECLARE @ReturnedAuditPlan TABLE (
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
        [TimeEnd] [nvarchar](max) NOT NULL
    );

    INSERT INTO [dbo].[AuditPlans] (Requestor, Department, AuditCategory, TargetDate, TimeEnd, AuditorApproved, AuditeeApproved, Status)
    OUTPUT INSERTED.*
    VALUES ( @Requestor, @Department, @AuditCategory, @TargetDate, @TimeEnd, @AuditorApproved, @AuditeeApproved, @Status );
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateStatus]    Script Date: 06/03/2024 5:24:54 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_UpdateStatus]
    @Status nvarchar(12),
    @PlanId int
AS
BEGIN
    -- select * from dbo.AuditPlans where MONTH(TargetDate) = @Month;
    -- insert into [dbo].[AuditPlans] (    IF @Status NOT IN ('Open', 'Closed', 'For Approval')
    IF @Status NOT IN ('Open', 'Closed', 'For Approval')
    BEGIN
        RAISERROR ('Invalid status value: %s', 16, 1, @Status);
        RETURN;
    END

    update [dbo].[AuditPlans] set Status = 
        case 
            when @Status = 'For Approval' then 0
            when @Status = 'Open' then 1
            when @Status = 'Closed' then 2
        end
    where Id = @PlanId;
END;
GO
