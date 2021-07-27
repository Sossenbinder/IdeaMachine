CREATE TABLE "Ideas" (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShortDescription] [varchar](400) NOT NULL,
	[LongDescription] [varchar](5000) NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[Creator] [uniqueidentifier] NOT NULL
);

CREATE TABLE Reactions (
    UserID UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    IdeaId int NOT NULL,
    LikeState int NOT NULL,
);