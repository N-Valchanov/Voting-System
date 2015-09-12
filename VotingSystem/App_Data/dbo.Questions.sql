USE VotingSystem;

CREATE TABLE [dbo].[Questions]
(
	[Id] INT NOT NULL IDENTITY, 
    [Content] NVARCHAR(250) NOT NULL, 
    [UrlId] CHAR(10) NOT NULL,
	RequireNames BIT NOT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC), 
)

GO

CREATE UNIQUE INDEX [UIX_Questions_UrlId] ON [dbo].[Questions] ([UrlId])
