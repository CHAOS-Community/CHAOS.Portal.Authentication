CREATE PROCEDURE AuthKey_Get 
(
	Token		VARCHAR(255),
	UserGuid	BINARY(16)
)
BEGIN

	SELECT
		*
	FROM
		AuthKey
	WHERE
		(Token IS NULL OR AuthKey.Token = Token)
		AND (UserGuid IS NULL OR AuthKey.UserGuid = UserGuid);

END