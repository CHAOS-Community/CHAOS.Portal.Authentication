CREATE PROCEDURE AuthKey_Create 
(
	Token		VARCHAR(255),
	UserGuid	BINARY(16),
	Name		VARCHAR(255)
)
BEGIN

	INSERT INTO AuthKey	(`Token`,	`UserGuid`,	`Name`)	
	VALUES	(Token,	UserGuid, Name);

	SELECT ROW_COUNT();

END