CREATE PROCEDURE SecureCookie_Update
(
    WhereUserGUID            BINARY(16),
    WhereSecureCookieGUID    BINARY(16),
    WherePasswordGUID        BINARY(16)
)
BEGIN

	UPDATE	
		SecureCookie AS SC
	SET	
		SC.DateUsed = COALESCE( SC.DateUsed, NOW() )
	WHERE	
			( WhereUserGUID         IS NULL OR SC.UserGUID         = WhereUserGUID )
		AND	( WhereSecureCookieGUID IS NULL OR SC.SecureCookieGUID = WhereSecureCookieGUID ) AND
		AND	( WherePasswordGUID     IS NULL OR SC.PasswordGUID     = WherePasswordGUID );
            
    SELECT ROW_COUNT();
    
END