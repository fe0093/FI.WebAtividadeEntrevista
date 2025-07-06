CREATE PROC FI_SP_IncBeneficiarioV1  
    @NOME          VARCHAR (50) ,   
    @CPF           CHAR    (14) ,
    @IDCLIENTE     BIGINT,
    @ID            BIGINT
AS  
BEGIN
IF EXISTS (SELECT 1 FROM Beneficiarios WHERE ID = @ID)
BEGIN
    UPDATE Beneficiarios
    SET
        CPF = @CPF,
        Nome = @Nome
    WHERE ID = @ID
END
ELSE
BEGIN
    INSERT INTO Beneficiarios (IDCLIENTE, CPF, Nome)
    VALUES (@IDCLIENTE, @CPF, @Nome)
END
    SELECT SCOPE_IDENTITY()
END

DROP PROCEDURE FI_SP_IncBeneficiarioV1 