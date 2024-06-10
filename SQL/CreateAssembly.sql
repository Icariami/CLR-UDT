CREATE ASSEMBLY [CLR_UDT]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\IBANAccountNumber.dll'
WITH PERMISSION_SET = SAFE
GO

DROP TYPE [dbo].[IBANAccountNumber]
GO
DROP ASSEMBLY [CLR_UDT]
GO

CREATE TYPE [dbo].[IBANAccountNumber] 
EXTERNAL NAME [CLR_UDT].[IBANAccountNumber]
GO

DROP TABLE BankAccounts
GO
 -- DROP ASSEMBLY CLR_UDT;
ALTER ASSEMBLY [CLR_UDT]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\IBANAccountNumber.dll'


CREATE TABLE BankAccounts
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    iban [dbo].[IBANAccountNumber]
)
GO

INSERT INTO BankAccounts VALUES (CONVERT([dbo].[IBANAccountNumber], 'PL12123456781234123412341234,Gosia Makiela,543.54'));

SELECT iban.ToString() FROM BankAccounts;

