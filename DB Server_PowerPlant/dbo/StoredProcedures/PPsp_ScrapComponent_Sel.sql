
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov.29, 2011
-- Description: Scrap Oomponent Selection
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ScrapComponent_Sel]
	@vchAction varchar(50) = NULL
	,@vchFacility varchar(3) = NULL
	,@vchMachineID varchar(10) = NULL
	,@vchOperator varchar(10) = NULL
	,@intShopOrder int = NULL
	,@dteShiftProductionDate datetime = NULL
	,@intShiftSeq int = NULL
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY
	SELECT tCS.*, tSCH.DefaultPkgLine, tSCH.Operator, tPS.FirstName + '' + tPS.LastName as OperatorName FROM dbo.tblComponentScrap tCS
	INNER JOIN dbo.tfnSessionControlHstDetail(NULL,@vchFacility, @vchMachineID,@vchOperator,@intShopOrder,NULL,@dteShiftProductionDate,@intShiftSeq,@dteShiftProductionDate,@intShiftSeq) tSCH
	ON tCS.Facility  = tSCH.Facility  AND
	   tCS.ShopOrder = tSCH.ShopOrder AND
	   tCS.StartTime = tSCH.StartTime
	LEFT OUTER JOIN tbLPlantStaff as tPS
	ON tCS.Facility  = tPS.Facility AND
	   tSCH.Operator = tPS.StaffID
	Order By ShopOrder,StartTime,DefaultPkgLine,OperatorName

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

END

GO

