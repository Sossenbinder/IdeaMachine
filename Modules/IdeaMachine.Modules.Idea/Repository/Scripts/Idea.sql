CREATE TABLE "Ideas" (
	"Id" SERIAL PRIMARY KEY,
	"ShortDescription" VARCHAR(400) NOT NULL,
	"LongDescription" VARCHAR(5000),
	"CreationDate" timestamp NOT NULL,
	"Creator" UUID NOT NULL,
);

CREATE TABLE Reactions (
    UserID UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    PostId int NOT NULL,
    LikeState TINYINT NOT NULL,
);