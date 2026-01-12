CREATE TABLE [dbo].[Cause] (
    [ID]          UNIQUEIDENTIFIER NOT NULL,
    [Name]        VARCHAR (50)     NOT NULL,
    [Title]       VARCHAR (255)    NULL,
    [Description] VARCHAR (MAX)    NULL,
    [StartDate]   DATETIME         NULL,
    [EndDate]     DATETIME         NULL,
    CONSTRAINT [PK_Cause] PRIMARY KEY CLUSTERED ([ID] ASC)
);

