


-- =============================================
-- Author:		Bong Lee
-- Create date: Feb. 25, 2010
-- Description:	Check whether the session control history recrod has 
--              any related child records in other tables
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_IsPackagingSessionAlone]
	@vchPackagingLine varchar(10),
	@dteStartTime DateTime,
	@intShopOrder int = 0,
	@vchResult varchar(50) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;
	Declare @dteDateTime as varchar(19)

	BEGIN TRY
	
	select @dteDateTime = convert(varchar(19),@dteStartTime ,120)

	If exists ( 
		Select * from dbo.tblPalletHst where DefaultPkgLine = @vchPackagingLine and convert(varchar(19),StartTime,120) = @dteDateTime
	)
		Set @vchResult = 'Pallet' 
	Else
		If exists ( 
		Select * from dbo.tblDownTimeLog where MachineID = @vchPackagingLine and convert(varchar(19),DownTimeBegin,120) = @dteDateTime
		)
			Set @vchResult = 'Down Time'
		Else
			If exists ( 
			Select * from dbo.tblOperationStaffing where PackagingLine = @vchPackagingLine and convert(varchar(19),StartTime,120) = @dteDateTime
			)
				Set @vchResult = 'Utility Tech.'
			Else
				If exists ( 
				Select * from dbo.tblComponentScrap where ShopOrder = @intShopOrder and convert(varchar(19),StartTime,120) = @dteDateTime
				)
					Set @vchResult = 'Component Scrap'
				Else
					If exists ( 
					Select * from dbo.tblWeightLog where PackagingLine = @vchPackagingLine and ShopOrder = @intShopOrder and convert(varchar(19),ShopOrderStartTIme,120) = @dteDateTime
					)
						Set @vchResult = 'Weight Log'
					Else
						If exists ( 
						Select * from dbo.tblPLCLog where PackagingLine = @vchPackagingLine and ShopOrder = @intShopOrder and convert(varchar(19),ShopOrderStartTIme,120) = @dteDateTime
						)
							Set @vchResult = 'PLC Log'
						Else
							Set @vchResult = ''

	END TRY
	BEGIN CATCH
		DECLARE 
			@ErrorMessage    NVARCHAR(4000),
			@ErrorNumber     INT,
			@ErrorSeverity   INT,
			@ErrorState      INT,
			@ErrorLine       INT,
			@ErrorProcedure  NVARCHAR(200);
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

		RAISERROR 
			(
			@ErrorMessage, 
			@ErrorSeverity, 
			@ErrorState,     -- parameter: original error state.               
			@ErrorNumber,    -- parameter: original error number.
			@ErrorSeverity,  -- parameter: original error severity.
			@ErrorProcedure, -- parameter: original error procedure name.
			@ErrorLine       -- parameter: original error line number.
			);

	END CATCH
END

GO

