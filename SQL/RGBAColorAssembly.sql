CREATE ASSEMBLY [CLR_UDT_RGBA]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\RGBAColor.dll'
WITH PERMISSION_SET = SAFE
GO

CREATE TYPE [dbo].[RGBA] 
EXTERNAL NAME [CLR_UDT_RGBA].[RGBAColor]
GO

CREATE TABLE RGBAs
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    rgba [dbo].[RGBA]
)
GO

--ALTER ASSEMBLY [CLR_UDT_RGBA]
--FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\RGBAColor.dll'

--DROP TABLE RGBAs
--GO
--DROP TYPE [dbo].[RGBA]
--GO
--DROP ASSEMBLY [CLR_UDT_RGBA]
--GO


