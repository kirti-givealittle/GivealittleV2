CREATE TABLE [dbo].[BankAccountDocument] (
    [ID]            UNIQUEIDENTIFIER NOT NULL,
    [BankAccountID] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_BankAccountDocument] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BankAccountDocument_BankAccount] FOREIGN KEY ([BankAccountID]) REFERENCES [dbo].[BankAccount] ([ID])
);

