CREATE PROCEDURE WayfProfile_Get
(
    WayfId	VARCHAR(256)
)
SELECT	
	*
FROM	
	WayfProfile AS WP
WHERE
	WayfId = WP.WayfId
LIMIT 1;