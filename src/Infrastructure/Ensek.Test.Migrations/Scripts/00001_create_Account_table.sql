CREATE TABLE dbo.Account
(
	Id int NOT NULL,
	FirstName varchar(50) NOT NULL,
	LastName varchar(50) NOT NULL,
	CONSTRAINT PK_Account PRIMARY KEY CLUSTERED (Id ASC),
	INDEX IX_Account NONCLUSTERED (FirstName ASC, LastName ASC)
);