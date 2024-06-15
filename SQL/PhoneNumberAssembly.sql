CREATE ASSEMBLY [CLR_UDT_PhoneNr]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\PhoneNumber.dll'
WITH PERMISSION_SET = SAFE
GO

CREATE TYPE [dbo].[PhoneNumber] 
EXTERNAL NAME [CLR_UDT_PhoneNr].[PhoneNumber]
GO

CREATE TABLE PhoneNumbers
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    phoneNumber [dbo].[PhoneNumber]
)
GO

--ALTER ASSEMBLY [CLR_UDT_PhoneNr]
--FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\PhoneNumber.dll'
--GO

--DROP TABLE PhoneNumbers
--GO
--DROP TYPE [dbo].[PhoneNumber]
--GO
--DROP ASSEMBLY [CLR_UDT_PhoneNr]
--GO
