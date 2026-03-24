
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct. 09, 2018
-- Description:	Select Dynamic Label Data
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_DynamicLabelData_Sel]
	@vchLabelKey varchar(50) = NULL
	,@chrRecordType char(1) = NULL
	,@vchPackagingLine varchar(10) = NULL
	,@intShopOrder int = NULL

AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY 
		SELECT	*
		FROM	 dbo.tblDynamicLabelData
		WHERE																			
			LabelKey = ISNULL(@vchLabelKey,LabelKey)
			AND	RecordType = ISNULL(@chrRecordType,RecordType)						
			AND	PackagingLine = ISNULL(@vchPackagingLine,PackagingLine)
			AND ShopOrder = ISNULL(@intShopOrder,ShopOrder)
	END TRY
	BEGIN CATCH
		THROW
	END CATCH
END

GO

