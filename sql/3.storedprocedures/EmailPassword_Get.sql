CREATE PROCEDURE EmailPassword_Get
(
    UserGUID	BINARY(16),
    Password	VARCHAR(1024)
)
BEGIN

	SELECT	
		*
	FROM	
		EmailPassword AS EP
	WHERE	
			( UserGUID IS NULL OR EP.UserGUID = UserGUID )
        AND ( Password IS NULL OR EP.Password = Password );

END