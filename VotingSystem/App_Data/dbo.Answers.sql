USE VotingSystem;

CREATE TABLE [dbo].[Answers] (
    [Id]         INT            NOT NULL IDENTITY,
    [QuestionId] INT            NOT NULL,
    [Content]    NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Answers_Questions] FOREIGN KEY ([QuestionId]) REFERENCES [Questions]([Id])
);

