CREATE PROCEDURE Facebook_User_Join_Create
(
	FacebookUserId	BIGINT(20),
	UserGuid		BINARY(16)
)
BEGIN

	INSERT INTO 
		Facebook_User_Join (FacebookId, UserGuid, DateCreated)
	VALUES
		(FacebookUserId, UserGuid, NOW());

	SELECT ROW_COUNT();

END