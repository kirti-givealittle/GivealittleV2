CREATE TABLE [dbo].[EntityEmail] (
    [ID]           UNIQUEIDENTIFIER CONSTRAINT [DF_EntityEmail_Id] DEFAULT (newsequentialid()) NOT NULL,
    [EmailAddress] NVARCHAR (50)    NOT NULL,
    [IsDefault]    BIT              NOT NULL,
    [EntityID]     UNIQUEIDENTIFIER NOT NULL,
    [IsVerified]   BIT              CONSTRAINT [DF_EntityEmail_IsVerified] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ContactEmail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_EntityEmail_Entity] FOREIGN KEY ([EntityID]) REFERENCES [dbo].[Entity] ([ID])
);

