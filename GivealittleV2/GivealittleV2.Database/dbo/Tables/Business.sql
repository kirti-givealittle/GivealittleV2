CREATE TABLE [dbo].[Business] (
    [ID] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Business_NonIndividual1] FOREIGN KEY ([ID]) REFERENCES [dbo].[Entity] ([ID])
);

