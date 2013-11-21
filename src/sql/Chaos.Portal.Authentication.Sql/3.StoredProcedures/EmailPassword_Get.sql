CREATE PROCEDURE EmailPassword_Get
(
    UserGuid        BINARY(16),
    Password        VARCHAR(1024)
)
BEGIN

        SELECT        
                *
        FROM        
                EmailPassword AS EP
        WHERE        
                        ( UserGuid IS NULL OR EP.UserGUID = UserGuid )
        AND ( Password IS NULL OR EP.Password = Password );

END