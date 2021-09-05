CREATE TABLE [dbo].[Ideas] (
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ShortDescription] [varchar](400) NOT NULL,
	[LongDescription] [varchar](5000) NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[Creator] [uniqueidentifier] NOT NULL
);

CREATE TABLE [dbo].[Reactions](
	[UserID] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[IdeaId] [int] NOT NULL,
	[LikeState] [int] NULL
);

CREATE TABLE [dbo].[Tags](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[IdeaId] [int] NOT NULL FOREIGN KEY REFERENCES Ideas(Id),
	[Tag] [varchar](200) NOT NULL
);

CREATE TABLE [dbo].[AttachmentUrls](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[IdeaId] [int] NOT NULL FOREIGN KEY REFERENCES Ideas(Id),
	[AttachmentUrl] [varchar](500) NOT NULL
);