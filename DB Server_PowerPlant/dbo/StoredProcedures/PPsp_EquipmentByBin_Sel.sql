

-- =============================================
-- Author:		Bong Lee
-- Create date: Feb 12, 2008
-- Description:	Select Equipment based on the Bin
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_EquipmentByBin_Sel] 
	@chrFacility char(3), 
	@vchBinTote varchar(6) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT Distinct T3.Description, ST1.PackagingLine as EquipmentID
		FROM  (SELECT  T1.Facility, CASE WHEN T2.PackagingLine IS NULL THEN T1.PackagingLine ELSE T2.PackagingLine END AS PackagingLine 
		FROM   tblBinToteLine AS T1 
		LEFT OUTER JOIN  tblBinToteLine AS T2 
		ON T1.Facility = T2.facility AND SUBSTRING(T1.PackagingLine, 1, 1) = T2.EquipmentType 
		WHERE T1.Facility = @chrFacility AND T1.BinTote = @vchBinTote) AS ST1 
		LEFT OUTER JOIN tblEquipment AS T3 ON ST1.Facility = T3.facility AND ST1.PackagingLine = T3.EquipmentID 
		WHERE (T3.Type = N'P') ORDER BY T3.Description
END

GO

