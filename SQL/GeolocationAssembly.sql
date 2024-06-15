CREATE ASSEMBLY [CLR_UDT_Geo]
FROM 'C:\Users\gosia\Documents\C#\CLR-UDT\CLR_UDT_main_program\CLR_UDT_main_program\Geolocation.dll'
WITH PERMISSION_SET = SAFE
GO

CREATE TYPE [dbo].[Geo] 
EXTERNAL NAME [CLR_UDT_Geo].[Geolocation]
GO

CREATE TABLE Geolocations
(
    ID int IDENTITY(1,1) PRIMARY KEY,
    geolocation [dbo].[Geo]
)
GO

--DROP TABLE Geolocations
--GO
--DROP TYPE [dbo].[Geo]
--GO
--DROP ASSEMBLY [CLR_UDT_Geo]
--GO
