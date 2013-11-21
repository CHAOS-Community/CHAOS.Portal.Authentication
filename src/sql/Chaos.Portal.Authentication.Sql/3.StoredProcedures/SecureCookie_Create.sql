CREATE PROCEDURE SecureCookie_Create
(
    UserGuid            BINARY(16),
    SecureCookieGuid    BINARY(16),
    PasswordGuid        BINARY(16),
    SessionGuid         BINARY(16)
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
		( SecureCookieGuid, PasswordGuid, UserGuid, SessionGuid, NOW()      , NULL );

	SELECT	ROW_COUNT();

END