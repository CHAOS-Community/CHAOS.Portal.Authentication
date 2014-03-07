CREATE TABLE OAuthUsers 
(
	UserGuid			binary(16) NOT NULL,
	OAuthId				varchar(256) NOT NULL,
	DateCreated			datetime NOT NULL,
	DateModified		datetime DEFAULT NULL,
	PRIMARY KEY (OAuthId),
	UNIQUE INDEX (OAuthId)
) 
ENGINE=MyISAM