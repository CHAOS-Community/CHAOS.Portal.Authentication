CREATE PROCEDURE AuthKey_Delete
(
	Name		VARCHAR(255),
	UserGuid	BINARY(16)
)
BEGIN

	DELETE
	FROM
		AuthKey
	WHERE
		AuthKey.Name = Name
		AND AuthKey.UserGuid = UserGuid
	LIMIT 1;
		
	SELECT ROW_COUNT();

END