CREATE PROCEDURE EmailPassword_Update
(
    UserGUID	BINARY(16),
    Password	varchar(1024)
)
BEGIN

	UPDATE	
		EmailPassword AS EP
	SET	
		EP.Password     = Password,
        EP.DateModified = NOW()
	WHERE
		EP.UserGUID = UserGUID;

	SELECT ROW_COUNT();

END