CREATE PROCEDURE Facebook_User_Join_Get
(
	FacebookUserId	BIGINT(20)
)
BEGIN

	SELECT
		*
	FROM
		Facebook_User_Join
	WHERE
		Facebook_User_Join.FacebookUserId = FacebookUserId;

END