
-- =============================================
-- Author:		Alan Tsui
-- Create date: Oct 11, 2018
-- Description:	WO#17432 Update columns "Confirmed By" and "QAConfirmed"
--				by provided parameters InterfaceFormID and InterfaceID
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_QATConfirmation] 
	-- Add the parameters for the stored procedure here
		@vchInterfaceFormID			varchar(50)
		,@vchInterfaceID			varchar(24)
		,@vchConfirmedBy 			varchar(10)
		,@vchQAConfirmed			bit
	WITH EXECUTE AS 'dbo'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE 
	@SQLQuery AS nvarchar(MAX)
	,@NewTableName varchar(50) 

	SET @SQLQuery = '' 
	SET @NewTableName = ''	

	SELECT @NewTableName = TableName  
	FROM tblQATForm 
	WHERE InterfaceFormID = @vchInterfaceFormID

	IF OBJECT_ID (N'' + @NewTableName + '', N'U') IS NOT NULL 
	BEGIN 
		Set @SQLQuery =
		' DECLARE @vchWarningMSG char(500) ' +
		' BEGIN TRY ' +
			' BEGIN TRAN ' +
				' Update ' + @NewTableName +
				' Set ConfirmedBy = ''' + @vchConfirmedBy + '''' +
				' , QAConfirmed = ' + Cast(@vchQAConfirmed as CHAR(1)) +
				' Where InterfaceID = ''' + @vchInterfaceID + '''' + 
			' IF @@ROWCOUNT = 0 ' +
			' BEGIN ' +
				' ROLLBACK ' +
				' SET @vchWarningMSG = ''Could not find record with interface ID = ' + @vchInterfaceID + '. No record was updated. ' + convert (varchar(30), getdate(), 120) + '''' +
				' Select @vchWarningMSG' +
				' RAISERROR(@vchWarningMSG, 16, 1)  WITH NOWAIT ' +
			' END ' +
			' ELSE ' +
			' BEGIN ' + 
				' COMMIT ' + 
			' END ' +
		' END TRY ' +
		' BEGIN CATCH ' +
			' ;THROW ' + 
		' END CATCH ' 
		--print @SQLQuery
		 exec sp_executesql @SQLQuery 
	END
END

GO

