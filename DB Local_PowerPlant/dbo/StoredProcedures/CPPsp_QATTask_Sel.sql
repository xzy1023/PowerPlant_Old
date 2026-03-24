
-- =============================================================================
-- Author:		
-- WO#17423:	June. 08, 2018	Bong Lee
-- Description:	Select QAT Tasks
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_QATTask_Sel] 
	@vchFacility varchar(3)
	,@chrPackagingLine char(10) = NULL
	,@intQATDefnID int = NULL
	,@intTestFormID int = NULL
	,@bitActive bit = 1
	,@chrQATEntryPoint char(1) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;

	BEGIN TRY
		SELECT tW.Facility, tW.PackagingLine, tW.QATDefnID
			,tD.QATDefnDescription, tD.Alert, tD.AllowOverride
			,tT.TaskID, tTM.TaskDescription, tT.NoteID, tN.Note
			FROM tblQATWorkFlow AS tW 
			LEFT OUTER JOIN tblQATDefinition AS tD 
			ON tW.Facility = tD.Facility 
				AND tW.QATDefnID = tD.QATDefnID 
			LEFT OUTER JOIN	[dbo].[tblQATTask] tT
			ON tW.[Facility] = tT.[Facility]
				AND tW.QATDefnID = tT.QATDefnID
			LEFT OUTER JOIN	[dbo].[tblQATTaskMaster] tTM
			ON tT.TaskID = tTM.TaskID 
			LEFT OUTER JOIN	[dbo].[tblQATNote] tN
			ON tT.[Facility] = tN.[Facility]
				AND tT.NoteID = tN.NoteID
			WHERE tW.Facility = @vchFacility
				AND TW.PackagingLine = ISNULL(@chrPackagingLine, TW.PackagingLine)
				AND (@bitActive IS NULL OR tW.Active = @bitActive) 
				AND TD.QATDefnID = ISNULL(@intQATDefnID, TD.QATDefnID)
				AND (@chrQATEntryPoint IS NULL OR tD.QATEntryPoint = @chrQATEntryPoint)
				AND tT.TaskID IS NOT NULL
				AND tD.TestFormID = ISNULL(@intTestFormID, tD.TestFormID)
			ORDER BY tT.TaskSeq
				
	END TRY
	BEGIN CATCH
	SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

			-- Use RAISERROR inside the CATCH block to return error
			-- information about the original error that caused
			-- execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, -- Message text.
					   @ErrorSeverity, -- Severity.
					   @ErrorState -- State.
					   );
	END CATCH
END

GO

