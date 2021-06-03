CREATE TABLE "Ideas" (
	"Id" SERIAL PRIMARY KEY,
	"ShortDescription" VARCHAR(400) NOT NULL,
	"LongDescription" VARCHAR(5000),
	"CreationDate" timestamp NOT NULL,
	"CreatorMail" VARCHAR(500),
);