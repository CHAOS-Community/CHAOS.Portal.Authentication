CREATE TABLE Facebook_User_Join 
(
	FacebookId	BIGINT(20) NOT NULL,
	UserGuid	BINARY(16) NOT NULL,
	DateCreated	DATETIME NOT NULL,
	PRIMARY KEY (FacebookId, UserGuid),
	INDEX IX_FacebookId (FacebookId ASC)
);
