
-- =============================================
-- Author:		Bong Lee
-- WO#1297      Aug. 07, 2014   Bong Lee
-- Description: Send Message to operator
-- =============================================
CREATE Procedure [dbo].[PPsp_SndMsgToOperator] 
	@vchMsgBody nvarchar (MAX)
	,@vchSubject nvarchar(512) = NULL	
	,@vchProfileName nvarchar(128) = 'PowerPlantSupport'								 
	,@vchName nvarchar(128) = N'Alert Notification List'		
AS
BEGIN
	DECLARE @varSubject as varchar(100);

	IF @vchSubject is NULL	
	BEGIN	
		IF object_id(N'tblControl',N'U') IS NOT NULL	 
		BEGIN	 
			Select @vchSubject = 'Error on Server: ' + CONVERT(varchar(20), SERVERPROPERTY('MachineName')) + ',DataBase: ' + DB_NAME()
		END	 
	END		 
	
	EXEC msdb..sp_notify_operator   
		@profile_name = @vchProfileName,		-- email profile
		@name = @vchName,						-- operator name
		@subject = @vchSubject,
		@body = @vchMsgBody;
	
END

GO

