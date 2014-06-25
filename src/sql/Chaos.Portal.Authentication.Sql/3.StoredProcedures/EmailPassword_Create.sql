CREATE PROCEDURE EmailPassword_Create
(
    UserGUID	BINARY(16),
    Password	VARCHAR(1024)
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
        Password,
        UTC_TIMESTAMP(),
        UTC_TIMESTAMP()
    );

	SELECT ROW_COUNT();
END