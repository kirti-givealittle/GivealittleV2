CREATE TABLE [dbo].[EntityRelationshipRole] (
    [ID]                   UNIQUEIDENTIFIER NOT NULL,
    [Title]                VARCHAR (50)     NOT NULL,
    [Description]          VARCHAR (50)     NULL,
    [EntityRelationshipID] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_EntityRelationshipRole] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_EntityRelationshipRole_EntityRelationship] FOREIGN KEY ([EntityRelationshipID]) REFERENCES [dbo].[EntityRelationship] ([ID])
);

