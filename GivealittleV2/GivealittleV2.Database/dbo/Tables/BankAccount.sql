CREATE TABLE [dbo].[BankAccount] (
    [ID]            UNIQUEIDENTIFIER NOT NULL,
    [AccountNumber] SMALLINT         NULL,
    [Reference]     NVARCHAR (50)    NULL,
    [EntityID]      UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_BankAccount] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BankAccount_Entity] FOREIGN KEY ([EntityID]) REFERENCES [dbo].[Entity] ([ID])
);

