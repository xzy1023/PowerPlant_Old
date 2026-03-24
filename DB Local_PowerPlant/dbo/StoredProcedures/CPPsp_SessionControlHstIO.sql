

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 15, 2006
-- Description:	Session Control History IO
-- POAP 74: Down Time Log Maintenance
--			Jan 13,2009 Bong Lee
-- WO#359: Handle multi-facilities in the Item Master table
--			Feb 07,2012 Bong Lee
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_SessionControlHstIO] 
	-- Add the parameters for the stored procedure here
	@vchAction VARCHAR(50),
	@chrPkgLine char(10) = NULL,
	@intShopOrder int = 0,
	@intShift int = 0,
	@dteShiftProductionDate datetime = NULL,
	@dteDateTime datetime = NULL,
	@chrFacility char(3) = NULL,
	@vchOperator varchar(10) = NULL,
	@intRRN int = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @dteShiftStart as datetime
	DECLARE @dteShiftEnd as datetime
	DECLARE @dteLatestStartTime as datetime

  BEGIN TRY
    -- Insert statements for procedure here
	IF @vchAction = 'SelectLastRecord'
	BEGIN
		SELECT TOP 1 T1.*, T2.ItemDesc1 AS ItemDesc,
			T3.FirstName + ' ' + T3.LastName AS OperatorName
		FROM tblSessionControlhst T1
		LEFT OUTER JOIN dbo.tblItemMaster T2
--WO#359	ON T1.ItemNumber = T2.ItemNumber
		ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility --WO#359
		LEFT OUTER JOIN dbo.tblPlantStaff T3
--WO#359	ON T1.Operator = T3.StaffID
		ON T1.Operator = T3.StaffID AND T1.Facility = T3.Facility --WO#359
		WHERE T1.DefaultPkgLine = @chrPkgLine AND EXISTS 
			(SELECT Max(StartTime) FROM tblSessionControlhst T2
			 WHERE 	T2.DefaultPkgLine = @chrPkgLine
			 HAVING Max(StartTime) = T1.StartTime )
	END
	ELSE
	--IF @vchAction = 'LastRec_Line_SO_Shift_PrdDate'
	IF @vchAction = 'LastRec_Line_SO_Shift'
	BEGIN
--		SELECT @dteShiftStart = ShiftStartTime, @dteShiftEnd = ShiftEndTime from dbo.fnShiftInfo(@chrFacility,@dteDateTime,@intShift,NULL,@chrPkgLine)
		SELECT TOP 1 T1.*, T2.ItemDesc1 AS ItemDesc,
			T3.FirstName + ' ' + T3.LastName AS OperatorName
		FROM tblSessionControlhst T1
		LEFT OUTER JOIN dbo.tblItemMaster T2
--WO#359	ON T1.ItemNumber = T2.ItemNumber
		ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility --WO#359
		LEFT OUTER JOIN dbo.tblPlantStaff T3
--WO#359	ON T1.Operator = T3.StaffID
		ON T1.Operator = T3.StaffID AND T1.Facility = T3.Facility --WO#359
		WHERE T1.DefaultPkgLine = @chrPkgLine AND EXISTS 
			(SELECT Max(StartTime) FROM tblSessionControlhst T2
			 WHERE 	T2.DefaultPkgLine = @chrPkgLine
				AND T2.ShopOrder = @intShopOrder
--				AND T2.StartTime Between @dteShiftStart and @dteShiftEnd	-- for reporting
				AND T2.OverrideShiftNo = @intShift
				AND ShiftProductionDate = @dteShiftProductionDate
			 HAVING Max(StartTime) = T1.StartTime )
	END
		ELSE
		IF @vchAction = 'SelectLastRecByLineSO'
		BEGIN
			SELECT TOP 1 T1.*, T2.ItemDesc1 AS ItemDesc,
				T3.FirstName + ' ' + T3.LastName AS OperatorName
			FROM tblSessionControlhst T1
			LEFT OUTER JOIN dbo.tblItemMaster T2
--WO#359	ON T1.ItemNumber = T2.ItemNumber
			ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility --WO#359
			LEFT OUTER JOIN dbo.tblPlantStaff T3
