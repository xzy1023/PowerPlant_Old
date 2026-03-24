
-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 17, 2007
-- Description:	Green Cofee Packaging Activity Selection
-- WO#275		Bong Lee		Aug. 13, 2013
-- Description:	Handle 7 digits shop order number
-- =============================================
CREATE PROCEDURE [dbo].[LTsp_SltedPkgActivityAdd]

	@vchProcEnv	varchar(10),
	@chrFacility as varchar(3),
	@vchGCLotID as varchar(30),
	@intPalletID as int,
	@vchCmptBlend as varchar(11),
	@vchFGBlend as varchar(11),
	@vchGrind as varchar(6),
	@chrPkgLine as char(10),
	@intShopOrder as int,
	@vchItemNumber as varchar(35),
	@intPalletDateCreated as int,
	@intPalletTimeCreated as int,
	@intPalletQty as int,
	@intAdjQty as int,
	@intWeight as int,
	@vchUserID as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @vchServerName as varchar(50);
	DECLARE @vchServerSQLStmt  varchar(500);
	DECLARE @vchSQLStmt  varchar(550);
	DECLARE @vchUserLib varchar(10);
	DECLARE @vchOriginalLib varchar(10);

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

		SET @vchSQLStmt = 'INSERT INTO ' + @vchUserLib + '.FSPA$' + 
			  ' (SPFAC, SPGCLOT, SPPALID, SPCBLND, SPFBLND, SPGRND, SPLINE, SPSORD, SPBLPRD, SPPCDT ,SPPCTM, SPPQTY, SPAQTY,SPWGHT,SPCDT)
				values(''' + @chrFacility + ''',''' +
				@vchGCLotID + ''',' +
				CAST(@intPalletID AS varchar(9)) + ',''' +
				@vchCmptBlend + ''',''' +
				@vchFGBlend + ''',''' +
				@vchGrind + ''',''' +
				@chrPkgLine + ''',' +
--WO#275		CAST(@intShopOrder AS varchar(6)) + ','''+	
				CAST(@intShopOrder AS varchar(10)) + ',''' +		--WO#275	
				@vchItemNumber + ''',' +
				CAST(@intPalletDateCreated AS varchar(8)) + ',' +
				CAST(@intPalletTimeCreated AS varchar(6)) + ',' +
				CAST(@intPalletQty AS varchar(12)) + ',' +
				CAST(@intAdjQty AS varchar(12)) + ',' +
				CAST(@intWeight AS varchar(16)) + ', Now())' 

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

			if substring(ERROR_MESSAGE(),2,3) <> 'Now' or ERROR_Number()<>195
				RAISERROR (@ErrorMessage,@ErrorSeverity, @ErrorState )
		END CATCH

		Update dbo.tblGreenCoffeeLot SET UpdatedBy = @vchUserID, DateLastUpdated = getdate() WHERE GCLotID = @vchGCLotID

	END TRY
	BEGIN CATCH
		PRINT ERROR_MESSAGE()
	END CATCH
END

GO

