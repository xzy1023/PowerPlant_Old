
-- =============================================
-- Author:		Bong Lee
-- Create date: Sept. 21, 2006
-- Description:	Operation Staffing I/O Module
-- =============================================
Create PROCEDURE [dbo].[@del_New_CPPsp_OperationStaffingIO]
	@chrAction varchar(30),
	@chrPackagingLine char(10) = NULL,
	@dtmStartTime datetime 
AS
BEGIN
	DECLARE @dteStartTimeCYMD datetime;
	DECLARE @chrStartTime char(20);
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	IF @chrAction = 'ByLine_StartTime'
	BEGIN
		-- Convert to date time to character to drop the millisecond portion
		SET @chrStartTime =  CONVERT(varchar(20), @dtmStartTime,120);

		-- Convert back to date time data type for seach
		SET @dteStartTimeCYMD = CONVERT(DATETIME,@chrStartTime);

		-- Insert statements for procedure here
		SELECT	T1.StaffID,RTRIM(T2.FirstName) + ' ' + RTRIM(T2.LastName) as StaffName,
				T2.WorkGroup, T2.StaffClass
		FROM	tblOperationStaffing T1
			LEFT OUTER JOIN dbo.tblPlantStaff T2
			ON T1.StaffID = T2.StaffID
		WHERE     (PackagingLine = @chrPackagingLine)
			AND (StartTime = @dteStartTimeCYMD)
	END
	ELSE
	BEGIN
		IF @chrAction = 'All'
		SELECT * FROM tblOperationStaffing
	END 
END

GO

