CREATE PROC FI_SP_AltBeneficiarioV1  
    @NOME          VARCHAR (50) ,  
    @Id            BIGINT,  
    @CPF     CHAR    (14)  
AS  
BEGIN  
 UPDATE BENEFICIARIOS   
 SET   
  NOME = @NOME,    
  CPF = @CPF  
 WHERE Id = @Id  
END