CREATE TABLE [dbo].[Entity] (
    [ID]           UNIQUEIDENTIFIER CONSTRAINT [DF_Entity_Id] DEFAULT (newsequentialid()) NOT NULL,
    [IsIndividual] BIT              NOT NULL,
    [IRDNumber]    VARCHAR (12)     NULL,
    CONSTRAINT [PK_Entity] PRIMARY KEY CLUSTERED ([ID] ASC)
);

