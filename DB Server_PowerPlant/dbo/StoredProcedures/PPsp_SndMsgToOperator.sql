
-- =============================================
-- Author:		Bong Lee
-- WO#1297      Aug. 07, 2014   Bong Lee
-- Description: Send Message to operator
-- =============================================
Create Procedure [dbo].[PPsp_SndMsgToOperator] 
	@vchMsgBody nvarchar (MAX)
	,@vchSubject nvarchar(512) = NULL	
	,@vchProfileName nvarchar(128) = 'PowerPlantSupport'								 
	,@vchName nvarchar(128) = N'Alert Notification List'		
AS
BEGIN
	DECLARE @varSubject as varchar(100)
	DECLARE @varProcEnv as varchar(10)

	IF @vchSubject is NULL	
	BEGIN	
		IF object_id(N'tblControl',N'U') IS NOT NULL	 
		BEGIN	 
			Select @varProcEnv = ISNULL(Value2,'') FROM tblControl WHERE [Key] = 'CompanyName' and subKey = 'General'
			Select @vchSubject = 'Error on Server: ' + CONVERT(varchar(20), SERVERPROPERTY('MachineName')) + ',Env: ' + @varProcEnv	
		END	 
	END		 
	
	EXEC msdb..sp_notify_operator   
		@profile_name = @vchProfileName,		-- email profile
		@name = @vchName,						-- operator name
		@subject = @vchSubject,
		@body = @vchMsgBody;
	
END

GO

