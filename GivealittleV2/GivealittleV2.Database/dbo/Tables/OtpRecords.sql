CREATE TABLE [dbo].[OtpRecords] (
    [Id]                     UNIQUEIDENTIFIER CONSTRAINT [DF_OtpRecords_Id] DEFAULT (newsequentialid()) NOT NULL,
    [UserEmail]              NVARCHAR (256)   NOT NULL,
    [Purpose]                INT              NOT NULL,
    [OtpHash]                VARBINARY (64)   NOT NULL,
    [Salt]                   VARBINARY (32)   NOT NULL,
    [CreatedAtUtc]           DATETIME2 (3)    NOT NULL,
    [ExpiresAtUtc]           DATETIME2 (3)    NOT NULL,
    [FailedAttempts]         INT              CONSTRAINT [DF_OtpRecords_FailedAttempts] DEFAULT ((0)) NOT NULL,
    [MaxAttempts]            INT              CONSTRAINT [DF_OtpRecords_MaxAttempts] DEFAULT ((5)) NOT NULL,
    [LockedUntilUtc]         DATETIME2 (3)    NULL,
    [UsedAtUtc]              DATETIME2 (3)    NULL,
    [NextResendAllowedAtUtc] DATETIME2 (3)    NULL,
    CONSTRAINT [PK_OtpRecords] PRIMARY KEY CLUSTERED ([Id] ASC)
);

