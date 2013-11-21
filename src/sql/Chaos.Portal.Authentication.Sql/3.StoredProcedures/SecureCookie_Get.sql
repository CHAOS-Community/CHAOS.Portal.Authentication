CREATE PROCEDURE SecureCookie_Get
(
    UserGuid			BINARY(16),
    SecureCookieGuid	BINARY(16),
    PasswordGuid		BINARY(16)
)
BEGIN

    SELECT	
		*
	FROM	
		SecureCookie AS SC
	WHERE	
			( UserGuid         IS NULL OR SC.UserGUID         = UserGuid ) 
		AND ( SecureCookieGuid IS NULL OR SC.SecureCookieGUID = SecureCookieGuid ) 
		AND ( PasswordGuid     IS NULL OR SC.PasswordGUID     = PasswordGuid );

END