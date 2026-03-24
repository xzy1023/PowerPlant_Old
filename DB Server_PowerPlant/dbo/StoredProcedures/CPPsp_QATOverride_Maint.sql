
-- =============================================
-- Author:		Bong Lee
-- Create date: Aug 08, 2018
-- Description:	WO#17432 Insert record to the QAT Override table
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATOverride_Maint]
		@dteBatchID			datetime		= NULL
		,@vchByPassLanes	varchar(30)		= NULL
		,@bitByPassTest		bit				= NULL
		,@vchFacility		varchar(3)		= NULL		
		,@vchOverridedBy	varchar(50)		= NULL
		,@dteOverrideID		datetime		= NULL
		,@vchPackagingLine	varchar(10)		= NULL
		,@intQATDefnID		int				= NULL
		,@intShopOrder		int				= NULL
		,@dteSOStartTime	datetime		= NULL
		,@bitAlert			bit				= NULL
AS
BEGIN

	BEGIN TRY
		INSERT INTO tblQATOverride
				(BatchID
				,ByPassLanes
				,ByPassTest
				,Facility
				,OverridedBy
				,OverrideID
				,PackagingLine
				,QATDefnID
				,ShopOrder
				,SOStartTime
				,Alert)
		VALUES  (@dteBatchID
				,@vchByPassLanes
				,@bitByPassTest
				,@vchFacility
				,@vchOverridedBy
				,@dteOverrideID
				,@vchPackagingLine
				,@intQATDefnID
				,@intShopOrder
				,@dteSOStartTime
				,@bitAlert
				)
	END TRY
	BEGIN CATCH
		THROW
	END CATCH
END

GO

