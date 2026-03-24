
-- =============================================
-- FIX20160428: Mar 18, 2016	Bong Lee
-- Description:	Filter invalid Order Name.
-- =============================================

CREATE VIEW [dbo].[PRO_EXP_TRANSF_VIEW]
AS
SELECT			TRANSFERED, TRANSFERED_TIMESTAMP, RECORDING_DATE, DESTINATION, ORDER_NAME, BATCH_ID, MASTER_ID, CUSTOMER_CODE, SOURCE, 
                         S_PRODUCT_ID, S_CUSTOMER_CODE, S_TYPE_CELL, S_EMPTY, WEIGHT, START_FLAG, END_FLAG, PRO_EXPORT_GENERAL_ID AS ID, S_LOT_NAME
FROM            dbo.PRO_EXP_TRANSF
WHERE		Len(ORDER_NAME) > 7 AND IsNumeric([ORDER_NAME]) = 1						-- FIX20160428

GO

