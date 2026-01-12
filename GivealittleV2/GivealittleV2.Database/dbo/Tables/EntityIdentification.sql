CREATE TABLE [dbo].[EntityIdentification] (
    [ID]                    UNIQUEIDENTIFIER NOT NULL,
    [DrivingLicenseNumer]   VARBINARY (4096) NULL,
    [DrivingLicenseVersion] VARBINARY (4096) NULL,
    [PassportNumber]        VARBINARY (4096) NULL,
    [PassportExpiryDate]    VARBINARY (4096) NULL,
    [DocumentID]            UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_EntityIdentification_Entity] FOREIGN KEY ([ID]) REFERENCES [dbo].[Entity] ([ID]),
    CONSTRAINT [FK_EntityIdentification_EntityDocuments] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[EntityDocument] ([ID])
);

