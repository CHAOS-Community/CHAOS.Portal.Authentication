CREATE PROCEDURE SecureCookie_Delete
(
    WhereUserGuid            BINARY(16),
    WhereSecureCookieGuid    BINARY(16)
)
BEGIN

	DELETE
	FROM
		SecureCookie
	WHERE	
		    UserGUID	     = WhereUserGuid
		AND SecureCookieGUID = WhereSecureCookieGuid;
                  
    SELECT ROW_COUNT();

END