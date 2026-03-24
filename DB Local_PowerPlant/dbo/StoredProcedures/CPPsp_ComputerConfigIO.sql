
-- =============================================
-- Author:		Bong Lee
-- Create date: Sept. 05, 2006
-- Description:	Computer Config. Table I/O Module
-- POAP 74 – Down Time Log Maintenance
--			 Jan 12,2009 Bong Lee
-- WO# 755 – Add action for all lines include lines on virtual IPC
--			 Jul 26,2013 Bong Lee
-- FIX20170119: Jan. 19, 2017
--				Change the parameter length of Computer Name to varchar 50
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_ComputerConfigIO] 
	-- Add the parameters for the stored procedure here
	@chrAction varchar(30),
	-- FIX20170119	@chrComputerName char(10) = NULL,	-- POAP 74 Chg
	@chrComputerName varchar(50) = NULL,	-- FIX20170119
	@vchMachineID varchar(10) = NULL	-- POAP 74 Add
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY
		If @chrAction = 'SelectAllFields'
		BEGIN
			SELECT  *
			FROM    dbo.tblComputerConfig
			WHERE   (@chrComputerName is Null OR ComputerName = @chrComputerName) AND 
					(@vchMachineID is Null or PackagingLine = @vchMachineID) AND		-- POAP 74 Add:
					RecordStatus = 1
		END
-- WO#755 Add: Begin
		ELSE
			If @chrAction = 'AllActiveInclVirtual'
			BEGIN
				SELECT  *
				FROM    dbo.tblComputerConfig
				WHERE   (@chrComputerName is Null OR ComputerName = @chrComputerName) AND 
						(@vchMachineID is Null OR PackagingLine = @vchMachineID) AND
						(RecordStatus = 1 OR virtualIPC = 1)
			END
			ELSE
-- WO#755 Add: End
-- POAP 74 Add: Begin
		If @chrAction = 'AllMachines'
		BEGIN
			SELECT  Top 1 *
			FROM    dbo.tblComputerConfig
			WHERE   (@chrComputerName is Null OR ComputerName = @chrComputerName) AND 
					(@vchMachineID is Null or PackagingLine = @vchMachineID)		
					Order By RecordStatus
-- POAP 74 Add: End
		END
-- POAP 74 Add: Begin	
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage, -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH;
-- POAP 74 Add: End
END

GO

