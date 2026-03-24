
-- =============================================================================
-- Author:		
-- WO#17423:	June. 08, 2018	Bong Lee
-- Description:	Select QAT Test work flow
-- WO#17423:	March. 11, 2019	Bong Lee
-- Description:	Add a column, ExceptionCode
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_QATWorkFlow_Sel] 
	@vchFacility			varchar(3)		=NULL
	,@chrPackagingLine		char(10)		= NULL
	,@chrQATEntryPoint		char(1)			= NULL
	,@bitActive				bit				= 1
	,@vchFormName			varchar(50)		= NULL

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		SELECT 
			tW.Active, tW.Facility, tW.PackagingLine
			,tW.QATDefnID, tW.SerialConnID
			,tW.TestSeq, tW.TCPConnID, tW.ExceptionCode
			,tD.Alert, tD.AllowOverride, tD.NoOfLanes
			,tD.NoOfSamples, tD.NoteID, tD.QATDefnDescription
			,tD.QATEntryPoint ,tD.TestFormID, ISNULL(tD.TestFormTitle ,'') as TestFormTitle
			,tD.InProcFreqType ,td.InProcNoOfFreq, tD.TestSpecID
			,tF.FormName, tF.TableName, tF.InterfaceFormID, tf.TestCategory
			,tN.Note
			FROM tblQATWorkFlow AS tW 
			LEFT OUTER JOIN tblQATDefinition AS tD 
			ON tW.Facility = tD.Facility 
				AND tW.QATDefnID = tD.QATDefnID 
			LEFT OUTER JOIN tblQATForm as tF
			ON tD.Facility = tF.Facility
				AND tD.TestFormID = tF.TestFormID
			LEFT OUTER JOIN	[dbo].[tblQATNote] tN
			ON tW.[Facility] = tN.[Facility]
				AND tD.NoteID = tN.NoteID
			WHERE tW.Facility = ISNULL(@vchFacility, tW.Facility)
				AND tW.PackagingLine = ISNULL(@chrPackagingLine, tW.PackagingLine)
				AND (@bitActive IS NULL OR tW.Active = @bitActive) 
				AND tD.QATEntryPoint = ISNULL(@chrQATEntryPoint,tD.QATEntryPoint)
				AND tF.FormName = ISNULL(@vchFormName, tF.FormName)
			Order by tW.TestSeq
				
	END TRY
	BEGIN CATCH
		THROW
	END CATCH
END

GO

