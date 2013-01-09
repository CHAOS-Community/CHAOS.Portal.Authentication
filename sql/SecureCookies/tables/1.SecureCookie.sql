CREATE TABLE SecureCookie 
(
	UserGUID binary(16) NOT NULL,
	SecureCookieGUID binary(16) NOT NULL,
	PasswordGUID binary(16) NOT NULL,
	SessionGUID binary(16) NOT NULL,
	DateCreated datetime NOT NULL,
	DateUsed datetime DEFAULT NULL,
  PRIMARY KEY (UserGUID, PasswordGUID, SecureCookieGUID)
) 
ENGINE=MyISAM DEFAULT CHARSET=utf8