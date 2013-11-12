CREATE PROCEDURE SiteKey_Create 
(
	`Key`		VARCHAR(255),
	UserGuid	BINARY(16),
	Name		VARCHAR(255)
)
BEGIN

	INSERT INTO SiteKey	(`Key`,	`UserGuid`,	`Name`)	
	VALUES	(`Key`,	UserGuid, Name);

	SELECT ROW_COUNT();

END