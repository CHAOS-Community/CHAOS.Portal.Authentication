CREATE PROCEDURE SiteKey_Get 
(
	`Key`	VARCHAR(256)
)
BEGIN

	SELECT
		*
	FROM
		SiteKey
	WHERE
		SiteKey.`Key` = `Key`;

END