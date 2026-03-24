-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 14, 2011
-- Description:	Append text to a Text File
-- =============================================
/*
Requirement: Need to enable OLE_Automation feature from MS SQL Configuration Surface
*/

CREATE PROCEDURE [dbo].[PPsp_AppendToTextFile]
	@vchFileName varchar(255),
	@vchText varchar(255),
	@bitAddTimeStamp bit = 1,	-- Default add time stamp at the beginning of the text
	@vchWriteMethod varchar(10) = 'NewLine',
	@intIOMode tinyint = 8

AS
BEGIN

	DECLARE @FS int, @OLEResult int, @FileID int
--	insert into tblTestLog Values(@vchFileName)

	BEGIN TRY
		EXECUTE @OLEResult = sp_OACreate 'Scripting.FileSystemObject', @FS OUT
		IF @OLEResult <> 0 PRINT 'Scripting.FileSystemObject'

		/* IO Mode
		Constant		Value	Description
		ForReading		1		Open a file for reading only. You can't write to this file.
		ForWriting		2		Open a file for writing.
		ForAppending	8		Open a file and write to the end of the file.

		Create
		Optional. Boolean value that indicates whether a new file can be created if the specified filename doesn't exist. 
		The value is True if a new file is created, False if it isn't created. If omitted, a new file isn't created.	
		*/
		
		execute @OLEResult = sp_OAMethod @FS, 'OpenTextFile', @FileID OUT, @vchFileName, @intIOMode, True
		IF @OLEResult <> 0 PRINT 'OpenTextFile'
--		IF @OLEResult <> 0 insert into tblTestLog Values('OpenTextFile')
		IF @bitAddTimeStamp = 1
			SET @vchText = CONVERT(varchar(19), GETDATE(),120) + ' - ' +  @vchText

		--Write text to new line
		IF @vchWriteMethod = 'NewLine'
			Execute @OLEResult = sp_OAMethod @FileID, 'WriteLine', Null, @vchText
		ELSE	--Write text to same line
			Execute @OLEResult = sp_OAMethod @FileID, 'Write', Null, @vchText

		IF @OLEResult <> 0 PRINT 'WriteLine'
--		IF @OLEResult <> 0 insert into tblTestLog Values('WriteLine')

		EXECUTE @OLEResult = sp_OADestroy @FileID
		EXECUTE @OLEResult = sp_OADestroy @FS

	END TRY
	BEGIN CATCH
		PRINT ERROR_MESSAGE()
--		insert into tblTestLog Values(ERROR_MESSAGE())

	END CATCH
	
END

GO

