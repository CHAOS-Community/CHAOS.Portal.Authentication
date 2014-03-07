CREATE PROCEDURE OAuthUsers_Update
(
    UserGuid	BINARY(16),
    OAuthId	varchar(256)
)
BEGIN
	INSERT INTO OAuthUsers
    (
        UserGUID,
        OAuthId,
        DateCreated,
        DateModified
    )
    VALUES
    (
        UserGUID,
        OAuthId,
        NOW(),
        NULL
    )
	ON DUPLICATE KEY UPDATE 
		DateModified = NOW();

	SELECT ROW_COUNT();
END