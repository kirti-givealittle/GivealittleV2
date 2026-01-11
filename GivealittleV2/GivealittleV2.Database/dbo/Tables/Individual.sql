CREATE TABLE [dbo].[Individual] (
    [ID]          UNIQUEIDENTIFIER CONSTRAINT [DF_Individual_Id] DEFAULT (newsequentialid()) NOT NULL,
    [FirstName]   NVARCHAR (50)    NOT NULL,
    [MiddleName]  NVARCHAR (50)    NULL,
    [LastName]    NVARCHAR (50)    NOT NULL,
    [DateOfBirth] DATETIME         NOT NULL,
    [IsMinor]     BIT              NULL,
    CONSTRAINT [PK_Individual] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Individual_Entity] FOREIGN KEY ([ID]) REFERENCES [dbo].[Entity] ([ID])
);

