CREATE TABLE [dbo].[SchoolClass] (
    [ID]              UNIQUEIDENTIFIER NOT NULL,
    [ClassYear]       INT              NOT NULL,
    [ClassRoomNumber] INT              NOT NULL,
    CONSTRAINT [PK_SchoolClass] PRIMARY KEY CLUSTERED ([ID] ASC)
);

