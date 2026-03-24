
-- =============================================
-- Author:		Bong Lee
-- Create date: Dec 03, 2018
-- Description:	WO#17432 Test whether the same QA test has been tested in the same batch id.
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATIsTested] 	
		@vchFormName as varchar(50)
		,@dteBatchID as datetime
		,@bitIsTested as bit OUT
AS
BEGIN

	DECLARE @vchTableName as varchar(50);
	DECLARE @vchSQLStmt as nvarchar(500);
	DECLARE @varParmDefinition nvarchar(500); 
	DECLARE @intDateCoderType as int;
 
	BEGIN Try

		SET @bitIsTested  = 0

		SELECT @vchTableName = TableName FROM tblQATForm 
			WHERE FormName = @vchFormName

		IF @vchTableName <> 'tblQATDateCodeResultHeader'
		BEGIN
			SELECT @vchSQLStmt = 'SELECT @bitIsTestedOut = 1 FROM ' + @vchTableName + ' WHERE (BatchID = @dteBatchID)';
			SET @varParmDefinition = N'@dteBatchID datetime, @bitIsTestedOut bit OUTPUT';  
			EXECUTE sp_executesql @vchSQLStmt, @varParmDefinition, @dteBatchID = @dteBatchID, @bitIsTestedOUT=@bitIsTested OUTPUT;
		END
		ELSE
		BEGIN
			SELECT @intDateCoderType =  CASE WHEN @vchFormName = 'frmQATPackageDateCoder' THEN 1 
											 WHEN @vchFormName = 'frmQATCartonDateCoder'  THEN 2
											 WHEN @vchFormName = 'frmQATCaseDateCoder'	  THEN 3
										END;
			SELECT @vchSQLStmt = 'SELECT @bitIsTestedOut = 1 FROM ' + @vchTableName + ' WHERE (BatchID = @dteBatchID) AND DateCodeType = @intDateCoderType';
			SET @varParmDefinition = N'@dteBatchID datetime,  @intDateCoderType int, @bitIsTestedOut bit OUTPUT';  
			EXECUTE sp_executesql @vchSQLStmt, @varParmDefinition, @dteBatchID = @dteBatchID, @intDateCoderType = @intDateCoderType, @bitIsTestedOUT = @bitIsTested OUTPUT;
		END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN;
		THROW
	END CATCH
END

GO

