CREATE TABLE [dbo].[AuthLoginAudit] (
    [AuthLoginAuditId] UNIQUEIDENTIFIER CONSTRAINT [DF_AuthLoginAudit_Id] DEFAULT (newsequentialid()) NOT NULL,
    [OccurredAtUtc]    DATETIME2 (0)    CONSTRAINT [DF_AuthLoginAudit_OccurredAtUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [AuthUserId]       UNIQUEIDENTIFIER NULL,
    [Email]            NVARCHAR (320)   NULL,
    [Success]          BIT              NOT NULL,
    [EventType]        NVARCHAR (50)    NOT NULL,
    [FailureReason]    NVARCHAR (200)   NULL,
    [IpAddress]        NVARCHAR (45)    NULL,
    [UserAgent]        NVARCHAR (512)   NULL,
    CONSTRAINT [PK_AuthLoginAudit] PRIMARY KEY CLUSTERED ([AuthLoginAuditId] ASC)
);

