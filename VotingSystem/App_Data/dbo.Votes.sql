USE VotingSystem;

CREATE TABLE [dbo].[Votes] (
    [Id]         INT          NOT NULL,
    [AnswerId]   INT          NOT NULL,
    [QuestionId] INT          NOT NULL,
    [Ip]         VARCHAR (40) NOT NULL,
    [SecretKey]  VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Votes_Answers] FOREIGN KEY ([AnswerId]) REFERENCES [Answers]([Id]), 
    CONSTRAINT [FK_Votes_Questions] FOREIGN KEY ([QuestionId]) REFERENCES [Questions]([Id])
);

USE VotingSystem
GO

ALTER TABLE [dbo].[Votes]
ADD FullName nvarchar(50) NULL
GO