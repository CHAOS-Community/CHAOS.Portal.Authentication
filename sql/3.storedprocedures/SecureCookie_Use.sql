CREATE PROCEDURE SecureCookie_Use
(
    WhereUserGuid            BINARY(16),
    WhereSecureCookieGuid    BINARY(16),
    WherePasswordGuid        BINARY(16)
)
BEGIN

	UPDATE	
		SecureCookie AS SC
	SET	
		SC.DateUsed = COALESCE( SC.DateUsed, NOW() )
	WHERE	
			( WhereUserGuid         IS NULL OR SC.UserGUID         = WhereUserGuid )
		AND	( WhereSecureCookieGuid IS NULL OR SC.SecureCookieGUID = WhereSecureCookieGuid )
		AND	( WherePasswordGuid     IS NULL OR SC.PasswordGUID     = WherePasswordGuid );
            
    SELECT ROW_COUNT();
    
END