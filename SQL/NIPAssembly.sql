CREATE ASSEMBLY [CLR_UDT_NIP]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\NIP.dll'
WITH PERMISSION_SET = SAFE
GO

CREATE TYPE [dbo].[NIP] 
EXTERNAL NAME [CLR_UDT_NIP].[NIP]
GO

CREATE TABLE NIPs
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    nip [dbo].[NIP]
)
GO

--DROP TABLE NIPs
--GO
--DROP TYPE [dbo].[NIP]
--GO
--DROP ASSEMBLY [CLR_UDT_NIP]
--GO

--ALTER ASSEMBLY [CLR_UDT_NIP]
--FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\NIP.dll'





