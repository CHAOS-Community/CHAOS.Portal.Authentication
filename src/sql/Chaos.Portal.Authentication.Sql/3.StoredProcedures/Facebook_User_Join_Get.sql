CREATE PROCEDURE Facebook_User_Join_Get
(
	FacebookUserId	BIGINT(20),
  UserId BINARY(16)
)
BEGIN

	SELECT
		*
	FROM
		Facebook_User_Join
	WHERE
		    (FacebookUserId IS NULL OR Facebook_User_Join.FacebookUserId = FacebookUserId)
		AND (UserId IS NULL OR Facebook_User_Join.UserGuid = UserId);

END