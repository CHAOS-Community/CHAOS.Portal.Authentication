CREATE PROCEDURE EmailPassword_Update
(
    UserGUID	BINARY(16),
    NewPassword	varchar(1024)
)
BEGIN
	INSERT INTO EmailPassword
    (
        UserGUID,
        Password,
        DateCreated,
        DateModified
    )
    VALUES
    (
        UserGUID,
        NewPassword,
        UTC_TIMESTAMP(),
        UTC_TIMESTAMP()
    )
	ON DUPLICATE KEY UPDATE 
		Password = NewPassword, 
		DateModified = NOW();

	SELECT ROW_COUNT();
END