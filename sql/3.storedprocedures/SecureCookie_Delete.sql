CREATE PROCEDURE SecureCookie_Delete
(
    WhereUserGUID            BINARY(16),
    WhereSecureCookieGUID    BINARY(16)
)
BEGIN

	DELETE
	FROM
		SecureCookie
	WHERE	
		    UserGUID	     = WhereUserGUID 
		AND SecureCookieGUID = WhereSecureCookieGUID;
                  
    SELECT ROW_COUNT();

END