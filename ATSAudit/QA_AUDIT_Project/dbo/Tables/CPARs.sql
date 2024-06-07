CREATE TABLE [dbo].[CPARs] (
    [CPARId]           INT            IDENTITY (1, 1) NOT NULL,
    [PlanId]           INT            NOT NULL,
    [Respondent]       NVARCHAR (50)  NOT NULL,
    [Requestor]        NVARCHAR (50)  NOT NULL,
    [IssueDate]        DATETIME2 (7)  NULL,
    [ApprovalDate]     DATETIME2 (7)  NULL,
    [ResponseDueDate]  DATETIME2 (7)  NOT NULL,
    [ISOClause]        NVARCHAR (MAX) NOT NULL,
    [ProblemStatement] NVARCHAR (MAX) NOT NULL,
    [RCMethod]         NVARCHAR (MAX) NULL,
    [RCMachine]        NVARCHAR (MAX) NULL,
    [RCMaterial]       NVARCHAR (MAX) NULL,
    [RCMan]            NVARCHAR (MAX) NULL,
    [RCEnvironment]    NVARCHAR (MAX) NULL,
    [PreparedBy]       NVARCHAR (50)  NULL,
    [CheckedBy]        NVARCHAR (50)  NULL,
    [ApprovedBy]       NVARCHAR (50)  NULL,
    CONSTRAINT [PK_CPARId] PRIMARY KEY CLUSTERED ([CPARId] ASC)
);


GO

