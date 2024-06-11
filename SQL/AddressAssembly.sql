CREATE ASSEMBLY [CLR_UDT_Address]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\Address.dll'
WITH PERMISSION_SET = SAFE
GO

ALTER ASSEMBLY [CLR_UDT_Address]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\Address.dll'
GO

DROP ASSEMBLY [CLR_UDT_Address]
GO

DROP TABLE Addresses
GO

DROP TYPE [dbo].[Address]
GO

CREATE TYPE [dbo].[Address] 
EXTERNAL NAME [CLR_UDT_Address].[Address]
GO

CREATE TABLE Addresses
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    addres [dbo].[Address]
)
GO


INSERT INTO Addresses VALUES (convert([dbo].[Address], 'aaaaa'))
GO

SELECT addres.ToString() FROM Addresses
GO
