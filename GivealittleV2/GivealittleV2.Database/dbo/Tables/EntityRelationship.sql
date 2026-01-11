CREATE TABLE [dbo].[EntityRelationship] (
    [ID]              UNIQUEIDENTIFIER NOT NULL,
    [EntityID]        UNIQUEIDENTIFIER NOT NULL,
    [RelatedEntityID] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_EntityRelationship] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_EntityRelationship_Individual] FOREIGN KEY ([EntityID]) REFERENCES [dbo].[Entity] ([ID]),
    CONSTRAINT [FK_EntityRelationship_NonIndividual] FOREIGN KEY ([RelatedEntityID]) REFERENCES [dbo].[Entity] ([ID])
);

