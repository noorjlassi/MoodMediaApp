CREATE TABLE [dbo].[Company] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (255) NOT NULL,
    [Code]      NVARCHAR (50)  NOT NULL,
    [Licensing] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Code] ASC)
);

