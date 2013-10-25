CREATE TABLE WayfProfile 
(
	UserGuid			binary(16) NOT NULL,
	WayfId				varchar(1024) NOT NULL,
	GivenName		varchar(1024) NOT NULL,
	SurName			varchar(1024) NOT NULL,
	CommonName	varchar(1024) NOT NULL,
	DateCreated		datetime NOT NULL,
	DateModified		datetime DEFAULT NULL,
	PRIMARY KEY (UserGuid),
	UNIQUE INDEX (WayfId)
) 
ENGINE=MyISAM