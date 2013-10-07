CREATE PROCEDURE WayfProfile_Get
(
    WayfId	VARCHAR(1024)
)
BEGIN

	SELECT	
		*
	FROM	
		WayfProfile AS WP
	WHERE
		WayfId = WP.WayfId;
	LIMIT 1;

END