--WO#359	ON T1.Operator = T3.StaffID
			ON T1.Operator = T3.StaffID AND T1.Facility = T3.Facility --WO#359
			WHERE T1.DefaultPkgLine = @chrPkgLine 
				AND T1.ShopOrder = @intShopOrder 
			ORDER BY StartTime Desc
		END
		ELSE
			-- Find Carried Forward Cases when change on Operator/Shop Order/Shift
			IF @vchAction = 'CarriedForwardCases_Opr_SO_Shift'
			BEGIN
				SELECT @dteShiftStart = ShiftStartTime, @dteShiftEnd = ShiftEndTime from dbo.fnShiftInfo(@chrFacility,@dteDateTime,@intShift,NULL,@chrPkgLine)

				-- Find the last time the shop order was started by not the current operator in the shift.
				-- If the shop order is found in the current shift, use it and do not need to search
				-- previous shifts

				SELECT @dteLatestStartTime = Max(StartTime) FROM tblSessionControlhst
					 WHERE 	DefaultPkgLine = @chrPkgLine
						AND ShopOrder = @intShopOrder
						AND OverrideShiftNo = @intShift
						AND ShiftProductionDate = @dteShiftProductionDate
--						AND StartTime Between @dteShiftStart and @dteShiftEnd	-- for reporting
						AND Operator <> @vchOperator

				-- Assuming whenever shift change, loose cases will be recorded in shift that before shift change.
				-- So no matter who starts the shop order on the subsequence shift, the loose cases will become 
				-- the carried forward cases on the shift 

				IF @dteLatestStartTime is NULL
				Begin

					SELECT @dteLatestStartTime = Max(StartTime) FROM tblSessionControlhst
						 WHERE 	DefaultPkgLine = @chrPkgLine
							AND ShopOrder = @intShopOrder
							AND StartTime < @dteShiftStart 
				End

				SELECT TOP 1 T1.*, T2.ItemDesc1 AS ItemDesc,
					T3.FirstName + ' ' + T3.LastName AS OperatorName
				FROM tblSessionControlhst T1
				LEFT OUTER JOIN dbo.tblItemMaster T2
--WO#359		ON T1.ItemNumber = T2.ItemNumber
				ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility --WO#359
				LEFT OUTER JOIN dbo.tblPlantStaff T3
--WO#359		ON T1.Operator = T3.StaffID
				ON T1.Operator = T3.StaffID AND T1.Facility = T3.Facility --WO#359
				WHERE T1.DefaultPkgLine = @chrPkgLine AND StartTime = @dteLatestStartTime 
			END
			ELSE
			IF @vchAction = 'CarriedForwardCases_SO_Shift'
				BEGIN
--					SELECT @dteShiftStart = ShiftStartTime, @dteShiftEnd = ShiftEndTime from dbo.fnShiftInfo(@chrFacility,@dteDateTime,@intShift,NULL,@chrPkgLine)

					-- The first record of the shop order in the current shift should have the Carried Forward Cases for the Shop Order in the shift

					SELECT @dteLatestStartTime = Min(StartTime) FROM tblSessionControlhst
						 WHERE 	DefaultPkgLine = @chrPkgLine
							AND ShopOrder = @intShopOrder
							AND OverrideShiftNo = @intShift
							AND ShiftProductionDate = @dteShiftProductionDate
--							AND StartTime Between @dteShiftStart and @dteShiftEnd	-- for reporting

					-- Assuming whenever shift change, loose cases will be recorded and counted as that shift.
					-- So no matter who starts the shop order on the subsequence shift, the loose cases will become 
					-- the carried forward cases on the shift. 

					IF @dteLatestStartTime is not NULL
					Begin
						SELECT top 1 T1.*, T2.ItemDesc1 AS ItemDesc,
							T3.FirstName + ' ' + T3.LastName AS OperatorName
						FROM tblSessionControlhst T1
						LEFT OUTER JOIN dbo.tblItemMaster T2
--WO#359				ON T1.ItemNumber = T2.ItemNumber
						ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility --WO#359
						LEFT OUTER JOIN dbo.tblPlantStaff T3
