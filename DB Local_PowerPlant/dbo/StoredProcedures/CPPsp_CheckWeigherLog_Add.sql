
-- =============================================
-- Author:		Bong Lee
-- Create date: Jan 01, 2019
-- Description:	WO#17432 Insert record to the checkweigher log
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_CheckWeigherLog_Add] 	
	@decActualWeight		decimal(7,3)
	,@chrPackagingLine		char(10)
	,@intShopOrder			int
	,@dteSOStartTime		datetime
	,@dteTestTime			datetime
AS
BEGIN

	BEGIN TRY
		INSERT INTO tblCheckWeigherLog
				(ActualWeight
				,PackagingLine
				,ShopOrder
				,SOStartTime
				,TestTime)
			VALUES (
				@decActualWeight
				,@chrPackagingLine
				,@intShopOrder
				,@dteSOStartTime
				,@dteTestTime)
	END TRY
	BEGIN CATCH
		THROW
	END CATCH
END

GO

