CREATE TABLE AuthKey 
(
  `Token` VARCHAR(255) NOT NULL,
  `UserGuid` BINARY(16) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Key`),
  UNIQUE INDEX `Token_UNIQUE` (`Token` ASC),
  UNIQUE INDEX `UserGuid_Name_Unique` (`UserGuid` ASC, `Name` ASC)
);
