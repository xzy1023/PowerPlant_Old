

-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 27 2006
-- Description:	Grant store procedures authority
-- WO#745		Jan. 17 2012	Bong Lee
-- Description: Grant SELECT right on table value functions.
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_GrantSPAut]
	-- Add the parameters for the stored procedure here
	@vchUserName varchar(50 )
AS
BEGIN
	DECLARE @vchObjName as varchar(50); 
	DECLARE @vchSQLStmt as varchar(512);
	DECLARE @vchRight as varchar(50);				--WO#745
	DECLARE @vchType as varchar(10);				--WO#745
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	
	DECLARE object_cursor CURSOR FOR	
--WO#745		SELECT [name] FROM sysobjects where (type = 'P' or type = 'FN') and category = '0'
		SELECT [name], [type] FROM sysobjects where [type] in ('P','FN','TF' ) and category = '0'	--WO#745
			and substring([name],1,3) <> 'sp_' and [name] <> 'CPPsp_GrantSPAut' 
		ORDER BY [name]
	OPEN object_cursor

		-- read the frist table name from the Table for download
--WO#745	FETCH NEXT FROM object_cursor INTO @vchObjName
	FETCH NEXT FROM object_cursor INTO @vchObjName, @vchType		--WO#745

	WHILE @@FETCH_STATUS = 0
	BEGIN
	--WO#745 ADD Start
		IF @vchType = 'TF'
			 SET @vchRight = 'SELECT'
		ELSE
			SET @vchRight = 'EXECUTE'
		SET @vchSQLStmt = 'GRANT ' + @vchRight + ' ON OBJECT::' + @vchObjName + ' TO ' + @vchUserName
	--WO#745 ADD Stop

--WO#745		SET @vchSQLStmt = 'GRANT EXECUTE ON OBJECT::' + @vchObjName + ' TO ' + @vchUserName
		EXECUTE  (@vchSQLStmt)
		--GRANT EXECUTE ON OBJECT::@vchObjName TO @vchUserName
 
--WO#745	FETCH NEXT FROM object_cursor INTO @vchObjName
		FETCH NEXT FROM object_cursor INTO @vchObjName, @vchType		--WO#745
	END

	CLOSE object_cursor
	DEALLOCATE object_cursor

END

GO

