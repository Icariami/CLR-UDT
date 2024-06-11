CREATE ASSEMBLY [CLR_UDT_RGBA]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\RGBAColor.dll'
WITH PERMISSION_SET = SAFE
GO

DROP TYPE [dbo].[RGBA]
GO
DROP ASSEMBLY [CLR_UDT_RGBA]
GO

CREATE TYPE [dbo].[RGBA] 
EXTERNAL NAME [CLR_UDT_RGBA].[RGBAColor]
GO

DROP TABLE RGBAColors
GO
 -- DROP ASSEMBLY CLR_UDT;
ALTER ASSEMBLY [CLR_UDT_RGBA]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\RGBAColor.dll'


CREATE TABLE RGBAColors
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    color [dbo].[RGBA]
)
GO

INSERT INTO RGBAColors VALUES (cast(('23;43;49;0,3') as [dbo].[RGBA]))
GO


SELECT color.ToString() from RGBAColors
GO


