CREATE TABLE [dbo].[SchoolStudent] (
    [IndividualID] UNIQUEIDENTIFIER NOT NULL,
    [SchoolID]     UNIQUEIDENTIFIER NOT NULL,
    [ClassID]      UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_SchoolStudent_Individual] FOREIGN KEY ([IndividualID]) REFERENCES [dbo].[Individual] ([ID]),
    CONSTRAINT [FK_SchoolStudent_School] FOREIGN KEY ([SchoolID]) REFERENCES [dbo].[School] ([ID]),
    CONSTRAINT [FK_SchoolStudent_SchoolClass] FOREIGN KEY ([ClassID]) REFERENCES [dbo].[SchoolClass] ([ID])
);

