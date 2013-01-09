CREATE PROCEDURE SecureCookie_Get
(
    UserGUID			BINARY(16),
    SecureCookieGUID	BINARY(16),
    PasswordGUID		BINARY(16)
)
BEGIN

    SELECT	
		*
	FROM	
		SecureCookie AS SC
	WHERE	
			( UserGUID         IS NULL OR SC.UserGUID         = UserGUID ) 
		AND ( SecureCookieGUID IS NULL OR SC.SecureCookieGUID = SecureCookieGUID ) 
		AND ( PasswordGUID     IS NULL OR SC.PasswordGUID     = PasswordGUID );

END