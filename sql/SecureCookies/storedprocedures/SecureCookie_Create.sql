CREATE PROCEDURE SecureCookie_Create
(
    UserGUID            BINARY(16),
    SecureCookieGUID    BINARY(16),
    PasswordGUID        BINARY(16),
    SessionGUID         BINARY(16)
)
BEGIN

	DELETE
	FROM	
		SecureCookie
	WHERE	
		SecureCookie.DateUsed < NOW() - 90;
    
    INSERT INTO 
		SecureCookie
		( SecureCookieGUID, PasswordGUID, UserGUID, SessionGUID, DateCreated, DateUsed)
    VALUES             
		( SecureCookieGUID, PasswordGUID, UserGUID, SessionGUID, NOW()      , NULL );

	SELECT	ROW_COUNT();

END