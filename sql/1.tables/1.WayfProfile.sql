CREATE TABLE WayfProfile 
(
	UserGuid			binary(16) NOT NULL,
	WayfId				varchar(256) NOT NULL,
	DateCreated		datetime NOT NULL,
	DateModified		datetime DEFAULT NULL,
	PRIMARY KEY (UserGuid),
	UNIQUE INDEX (WayfId)
) 
ENGINE=MyISAM