CREATE PROCEDURE [dbo].[PPsp_ExportDataToIPCDB_Old]
@vchFacility NVARCHAR (3) NULL, @vchLogFileName NVARCHAR (200) NULL, @vchPowerPlantDB NVARCHAR (100) NULL
AS EXTERNAL NAME [ExportDataToIPCDB].[ExportDataToIPCDB.StoredProcedures].[CLRExportDataToIPC]


GO

