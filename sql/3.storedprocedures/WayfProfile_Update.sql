CREATE PROCEDURE WayfProfile_Update
(
    UserGuid			BINARY(16),
    WayfId				varchar(1024),
	GivenName		varchar(1024) NOT NULL,
	SurName			varchar(1024) NOT NULL,
	CommonName	varchar(1024) NOT NULL,
)
BEGIN
	INSERT INTO WayfProfile
    (
        UserGUID,
        WayfId,
		GivenName,
		SurName,
		CommonName,
        DateCreated,
        DateModified
    )
    VALUES
    (
        UserGUID,
        WayfId,
		GivenName,
		SurName,
		CommonName,
        NOW(),
        NULL
    )
	ON DUPLICATE KEY UPDATE 
		GivenName = GivenName, 
		SurName = SurName, 
		CommonName = CommonName, 
		DateModified = NOW();

	SELECT ROW_COUNT();
END