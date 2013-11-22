CREATE TABLE Facebook_User_Join 
(
	FacebookUserId	BIGINT(20) NOT NULL,
	UserGuid		BINARY(16) NOT NULL,
	DateCreated		DATETIME NOT NULL,
	PRIMARY KEY (FacebookUserId, UserGuid),
	INDEX IX_FacebookUserId (FacebookUserId ASC)
);
