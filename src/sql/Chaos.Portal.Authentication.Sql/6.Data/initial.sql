DELETE FROM EmailPassword;
DELETE FROM SecureCookie;
DELETE FROM AuthKey;
DELETE FROM WayfProfile;

INSERT INTO `EmailPassword`(`UserGUID`,`Password`,`DateCreated`,`DateModified`)
VALUES (0x34613336383661632D333562392D3131, '822f47daa187585d4e2475cad5cb06b4ed7f0ea7', UTC_TIMESTAMP(), UTC_TIMESTAMP());