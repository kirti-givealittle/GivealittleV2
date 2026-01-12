CREATE TABLE [dbo].[EntityAddress] (
    [ID]        UNIQUEIDENTIFIER NOT NULL,
    [IsDefault] BIT              NULL,
    [EntityID]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ContactAddress] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_EntityAddress_Entity] FOREIGN KEY ([EntityID]) REFERENCES [dbo].[Entity] ([ID])
);