--WO#359				ON T1.Operator = T3.StaffID
						ON T1.Operator = T3.StaffID AND T1.Facility = T3.Facility --WO#359	
						WHERE T1.DefaultPkgLine = @chrPkgLine 
							AND ShopOrder = @intShopOrder 
							AND StartTime = @dteLatestStartTime 
					End
				END
			ELSE
			IF @vchAction = 'SelectAllRecords'
			BEGIN
				SELECT T1.*, T2.ItemDesc1 AS ItemDesc,
					T3.FirstName + ' ' + T3.LastName AS OperatorName
				FROM tblSessionControlhst T1
				LEFT OUTER JOIN dbo.tblItemMaster T2
--WO#359		ON T1.ItemNumber = T2.ItemNumber
				ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility --WO#359
				LEFT OUTER JOIN dbo.tblPlantStaff T3
--WO#359		ON T1.Operator = T3.StaffID
				ON T1.Operator = T3.StaffID AND T1.Facility = T3.Facility --WO#359
				ORDER BY RRN
			END
			ELSE
				-- Did the shop order has packed more than 1 case?
				IF @vchAction = 'SelectLastNonZeroRecByLineSO'
				BEGIN
					SELECT TOP 1 T1.*, T2.ItemDesc1 AS ItemDesc,
						T3.FirstName + ' ' + T3.LastName AS OperatorName
					FROM tblSessionControlhst T1
					LEFT OUTER JOIN dbo.tblItemMaster T2
--WO#359			ON T1.ItemNumber = T2.ItemNumber
					ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility --WO#359
					LEFT OUTER JOIN dbo.tblPlantStaff T3
--WO#359			ON T1.Operator = T3.StaffID
					ON T1.Operator = T3.StaffID AND T1.Facility = T3.Facility --WO#359
					WHERE T1.DefaultPkgLine = @chrPkgLine 
						AND T1.ShopOrder = @intShopOrder 
						AND CasesProduced <> 0
					ORDER BY StartTime Desc
				END
				ELSE
					-- Select by facility, optional shop order, optional packaging line
					IF @vchAction = 'BySoLine'
					BEGIN
						SELECT T1.*, T2.ItemDesc1 AS ItemDesc,
							T3.FirstName + ' ' + T3.LastName AS OperatorName
						FROM tblSessionControlhst T1
						LEFT OUTER JOIN dbo.tblItemMaster T2
						ON T1.Facility = T2.Facility and T1.ItemNumber = T2.ItemNumber
						LEFT OUTER JOIN dbo.tblPlantStaff T3
						ON T1.Facility = T3.Facility and T1.Operator = T3.StaffID
						WHERE T1.facility = @chrFacility
							AND (@chrPkgLine is null or T1.DefaultPkgLine = @chrPkgLine)
							AND (@intShopOrder = 0 or T1.ShopOrder = @intShopOrder)
						ORDER BY T1.DefaultPkgLine,StartTime
					END
					ELSE
						-- Select by Relative Record Number
						IF @vchAction = 'ByRRN'
						BEGIN
							SELECT * FROM tblSessionControlhst 
								WHERE RRN = @intRRN
						END	
						ELSE
-- POAP 74: Add Begin
						-- Select by Production Shift
						IF @vchAction = 'ByShift'
						BEGIN
							SELECT T1.*, T2.ItemDesc1 AS ItemDesc,
								T3.FirstName + ' ' + T3.LastName AS OperatorName
							FROM tblSessionControlhst T1
							LEFT OUTER JOIN dbo.tblItemMaster T2
							ON T1.Facility = T2.Facility and T1.ItemNumber = T2.ItemNumber
							LEFT OUTER JOIN dbo.tblPlantStaff T3
							ON T1.Facility = T3.Facility and T1.Operator = T3.StaffID
							WHERE T1.facility = @chrFacility
								AND OverrideShiftNo = @intShift
								AND ShiftProductionDate = @dteShiftProductionDate
								AND (@chrPkgLine is null or T1.DefaultPkgLine = @chrPkgLine)
								AND (@vchOperator is null or Operator = @vchOperator)
							ORDER BY StartTime
						END

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
	END CATCH
-- POAP 74: Add End
END

GO

