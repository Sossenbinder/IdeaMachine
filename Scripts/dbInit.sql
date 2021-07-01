CREATE TABLE "Ideas" (
	"Id" int IDENTITY(1,1) PRIMARY KEY,
	"ShortDescription" VARCHAR(400) NOT NULL,
	"LongDescription" VARCHAR(5000),
	"CreationDate" datetime2 NOT NULL,
	"Creator" uniqueidentifier NOT NULL,
);