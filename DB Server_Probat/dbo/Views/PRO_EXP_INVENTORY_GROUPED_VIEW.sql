 
 
/*=====================================================================================
8/16/2024 - Aliaksei Azarchankau. Helper view basedoff pro_exp_inventory_view to assist 
with retreving grouped values for IDD 076. 'GREEN' location records are grouped by
LOT_NAME, while the rest are grouped by MASTER_ID.
======================================================================================*/
CREATE VIEW [dbo].[PRO_EXP_INVENTORY_GROUPED_VIEW] AS
 
SELECT
	TRANSFERED,
	MAX(TRANSFERED_TIMESTAMP) AS TRANSFERED_TIMESTAMP,
	MASTER_ID,
	'' AS LOT_NAME,
	ZONE,
	CUSTOMER_CODE,
	SUM(AMOUNT) AMOUNT,
	MAX(ID) AS ID,
	COUNT(ID) AS NUMRECORDS,
	LOCATION
FROM
	PRO_EXP_INVENTORY_VIEW
WHERE 
	LOCATION != 'GREEN'
GROUP BY
	TRANSFERED,
	MASTER_ID,
	ZONE,
	CUSTOMER_CODE,
	LOCATION
 
UNION
 
 
SELECT
	TRANSFERED,
	MAX(TRANSFERED_TIMESTAMP) AS TRANSFERED_TIMESTAMP,
	'' AS MASTER_ID,
	LOT_NAME,
	ZONE,
	CUSTOMER_CODE,
	SUM(AMOUNT) AMOUNT,
	MAX(ID) AS ID,
	COUNT(ID) AS NUMRECORDS,
	LOCATION
FROM
	pro_exp_inventory_view
WHERE 
	LOCATION = 'GREEN'
GROUP BY
	TRANSFERED,
	LOT_NAME,
	ZONE,
	CUSTOMER_CODE,
	LOCATION

GO

