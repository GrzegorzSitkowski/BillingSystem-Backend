CREATE TABLE [dbo].[AccountUsers]
(
	[Id] INT IDENTITY (1,1) NOT NULL,
	[AccountId] INT NOT NULL,
	[UserId] INT NOT NULL,
	CONSTRAINT [PK_AccountUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_AccountUsers_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_AccountUsers_Accounts] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([Id]) ON DELETE CASCADE
)


GO
CREATE INDEX [IX_AccountUsers_AccountId] ON [dbo].[AccountUsers] ([AccountId])

GO
CREATE INDEX [IX_AccountUsers_UserId] ON [dbo].[AccountUsers] ([UserId])
