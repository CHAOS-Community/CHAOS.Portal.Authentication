CREATE PROCEDURE `EmailPassword_Delete`(
	UserId BINARY(16)
)
BEGIN
	DELETE
	FROM
		EmailPassword
	WHERE
		EmailPassword.UserGUID = UserId;
        
	SELECT ROW_COUNT();
END