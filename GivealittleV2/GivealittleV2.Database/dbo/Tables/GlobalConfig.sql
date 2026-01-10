CREATE TABLE [dbo].[GlobalConfig] (
    [Id]           UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Key]          NVARCHAR (150)   NOT NULL,
    [Value]        NVARCHAR (MAX)   NULL,
    [KeyGroup]     NVARCHAR (100)   NOT NULL,
    [CreatedAtUtc] DATETIME2 (7)    DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAtUtc] DATETIME2 (7)    DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_GlobalConfig] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_GlobalConfig_Key] UNIQUE NONCLUSTERED ([Key] ASC)
);


GO
CREATE TRIGGER trg_GlobalConfig_UpdateTimestamp
ON GlobalConfig
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE t
    SET UpdatedAtUtc = SYSUTCDATETIME()
    FROM GlobalConfig t
    INNER JOIN INSERTED i ON t.Id = i.Id;
END;