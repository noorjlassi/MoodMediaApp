CREATE TABLE [dbo].[Location] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (255) NOT NULL,
    [Address]  NVARCHAR (MAX) NOT NULL,
    [ParentId] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Company] ([Id])
);

