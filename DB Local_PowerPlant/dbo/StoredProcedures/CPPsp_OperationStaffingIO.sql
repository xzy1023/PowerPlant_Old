

-- =============================================
-- Author:		Bong Lee
-- Create date: Sept. 21, 2006
-- Description:	Operation Staffing I/O Module
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_OperationStaffingIO]
	-- Add the parameters for the stored procedure here
	@chrPackagingLine char(10) = NULL,
	@dtmStartTime datetime, 
	@vchAction varchar(30),
	@chrFacility char(3) = NULL,
	@vchStaffID varchar(10) = NULL
AS
BEGIN
	DECLARE @dteStartTimeCYMD datetime;
	DECLARE @chrStartTime char(20);
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	If @vchAction = 'SelectAll' 
	BEGIN
		-- Convert to date time to character to drop the millisecond portion
		--SET @chrStartTime =  CONVERT(varchar(20), @dtmStartTime,120);

		-- Convert back to date time data type for seach
		--SET @dteStartTimeCYMD = CONVERT(DATETIME,@chrStartTime);

		-- Insert statements for procedure here
		SELECT	T1.StaffID,RTRIM(T2.FirstName) + ' ' + RTRIM(T2.LastName) as StaffName,
				T2.WorkGroup, T2.StaffClass
		FROM	tblOperationStaffing T1
			LEFT OUTER JOIN dbo.tblPlantStaff T2
			ON T1.StaffID = T2.StaffID
		WHERE     (PackagingLine = @chrPackagingLine)
			AND StartTime = @dtmStartTime
			--AND (CONVERT(varchar(20),StartTime,120) = @chrStartTime)
	END
	ELSE
		If @vchAction = 'Insert'
		BEGIN
			IF NOT EXISTS (SELECT 1 from dbo.tblOperationStaffing
						WHERE (facility = @chrFacility) AND (PackagingLine = @chrPackagingLine) AND
							  (StartTime = @dtmStartTime) AND (StaffID = @vchStaffID))	
			BEGIN
				INSERT INTO dbo.tblOperationStaffing
					(Facility, PackagingLine ,StartTime, StaffID) 
					VALUES(@chrFacility, @chrPackagingLine, @dtmStartTime, @vchStaffID) 
			END	
		END
END

GO

