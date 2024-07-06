CREATE TABLE [dbo].[Device] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [SerialNumber] NVARCHAR (255) NOT NULL,
    [Type]         INT            NOT NULL,
    [LocationId]   INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id]),
    UNIQUE NONCLUSTERED ([SerialNumber] ASC)
);

