CREATE TABLE [dbo].[MsGraphOAuthConfigs] (
    [MsGraphConfigId]         UNIQUEIDENTIFIER CONSTRAINT [DF_MsGraphOAuthConfigs_Id] DEFAULT (newsequentialid()) NOT NULL,
    [TenantId]                NVARCHAR (100)   NOT NULL,
    [ClientId]                NVARCHAR (300)   NOT NULL,
    [ClientSecret]            NVARCHAR (300)   NOT NULL,
    [RefreshToken]            NVARCHAR (1000)  NULL,
    [AccessToken]             NVARCHAR (2000)  NULL,
    [AccessTokenExpiresAtUtc] DATETIME2 (0)    NULL,
    [CreatedAtUtc]            DATETIME2 (0)    DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAtUtc]            DATETIME2 (0)    DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_MsGraphOAuthConfigs] PRIMARY KEY CLUSTERED ([MsGraphConfigId] ASC)
);


GO
CREATE TRIGGER trg_MsGraphOAuthConfigs_UpdateTimestamp
ON MsGraphOAuthConfigs
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE t
    SET UpdatedAtUtc = SYSUTCDATETIME()
    FROM MsGraphOAuthConfigs t
    INNER JOIN INSERTED i ON t.MsGraphConfigId = i.MsGraphConfigId;
END;