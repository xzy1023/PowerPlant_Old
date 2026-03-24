

-- =============================================
-- Author:		Bong Lee
-- Create date: Sept. 28, 2006
-- Description:	Upload Operation Staffing to Server
-- =============================================
CREATE PROCEDURE [dbo].[LPPsp_UploadOperationStaffing]
	 @ServerName VARCHAR(20),
	 @DatabaseName VARCHAR(20)
AS
BEGIN

	DECLARE @TableName VARCHAR(70)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT top 1 * FROM tblOperationStaffing)
	BEGIN
		-- SELECT @ServerName = value1, @DatabaseName = value2 FROM tblControl WHERE [Key] = 'SQLServerInfo'
		SET @TableName = @ServerName + '.' + @DatabaseName + '.dbo.tblOperationStaffing'
		
		BEGIN TRANSACTION
		EXEC ('INSERT INTO tblOperationStaffing (facility, PackagingLine, StartTime, StaffID)
				 SELECT facility, PackagingLine, StartTime, StaffID FROM ' + @TableName  )
		
		EXEC ('DELETE From ' + @TableName )
		COMMIT TRANSACTION
	END
END

GO

