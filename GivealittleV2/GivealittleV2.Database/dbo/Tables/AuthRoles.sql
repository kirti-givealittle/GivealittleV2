CREATE TABLE [dbo].[AuthRoles] (
    [AuthRoleId]     UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (128)   NOT NULL,
    [NormalizedName] NVARCHAR (128)   NOT NULL,
    [CreatedAtUtc]   DATETIME2 (0)    CONSTRAINT [DF_AuthRoles_CreatedAtUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_AuthRoles] PRIMARY KEY CLUSTERED ([AuthRoleId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_AuthRoles_NormalizedName]
    ON [dbo].[AuthRoles]([NormalizedName] ASC);

