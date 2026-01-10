CREATE TABLE [dbo].[EmailTemplates] (
    [TemplateId]      UNIQUEIDENTIFIER CONSTRAINT [DF_EmailTemplates_Id] DEFAULT (newsequentialid()) NOT NULL,
    [TemplateKey]     NVARCHAR (100)   NOT NULL,
    [TemplateType]    NVARCHAR (50)    NOT NULL,
    [SubjectTemplate] NVARCHAR (300)   NOT NULL,
    [BodyTemplate]    NVARCHAR (MAX)   NOT NULL,
    [IsHtml]          BIT              CONSTRAINT [DF_EmailTemplates_IsHtml] DEFAULT ((1)) NOT NULL,
    [IsActive]        BIT              CONSTRAINT [DF_EmailTemplates_IsActive] DEFAULT ((1)) NOT NULL,
    [Description]     NVARCHAR (400)   NULL,
    [CreatedAtUtc]    DATETIME2 (0)    DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAtUtc]    DATETIME2 (0)    DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED ([TemplateId] ASC),
    CONSTRAINT [UQ_EmailTemplates_TemplateKey] UNIQUE NONCLUSTERED ([TemplateKey] ASC)
);


GO
CREATE TRIGGER trg_EmailTemplates_UpdateTimestamp
ON EmailTemplates
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE t
    SET UpdatedAtUtc = SYSUTCDATETIME()
    FROM EmailTemplates t
    INNER JOIN INSERTED i ON t.TemplateId = i.TemplateId;
END;
