CREATE TABLE dbo.MeterReading
(
	Id int IDENTITY(1,1) NOT NULL,
	ReadingDateTime datetime NOT NULL,
	ReadingValue int NOT NULL,
	AccountId int NOT NULL,
	CONSTRAINT PK_MeterReading PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_MeterReading_Account FOREIGN KEY (AccountId) REFERENCES Account (Id),
	INDEX IX_MeterReading NONCLUSTERED (ReadingDateTime ASC, ReadingValue ASC, AccountId ASC)
);