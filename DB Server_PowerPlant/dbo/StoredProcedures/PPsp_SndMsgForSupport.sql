
-- =============================================
-- Author:		Bong Lee
-- Create date: Dec.19,2007
-- Description:	Send Message for Support
-- WO#359       Feb. 22, 2012   Bong Lee
-- Description: Accept parameters Profile Name, Subject, Recipient
--				to make the stored Proc. more generic.
-- =============================================
CREATE Procedure [dbo].[PPsp_SndMsgForSupport] 
	@vchMsgBody varchar (MAX)
	,@vchProfile_name varchar(128) = 'PowerPlantSupport'				-- WO#359 
	,@vchSubject varchar(512) = NULL									-- WO#359 
	,@vchRecipients varchar(128) = 'sqlSupport@Mother-Parkers.com'		-- WO#359 
AS
BEGIN
	DECLARE @varSubject as varchar(100)
	DECLARE @varProcEnv as varchar(10)

	IF @vchSubject is NULL	-- WO#359 
	BEGIN	-- WO#359 
		IF object_id(N'tblControl',N'U') IS NOT NULL	-- WO#359 
		BEGIN	-- WO#359 
			Select @varProcEnv = isnull(Value2,'') FROM tblControl WHERE [Key] = 'CompanyName' and subKey = 'General'
			Select @vchSubject = 'Error on Server: ' + CONVERT(varchar(20), SERVERPROPERTY('MachineName')) + ',Env: ' + @varProcEnv	-- WO#359 
--WO$359	Select @varSubject = 'Power Plant Error on Server: ' + CONVERT(varchar(20), SERVERPROPERTY('MachineName')) + ',Env: ' + @varProcEnv 
		END	-- WO#359 
	END		-- WO#359 
	
	EXEC msdb.dbo.sp_send_dbmail
	@profile_name = @vchProfile_name		-- WO#359 
	,@recipients = @vchRecipients			-- WO#359 
	,@body = @vchMsgBody					-- WO#359
	,@subject = @vchSubject					-- WO#359
-- WO#359 	@profile_name = 'PowerPlantSupport',
-- WO#359 	@recipients = 'sqlSupport@Mother-Parkers.com',
-- WO#359	@body = @vchMsgBody,
-- WO#359 	@subject = @varSubject
	
END

GO

