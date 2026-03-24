
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov 1, 2007
-- Description:	Delete selected Ground Cofee Packaging Activity Record
-- =============================================
CREATE PROCEDURE [dbo].[LTsp_SltedPkgActivityDel]
	-- Add the parameters for the stored procedure here
	@vchProcEnv	varchar(10),
	@vchGCLotID varchar(30),
	@intPalletID int = NULL,
	@vchCmptBlend varchar(11) = NULL,
	@vchUserID varchar(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	DECLARE @vchServerName varchar(50);
	DECLARE @vchServerSQLStmt varchar(500);
	DECLARE @vchSQLStmt varchar(550);
	DECLARE @vchUserLib varchar(10);
	DECLARE @vchOriginalLib varchar(10)

	BEGIN TRY
		If @vchProcEnv = 'PRD'
		BEGIN
			Select @vchServerName = value1 From tblControl Where [key] = 'iSeriesNames'
			Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibPRD'
		END
		ELSE
		BEGIN
			If @vchProcEnv = 'UA'
			BEGIN
				Select @vchServerName = value2 From tblControl Where [key] = 'iSeriesNames'
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibUA'
			END
			ELSE
			BEGIN
				Select @vchServerName = value2 From tblControl Where [key] = 'iSeriesNames'
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibDev'
			END
		END 
		
		IF @intPalletID is NULL AND @vchCmptBlend is NULL
			SET @vchSQLStmt = 'DELETE FROM ' + @vchUserLib + '.FSPA$ WHERE (SPGCLOT = ''' + @vchGCLotID + ''') '
		Else
			SET @vchSQLStmt = 'DELETE FROM ' + @vchUserLib + '.FSPA$ WHERE (SPGCLOT = ''' + @vchGCLotID + ''') ' +
				'AND (SPPALID = ' + CAST(@intPalletID AS varchar(9)) + ') ' + 
				'AND (SPCBLND = ''' + @vchCmptBlend + ''')'

		IF @vchServerName = 'S105HF5M'
			EXEC (@vchSQLStmt) at S105HF5M
		ELSE
			IF @vchServerName = 'S10A8379'
				EXEC (@vchSQLStmt) at S10A8379

PRINT @vchSQLStmt
		BEGIN TRY
			EXEC (@vchSQLStmt)
		END TRY
		BEGIN CATCH
			DECLARE @ErrorMessage NVARCHAR(4000);
			DECLARE @ErrorSeverity INT;
			DECLARE @ErrorState INT;
			SELECT 
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();

			if PATINDEX('%FSPA%',@ErrorMessage)=0 or ERROR_Number()<>208
				RAISERROR (@ErrorMessage,@ErrorSeverity, @ErrorState )
		END CATCH

		if (@vchUserID) IS NOT NULL
			Update dbo.tblGreenCoffeeLot SET UpdatedBy = @vchUserID, DateLastUpdated = getdate() WHERE GCLotID = @vchGCLotID
	END TRY
	BEGIN CATCH
		PRINT ERROR_MESSAGE()
	END CATCH
END

GO

