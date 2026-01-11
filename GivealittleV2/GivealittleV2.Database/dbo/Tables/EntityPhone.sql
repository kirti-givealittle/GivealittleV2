CREATE TABLE [dbo].[EntityPhone] (
    [ID]        UNIQUEIDENTIFIER NOT NULL,
    [Number]    NVARCHAR (50)    NOT NULL,
    [IsDefault] BIT              NOT NULL,
    [EntityID]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ContactPhone] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_EntityPhone_Entity] FOREIGN KEY ([EntityID]) REFERENCES [dbo].[Entity] ([ID])
);

