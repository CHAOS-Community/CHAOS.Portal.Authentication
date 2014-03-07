CREATE PROCEDURE OAuthUsers_Get
(
    OAuthId	VARCHAR(256)
)
BEGIN
	SELECT	
		*
	FROM	
		OAuthUsers
	WHERE
		OAuthId = OAuthUsers.OAuthId
	LIMIT 1;
END