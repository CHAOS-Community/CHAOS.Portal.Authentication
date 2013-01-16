CREATE TABLE EmailPassword
(
	UserGUID		binary(16) NOT NULL,
	Password		varchar(1024) NOT NULL,
	DateCreated		datetime NOT NULL,
	DateModified	datetime DEFAULT NULL,
	PRIMARY KEY (UserGUID),
	UNIQUE  KEY UserGUID_UNIQUE (UserGUID)
) 
ENGINE=MyISAM