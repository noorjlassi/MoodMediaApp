CREATE TABLE [dbo].[ApplicationLogs] (
    [LogID]     INT          IDENTITY (1, 1) NOT NULL,
    [Timestamp] DATETIME     NOT NULL,
    [LogLevel]  VARCHAR (50) NOT NULL,
    [Message]   TEXT         NOT NULL,
    [Exception] TEXT         NULL,
    PRIMARY KEY CLUSTERED ([LogID] ASC)
);

