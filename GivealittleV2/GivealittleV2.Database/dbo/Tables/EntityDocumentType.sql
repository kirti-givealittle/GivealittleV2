CREATE TABLE [dbo].[EntityDocumentType] (
    [ID]           UNIQUEIDENTIFIER NOT NULL,
    [DocumentType] VARCHAR (50)     NOT NULL,
    [Description]  VARBINARY (50)   NULL,
    CONSTRAINT [PK_EntityDocumentTypes] PRIMARY KEY CLUSTERED ([ID] ASC)
);

