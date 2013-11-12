DELIMITER ;;

CREATE PROCEDURE WayfProfile_Update
(
    UserGuid			BINARY(16),
    WayfId				varchar(256),
	GivenName		varchar(1024),
	SurName			varchar(1024)
)
BEGIN
	INSERT INTO WayfProfile
    (
        UserGUID,
        WayfId,
		GivenName,
		SurName,
        DateCreated,
        DateModified
    )
    VALUES
    (
        UserGUID,
        WayfId,
		GivenName,
		SurName,
        NOW(),
        NULL
    )
	ON DUPLICATE KEY UPDATE 
		GivenName = GivenName, 
		SurName = SurName, 
		DateModified = NOW();

	SELECT ROW_COUNT();
END;;