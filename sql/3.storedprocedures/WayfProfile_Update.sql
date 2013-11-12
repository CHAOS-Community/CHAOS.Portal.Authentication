CREATE PROCEDURE WayfProfile_Update
(
    UserGuid			BINARY(16),
    WayfId				varchar(1024),
	GivenName		varchar(1024) NOT NULL,
	SurName			varchar(1024) NOT NULL,
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
END