


-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 10, 2006
-- Description:	Pallet History I/O Module
-- WO#181:		Bong Lee	Create date: Mar 24, 2011
-- Description:	Add Facility filter option
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_PalletHstIO]
	-- Add the parameters for the stored procedure here
	@vchAction varchar(30), 
	@intPalletID int,
	@chrPkgLine char(10)='',
	@intShopOrder int = 0,
	@vchFacility varchar(3)	-- WO#181
	
AS
BEGIN
	DECLARE @vchSQLStmt  varchar(350);
	DECLARE @vchWhere  varchar(100);
	DECLARE @vchOrderBy varchar(100)
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
--	SET @vchSQLStmt = 'SELECT * '
----				,CreationDate + CreationTime as CreationOrder, substring(CreationDate,1,4) + ''/''+ substring(CreationDate,5,2) + ''/'' + 
----				substring(CreationDate,7,2) + '' '' + substring(CreationTime,1,2) + '':'' +  substring(CreationTime,3,2) as CreationDateTime FROM tblPalletHst'
--	
	If @vchAction = 'AllBy_Line_SO'
	BEGIN
--		-- Both Packaging line and shop order are not specified 
--		--IF @chrPkgLine = '' and @intShopOrder = 0
--			--Set @vchOrderBy = ' ORDER BY DefaultPkgLine, ShopOrder, CreationOrder'
--		--ELSE
--		-- Packaging line specified only
--			IF @intShopOrder = 0
--			BEGIN
--				Set @vchWhere = ' WHERE DefaultPkgLine = ''' + @chrPkgLine + ''''
--				--Set @vchOrderBy = ' ORDER BY DefaultPkgLine, ShopOrder, CreationOrder'
--			END
--			ELSE
--				-- Shop Order specified only
--				IF @chrPkgLine = '' OR @chrPkgLine is NULL
--				BEGIN
--					Set @vchWhere = ' WHERE  ShopOrder = ' + CAST(@intShopOrder AS varchar)
--					--Set @vchOrderBy = ' ORDER BY  ShopOrder, DefaultPkgLine, CreationOrder'
--				END
--				ELSE
--					BEGIN
--					-- Packaging line and Shop order specified
--						Set @vchWhere = ' WHERE DefaultPkgLine = ''' + @chrPkgLine + ''' AND ShopOrder = ' + CAST(@intShopOrder AS varchar)
--						--Set @vchOrderBy = ' ORDER BY CreationOrder'
--					END
--	Set @vchOrderBy = ' ORDER BY DefaultPkgLine, PalletID'
--	EXEC (@vchSQLStmt + @vchWhere + @vchOrderBy)

		SELECT * from tblPalletHst
		WHERE (@intShopOrder = 0 or ShopOrder = @intShopOrder)
			AND (@chrPkgLine  = '' or DefaultPkgLine = @chrPkgLine )
			AND (@vchFacility is NULL OR Facility = @vchFacility )	-- WO#181
			ORDER BY DefaultPkgLine, PalletID
		
	END
	Else
		If @vchAction = 'AllByPalletID'
		SELECT *
--			, convert(datetime,substring(CreationDate,1,4) + '/'+ substring(CreationDate,5,2) + '/' + substring(CreationDate,7,2) + ' ' +
--			substring(CreationTime,1,2) + ':' +  substring(CreationTime,3,2)+ ':'+ substring(CreationTime,5,2),102) as CreationDateTime 
			FROM dbo.tblPalletHst
-- WO#181	WHERE PalletID = @intPalletID
			WHERE (@vchFacility is NULL OR Facility = @vchFacility )	-- WO#181
				AND PalletID = @intPalletID								-- WO#181
			ORDER BY PalletID
		Else
		  If @vchAction = 'AllBy_Line_SO_with_Adj'
		  Begin
			 SELECT tPH.*, Isnull((Select Sum(AdjustedQty) 
						From tblPalletAdjustment tPA 
-- WO#181				Where tPA.PalletID = tPH.PalletID 
				WHERE (@vchFacility is NULL OR Facility = @vchFacility )	-- WO#181
					AND tPA.PalletID = tPH.PalletID							-- WO#181
				Group by tPA.PalletID),0) as AdjustedQty,
				-- if workgroup not P or All, put a non-blank value there otherwise the column named Column1 in .net repeater 
					Case When not (tPS.WorkGroup = 'P' or tPS.WorkGroup = 'ALL') OR tPS.WorkGroup Is null Then 'NA' Else tPS.WorkGroup End as WorkGroup,
					tPS.FirstName + ' ' + tPS.LastName as OperatorName 
			 FROM tblPalletHst tPH
			 Left Outer Join tblPlantStaff tPS
				On tPH.Facility = tPS.Facility and tPH.Operator = tPS.StaffID	
			 WHERE (@intShopOrder = 0 or ShopOrder = @intShopOrder)
			   AND (@chrPkgLine  = '' or DefaultPkgLine = @chrPkgLine )
 			   AND (@vchFacility is NULL OR tPH.Facility = @vchFacility )	-- WO#181
			 ORDER BY DefaultPkgLine, StartTime, PalletID
		  End 
	
END

GO

