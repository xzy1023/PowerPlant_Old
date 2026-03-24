
-- =============================================================================
-- Author:		Bong Lee
-- Create date: Sept. 05, 2006
-- Description:	Shop Order I/O Module
-- Mod: #1 
--		Make action 'GetSOList' and 'GetNextSO' to be facility specific. 
--      That handle shop order drop down box for Ajax. 
-- Mod: #2
--		If the item of the shop order consist of more one blend, just pick one of them.
-- WO#21        Mar. 23, 2010   Bong Lee
-- Description: Print Case Label with Shop Order Number, add facility
-- WO#274		Jan. 10, 2014	Bong Lee
--Description:	Show shop order on SODescription only if blend and grind are all null or blank.	
-- WO#1297		Nov. 18, 2014	Bong Lee
-- Description:	To handle AX does not provide Blend and Grid information
-- FX150505		May  05, 2015	Bong Lee
-- Description:	Show shop order from shop order history table
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_ShopOrderIO] 
	-- Add the parameters for the stored procedure here
	@chrAction varchar(30),
	@chrFacility char(3),
	@intShopOrder int = 0,
	@chrPackagingLine char(10) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	If @chrAction = 'GetSO&Item'
	BEGIN
		SELECT     T1.*, T2.*,
		(CONVERT(datetime, (CONVERT(char(10),T1.StartDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.StartTime)) AS StartDateTime,
		(CONVERT(datetime, (CONVERT(char(10),T1.EndDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.EndTime)) AS EndDateTime,
		'' AS Blend,'' AS Grind,'' AS SODescription
		FROM        tblShopOrder AS T1 
			LEFT OUTER JOIN tblItemMaster AS T2 
			ON T1.Facility = T2.Facility AND T1.ItemNumber = T2.ItemNumber		-- WO#21
		WHERE  T1.Facility = @chrFacility AND (T1.ShopOrder = @intShopOrder)	-- WO#21
	END
	Else
	BEGIN
		-- Get next available Shop Order by Planned Start Time
		If @chrAction = 'GetNextSO'
		BEGIN
/* WO#1297 DEL Start
			IF @chrFacility <> '02' and @chrFacility <> '20'
			BEGIN
				SELECT TOP 1 T1.*,T2.* , 
				(CONVERT(datetime, (CONVERT(char(10),T1.StartDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.StartTime)) AS StartDateTime,
				(CONVERT(datetime, (CONVERT(char(10),T1.EndDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.EndTime)) AS EndDateTime,
				T3.Blend, T3.Grind,'' AS SODescription
				FROM dbo.tblShopOrder AS T1 
				LEFT OUTER JOIN tblItemMaster AS T2 
				ON T1.Facility = T2.Facility AND T1.ItemNumber = T2.ItemNumber	-- WO#21
				LEFT OUTER JOIN (Select ShopOrder, Min(Blend) as Blend, Min(Grind) as Grind From tblBillOfMaterials 
					Where Blend <> '' Group By ShopOrder) AS T3		-- Mod#2
				ON T1.ShopOrder = T3.ShopOrder 
				WHERE T1.Facility = @chrFacility								-- WO#21
					AND T1.PackagingLine = @chrPackagingLine 
					AND T1.Closed IS NULL
					AND T1.StartDate IS NOT NULL
					--AND T3.Blend <> ''
				ORDER BY T1.StartDate, T1.StartTime, T1.ShopOrder
			END
			ELSE
			BEGIN
WO#1297 DEL Stop */
				SELECT TOP 1 T1.*,T2.* , 
				(CONVERT(datetime, (CONVERT(char(10),T1.StartDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.StartTime)) AS StartDateTime,
				(CONVERT(datetime, (CONVERT(char(10),T1.EndDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.EndTime)) AS EndDateTime,
				'' AS Blend, '' As Grind, LOWER(RTRIM(CAST(T1.ShopOrder as varchar(10))) + ' ' + RTRIM(T2.ItemDesc1) + ' ' + RTRIM(T2.ItemDesc2) + ' ' + LTRIM(RTRIM(T2.PackSize))) AS SODescription
				FROM dbo.tblShopOrder AS T1 
				LEFT OUTER JOIN tblItemMaster AS T2	
				ON T1.Facility = T2.Facility AND T1.ItemNumber = T2.ItemNumber	-- WO#21
				WHERE T1.Facility = @chrFacility								-- WO#21
					AND T1.PackagingLine = @chrPackagingLine 
					AND T1.Closed IS NULL
					AND T1.StartDate IS NOT NULL
					AND T2.ItemDesc1 <> ''
					AND T2.ItemDesc2 <> ''
					AND T2.PackSize <> ''
				ORDER BY T1.StartDate, T1.StartTime, T1.ShopOrder
--WO#1297 DEL Stop */END
		END
		Else
		-- Get next available Shop Order by Planned Start Time
		If @chrAction = 'GetSOList'
		BEGIN
		/* WO#1297 DEL Start
			IF @chrFacility <> '02' and @chrFacility <> '20'
			BEGIN
				SELECT T1.*,T2.* , 
				(CONVERT(datetime, (CONVERT(char(10),T1.StartDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.StartTime)) AS StartDateTime,
				(CONVERT(datetime, (CONVERT(char(10),T1.EndDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.EndTime)) AS EndDateTime,
				T3.Blend, T3.Grind, CASE WHEN T3.Grind <> '' THEN CAST(T1.ShopOrder as varchar(10)) + ' Bl:' + T3.Blend + ' Gr:' + T3.Grind ELSE 
				CASE WHEN T3.Blend <> '' THEN CAST(T1.ShopOrder as varchar(10)) + ' Bl:' + T3.Blend ELSE CAST(T1.ShopOrder as varchar(10)) END END AS SODescription	--WO#274
--WO#274			CAST(T1.ShopOrder as varchar(10)) + ' Bl:' + T3.Blend END AS SODescription
				FROM dbo.tblShopOrder AS T1 
				LEFT OUTER JOIN tblItemMaster AS T2 
				ON T1.Facility = T2.Facility AND T1.ItemNumber = T2.ItemNumber	-- WO#21
				LEFT OUTER JOIN (Select ShopOrder, Min(Blend) as Blend, Min(Grind) as Grind From tblBillOfMaterials 
					Where Blend <> '' Group By ShopOrder) AS T3		-- Mod#2
				ON T1.ShopOrder = T3.ShopOrder 
				WHERE T1.Facility = @chrFacility	-- WO#21
					AND T1.PackagingLine = @chrPackagingLine 
					AND T1.Closed IS NULL
					AND T1.StartDate IS NOT NULL
--WO#274			AND T3.Blend <> ''
				ORDER BY T1.StartDate, T1.StartTime, T1.ShopOrder
			END
			ELSE
			BEGIN
		WO#1297 DEL Stop */
				SELECT T1.*,T2.* , 
				(CONVERT(datetime, (CONVERT(char(10),T1.StartDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.StartTime)) AS StartDateTime,
				(CONVERT(datetime, (CONVERT(char(10),T1.EndDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.EndTime)) AS EndDateTime,
				'' AS Blend, '' As Grind, LOWER(RTRIM(CAST(T1.ShopOrder as varchar(10))) + ' ' + RTRIM(T2.ItemDesc1) + ' ' + RTRIM(T2.ItemDesc2) + ' ' + LTRIM(RTRIM(T2.PackSize))) AS SODescription
				FROM dbo.tblShopOrder AS T1 
				LEFT OUTER JOIN tblItemMaster AS T2 
				ON T1.Facility = T2.Facility AND T1.ItemNumber = T2.ItemNumber	-- WO#21
				WHERE T1.Facility = @chrFacility	-- WO#21
					AND T1.PackagingLine = @chrPackagingLine 
					AND T1.Closed IS NULL
					AND T1.StartDate IS NOT NULL
				ORDER BY T1.StartDate, T1.StartTime, T1.ShopOrder
			-- WO#1297 END
		END
			-- FX150505 ADD Start
			ELSE
			BEGIN
				IF  @chrAction = 'GetSOHist&Item'
				BEGIN
					SELECT     T1.*, T2.*,
					(CONVERT(datetime, (CONVERT(char(10),T1.StartDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.StartTime)) AS StartDateTime,
					(CONVERT(datetime, (CONVERT(char(10),T1.EndDate)),112) + dbo.fnCvtNumTimeToDateTime(T1.EndTime)) AS EndDateTime,
					'' AS Blend,'' AS Grind,'' AS SODescription
					FROM        tblShopOrderHst AS T1 
						LEFT OUTER JOIN tblItemMaster AS T2 
						ON T1.Facility = T2.Facility AND T1.ItemNumber = T2.ItemNumber		
					WHERE  T1.Facility = @chrFacility AND (T1.ShopOrder = @intShopOrder)
				END
			END
			-- FX150505 ADD Stop
	END
END

GO

