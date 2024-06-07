CREATE TABLE [dbo].[Departments] (
    [DepartmentID]      INT            IDENTITY (1, 1) NOT NULL,
    [DepartmentName]    NVARCHAR (MAX) NOT NULL,
    [DepartmentManager] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED ([DepartmentID] ASC)
);


GO

