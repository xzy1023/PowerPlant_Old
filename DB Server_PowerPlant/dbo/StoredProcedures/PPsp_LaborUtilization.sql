
-- =============================================
-- ID#13311		Mar.11 2022		Bong Lee
-- Description: lists the employees (their ID #) that worked on each shop order
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_LaborUtilization]

	@vchFacility varchar(3)
	,@vchPackagingLines as varchar(MAX) = NULL
	,@dteFromProdDate as DateTime
	,@dteToProdDate as DateTime

--declare 	@vchFacility varchar(3) = '07',
--	--@vchPackagingLines as varchar(MAX) = NULL,
--	@vchPackagingLines as varchar(MAX) = '3714-21,3715-30',
--	@dteFromProdDate as DateTime = '3/8/2022',
--	@dteToProdDate as DateTime = '3/8/2022'
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @tblUtilityTechList as TABLE (
		ShopOrder			int
		,ItemNumber			varchar(35)
		,DefaultPkgLine		varchar(10)
		,Operator			varchar(10)
		,ShiftDesc			varchar (50) 
		,StartTime			datetime
		,StopTime			datetime
		,CasesProduced		int
		,UtilityTechList	varchar(50)
	)

	DECLARE @IntShopOrder		as int
		,@vchItemNumber			as varchar(35)
		,@vchDefaultPkgLine		as varchar(10)
		,@vchOperator			as varchar(10)
		,@vchShiftDesc			as varchar(50)
		,@dteStartTime			as datetime
		,@dteStopTime			as datetime
		,@intCasesProduced		as int
		,@vchUtilityTech		as varchar(10)
		,@IntShopOrder_Save		as int
		,@vchItemNumber_Save	as varchar(35)
		,@vchDefaultPkgLine_Save as varchar(10)
		,@vchOperator_Save		as varchar(10)
		,@vchShiftDesc_Save		as varchar(50)
		,@dteStartTime_Save		as datetime
		,@dteStopTime_Save		as datetime
		,@intCasesProduced_Save	as int
		, @vchUtilityTechList	as varchar(50) = ''

	DECLARE object_cursor CURSOR FOR

		SELECT tSCHst.ShopOrder, ItemNumber, tSCHst.DefaultPkgLine, tSCHst.Operator, tSCHst.ShiftDesc, tSCHst.StartTime, tSCHst.StopTime
			,tSCHst.CasesProduced, tOS.StaffID 
		FROM dbo.tfnSessionControlHstDetail(NULL,@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,NULL,@dteToProdDate,NULL) tSCHst	
		LEFT OUTER JOIN [tblOperationStaffing] tOS
		ON tSCHst.facility = tOS.facility 
			AND tSCHst.DefaultPkgLine = tOS.PackagingLine 
			AND tSCHst.StartTime = tOS.StartTime
		WHERE (@vchPackagingLines IS NULL OR tSCHst.DefaultPkgLine IN (SELECT Value FROM STRING_SPLIT(@vchPackagingLines, ',')))
		GROUP BY tSCHst.ShopOrder, ItemNumber, tSCHst.DefaultPkgLine, tSCHst.StartTime, tSCHst.Operator, tSCHst.ShiftDesc, tSCHst.StopTime
			,tSCHst.CasesProduced, tOS.StaffID
		ORDER BY tSCHst.ShopOrder, tSCHst.DefaultPkgLine, tSCHst.StartTime, tSCHst.Operator

		OPEN Object_Cursor
		FETCH NEXT FROM Object_Cursor INTO @IntShopOrder, @vchItemNumber, @vchDefaultPkgLine, @vchOperator, @vchShiftDesc ,@dteStartTime ,@dteStopTime, @intCasesProduced, @vchUtilityTech
	
		-- Save the the key columns
		SELECT @IntShopOrder_Save = @IntShopOrder
			,@vchItemNumber_Save = @vchItemNumber
			,@vchDefaultPkgLine_Save = @vchDefaultPkgLine
			,@vchOperator_Save = @vchOperator
			,@vchShiftDesc_Save  = @vchShiftDesc 
			,@dteStartTime_Save = @dteStartTime
			,@dteStopTime_Save = @dteStopTime
			,@intCasesProduced_Save = @intCasesProduced

		WHILE @@FETCH_STATUS = 0
		BEGIN
	--select @IntShopOrder_Save , @IntShopOrder, @vchDefaultPkgLine_Save , @vchDefaultPkgLine, @vchOperator_Save , @vchOperator, @vchUtilityTech

			IF @IntShopOrder <> @IntShopOrder_Save OR @vchDefaultPkgLine <> @vchDefaultPkgLine_Save  OR @vchOperator <> @vchOperator_Save
				OR @vchShiftDesc <> @vchShiftDesc_Save OR @dteStartTime <> @dteStartTime_Save
			BEGIN
				IF LEFT(@vchUtilityTechList,1) = ','
				BEGIN
					SELECT @vchUtilityTechList = substring(@vchUtilityTechList, 2,len(@vchUtilityTechList) - 1)
				END

				INSERT INTO @tblUtilityTechList (ShopOrder, ItemNumber, DefaultPkgLine, Operator, ShiftDesc, StartTime, StopTime, CasesProduced, UtilityTechList)
					VALUES(@IntShopOrder_Save, @vchItemNumber_Save, @vchDefaultPkgLine_Save, @vchOperator_Save, @vchShiftDesc_Save, @dteStartTime_Save
						,@dteStopTime_Save, @intCasesProduced_Save, @vchUtilityTechList)
				SELECT @IntShopOrder_Save = @IntShopOrder
						,@vchItemNumber_Save = @vchItemNumber
						,@vchDefaultPkgLine_Save = @vchDefaultPkgLine
						,@vchOperator_Save = @vchOperator
						,@vchShiftDesc_Save  = @vchShiftDesc 
						,@dteStartTime_Save = @dteStartTime
						,@dteStopTime_Save = @dteStopTime
						,@intCasesProduced_Save = @intCasesProduced
						,@vchUtilityTechList = ''
			END
			SET @vchUtilityTechList = @vchUtilityTechList + ',' + ISNULL(@vchUtilityTech,'')
		
			FETCH NEXT FROM Object_Cursor INTO @IntShopOrder, @vchItemNumber, @vchDefaultPkgLine, @vchOperator, @vchShiftDesc ,@dteStartTime ,@dteStopTime, @intCasesProduced, @vchUtilityTech
		END

		IF @IntShopOrder_Save is not NULL
		BEGIN
			IF LEFT(@vchUtilityTechList,1) = ','
				BEGIN
					select @vchUtilityTechList = substring(@vchUtilityTechList, 2,len(@vchUtilityTechList) - 1)
				END
				INSERT INTO @tblUtilityTechList (ShopOrder, ItemNumber, DefaultPkgLine, Operator, ShiftDesc, StartTime, StopTime, CasesProduced, UtilityTechList)
					VALUES(@IntShopOrder_Save, @vchItemNumber_Save, @vchDefaultPkgLine_Save, @vchOperator_Save, @vchShiftDesc_Save, @dteStartTime_Save
						,@dteStopTime_Save, @intCasesProduced_Save, @vchUtilityTechList)
		END
		CLOSE object_cursor
		DEALLOCATE object_cursor

		SELECT tUTL.*, ROUND(DATEDIFF(second, tUTL.StartTime, tUTL.StopTIme)/3600.00,2) as RunHours, tEQ.[Description] as LineDesc
			,RTRIM(tIM.ItemDesc1) + ' ' + tIM.ItemDesc2 as ItemDesc 
		FROM @tblUtilityTechList tUTL
			LEFT OUTER JOIN tblEquipment tEQ
			ON tUTL.DefaultPkgLine = tEQ.EquipmentID
			LEFT OUTER JOIN tblItemMaster tIM
			ON tUTL.ItemNumber = tIM.ItemNumber
			WHERE tEQ.facility = @vchFacility and tIM.Facility = @vchFacility
		ORDER BY tUTL.DefaultPkgLine , tUTL.ShopOrder , tUTL.starttime

END

GO

