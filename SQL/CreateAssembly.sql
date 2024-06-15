CREATE ASSEMBLY [CLR_UDT_IBAN]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\IBAN.dll'
WITH PERMISSION_SET = SAFE
GO

CREATE TYPE [dbo].[IBAN] 
EXTERNAL NAME [CLR_UDT_IBAN].[IBAN]
GO

CREATE TABLE BankAccounts
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    iban [dbo].[IBAN]
)
GO

--ALTER ASSEMBLY [CLR_UDT_IBAN]
--FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\IBANAccountNumber.dll'

--DROP TABLE BankAccounts
--GO
--DROP TYPE [dbo].[IBAN]
--GO
--DROP ASSEMBLY [CLR_UDT_IBAN]
--GO
