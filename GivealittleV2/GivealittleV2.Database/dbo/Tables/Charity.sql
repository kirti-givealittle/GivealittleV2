CREATE TABLE [dbo].[Charity] (
    [ID]       UNIQUEIDENTIFIER NOT NULL,
    [CCNumber] NCHAR (10)       NOT NULL,
    CONSTRAINT [PK_Charity] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Charity_Entity] FOREIGN KEY ([ID]) REFERENCES [dbo].[Entity] ([ID])
);

