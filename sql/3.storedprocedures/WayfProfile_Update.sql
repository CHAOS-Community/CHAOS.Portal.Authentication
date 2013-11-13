DELIMITER ;;

CREATE PROCEDURE WayfProfile_Update
(
    UserGuid			BINARY(16),
    WayfId				varchar(256),
)
BEGIN
	INSERT INTO WayfProfile
    (
        UserGUID,
        WayfId,
        DateCreated,
        DateModified
    )
    VALUES
    (
        UserGUID,
        WayfId,
        NOW(),
        NULL
    )
	ON DUPLICATE KEY UPDATE 
		DateModified = NOW();

	SELECT ROW_COUNT();
END;;