


-- =============================================
-- Author:		Bong Lee
-- Create date: April 11, 2008
-- Description:	Check Seeeion Control History
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_SessionControlHst_Chk]
	@vchAction as varchar(50),
	@vchFacility as varchar(3),
	@vchPkgLine as varchar(10), 
	@dteParmStartTime as datetime,
	@dteParmStopTime as datetime,
	@intCurrentRRN as int = 0,
	@vchMessage as varchar(256) OUTPUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @intShopOrder as int,
	@dteStartTime as datetime, 
	@dteStopTime as datetime 

	BEGIN TRY
--	select @vchFacility='01 ',@vchPkgLine='3180-01   ',@dteParmStartTime = '2009-09-16 13:00:00.000', @dteParmStopTime = '2009/09/16 14:00:00'
		If @vchAction = 'ChkOverlap'
		Begin
			Select top 1 @intShopOrder = ShopOrder, @dteStartTime = StartTime, @dteStopTime= StopTime 
			From tblSessionControlHst 
			Where Facility = @vchFacility and DefaultPkgLine = @vchPkgLine and StartTime <= @dteParmStartTime AND (@intCurrentRRN = 0 OR RRN <> @intCurrentRRN)
			ORDER BY StartTime desc

			-- Start time in the session
			IF @dteParmStartTime >= @dteStartTime and @dteParmStartTime <= @dteStopTime
				SET @vchMessage = 'Start time falls in another scession. => Shop Order: ' + Cast(@intShopOrder as varchar(10)) + ', Start: (' + Convert(varchar(19),@dteStartTime,120) + '), Stop: (' + Convert(varchar(19),@dteStopTime,120) + ')'		
			ELSE
			BEGIN
				SELECT top 1 @intShopOrder = ShopOrder, @dteStartTime = StartTime, @dteStopTime= StopTime From tblSessionControlHst 
				WHERE Facility = @vchFacility and DefaultPkgLine = @vchPkgLine and StartTime <= @dteParmStopTime AND (@intCurrentRRN = 0 OR RRN <> @intCurrentRRN)
				ORDER BY StartTime desc
				
				IF @dteParmStopTime >= @dteStartTime and @dteParmStopTime <= @dteStopTime
					SET @vchMessage = 'Stop time falls in another scession. => Shop Order: ' + Cast(@intShopOrder as varchar(10)) + ', Start: (' + Convert(varchar(19),@dteStartTime,120) + '), Stop: (' + Convert(varchar(19),@dteStopTime,120) + ')'		
				Else
					IF 	@dteParmStartTime <= @dteStartTime	
					SET @vchMessage = 'Start and Stop times bound another scession. => Shop Order: ' + Cast(@intShopOrder as varchar(10)) + ', Start: (' + Convert(varchar(19),@dteStartTime,120) + '), Stop: (' + Convert(varchar(19),@dteStopTime,120) + ')'		
			END
		END
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
End

GO

