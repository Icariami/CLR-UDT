CREATE ASSEMBLY [CLR_UDT_IBAN]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\IBANAccountNumber.dll'
WITH PERMISSION_SET = SAFE
GO

DROP TYPE [dbo].[IBANAccountNumber]
GO
DROP ASSEMBLY [CLR_UDT_IBAN]
GO

CREATE TYPE [dbo].[IBANAccountNumber] 
EXTERNAL NAME [CLR_UDT_IBAN].[IBANAccountNumber]
GO

DROP TABLE BankAccounts
GO
 -- DROP ASSEMBLY CLR_UDT;
ALTER ASSEMBLY [CLR_UDT_IBAN]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\IBANAccountNumber.dll'


CREATE TABLE BankAccounts
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    iban [dbo].[IBANAccountNumber]
)
GO
