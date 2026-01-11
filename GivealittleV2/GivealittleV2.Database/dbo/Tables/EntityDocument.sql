CREATE TABLE [dbo].[EntityDocument] (
    [ID]                   UNIQUEIDENTIFIER NOT NULL,
    [EntityID]             UNIQUEIDENTIFIER NOT NULL,
    [EntityDocumentTypeID] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_EntityDocuments] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_EntityDocuments_Entity] FOREIGN KEY ([EntityID]) REFERENCES [dbo].[Entity] ([ID]),
    CONSTRAINT [FK_EntityDocuments_EntityDocumentTypes] FOREIGN KEY ([EntityDocumentTypeID]) REFERENCES [dbo].[EntityDocumentType] ([ID])
);

