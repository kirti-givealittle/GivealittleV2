CREATE TABLE [dbo].[GmailOAuthConfigs] (
    [GmailConfigId]           UNIQUEIDENTIFIER CONSTRAINT [DF_GmailOAuthConfigs_Id] DEFAULT (newsequentialid()) NOT NULL,
    [ClientId]                NVARCHAR (300)   NOT NULL,
    [ClientSecret]            NVARCHAR (300)   NOT NULL,
    [RefreshToken]            NVARCHAR (1000)  NOT NULL,
    [AccessToken]             NVARCHAR (2000)  NULL,
    [AccessTokenExpiresAtUtc] DATETIME2 (0)    NULL,
    [CreatedAtUtc]            DATETIME2 (0)    DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAtUtc]            DATETIME2 (0)    DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_GmailOAuthConfigs] PRIMARY KEY CLUSTERED ([GmailConfigId] ASC)
);


GO
CREATE TRIGGER trg_GmailOAuthConfigs_UpdateTimestamp
ON GmailOAuthConfigs
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE t
    SET UpdatedAtUtc = SYSUTCDATETIME()
    FROM GmailOAuthConfigs t
    INNER JOIN INSERTED i ON t.GmailConfigId = i.GmailConfigId;
END;