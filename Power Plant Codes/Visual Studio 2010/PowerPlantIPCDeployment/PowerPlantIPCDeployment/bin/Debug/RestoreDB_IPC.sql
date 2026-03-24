USE [master]
RESTORE DATABASE [Importdata] FROM  DISK = N'C:\SQL Database Backup\ImportData.bak' WITH  FILE = 1,  MOVE N'ImportData' TO N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Importdata.mdf',  MOVE N'ImportData_log' TO N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Importdata_1.ldf',  NOUNLOAD,  REPLACE,  STATS = 5

GO

RESTORE DATABASE [LocalPowerPlant] FROM  DISK = N'C:\SQL Database Backup\LocalPowerPlant.Bak' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 5

GO