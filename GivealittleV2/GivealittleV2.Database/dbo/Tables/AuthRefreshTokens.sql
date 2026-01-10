CREATE TABLE [dbo].[AuthRefreshTokens] (
    [RefreshTokenId]           UNIQUEIDENTIFIER NOT NULL,
    [AuthUserId]               UNIQUEIDENTIFIER NOT NULL,
    [TokenHash]                VARBINARY (32)   NOT NULL,
    [CreatedAtUtc]             DATETIME2 (0)    CONSTRAINT [DF_AuthRefreshTokens_CreatedAtUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [ExpiresAtUtc]             DATETIME2 (0)    NOT NULL,
    [RevokedAtUtc]             DATETIME2 (0)    NULL,
    [ReplacedByRefreshTokenId] UNIQUEIDENTIFIER NULL,
    [CreatedByIp]              NVARCHAR (45)    NULL,
    [UserAgent]                NVARCHAR (512)   NULL,
    CONSTRAINT [PK_AuthRefreshTokens] PRIMARY KEY CLUSTERED ([RefreshTokenId] ASC),
    CONSTRAINT [FK_AuthRefreshTokens_EntityEmail] FOREIGN KEY ([AuthUserId]) REFERENCES [dbo].[EntityEmail] ([ID])
);

