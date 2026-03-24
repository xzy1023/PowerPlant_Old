use master

BACKUP DATABASE [ImportData] TO  DISK = N'C:\SQL Database Backup\ImportData.Bak' WITH  DESCRIPTION = N'ImportData-Full Database Backup', NOFORMAT, INIT,  NAME = N'ImportData-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
GO

BACKUP DATABASE [LocalPowerPlant] TO  DISK = N'C:\SQL Database Backup\LocalPowerPlant.Bak' WITH  DESCRIPTION = N'LocalPowerPlant-Full Database Backup', NOFORMAT, INIT,  NAME = N'LocalPowerPlant-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
GO

BACKUP DATABASE [Master] TO  DISK = N'C:\SQL Database Backup\Master.Bak' WITH  DESCRIPTION = N'Master-Full Database Backup', NOFORMAT, INIT,  NAME = N'Master-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
GO

BACKUP DATABASE [MSDB] TO  DISK = N'C:\SQL Database Backup\MSDB.Bak' WITH  DESCRIPTION = N'MSDB-Full Database Backup', NOFORMAT, INIT,  NAME = N'MSDB-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
GO