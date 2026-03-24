
-- =============================================
-- Author:		Bong Lee
-- Create date: Apr. 15, 2012
-- Description:	Select Down Time Log
-- WO#14866		Dec. 6,2018 Bong Lee
-- Description:	Add action for all lines include lines on virtual IPC
--				filter by shift and shift production date.
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_DownTimeLog_Sel]
	@vchAction varchar(50)
	,@chrFacility char(3) = NULL
	,@vchMachineType varchar(3) = NULL
	,@dteDownTimeBegin datetime = NULL
	,@vchMachineID varchar(10) = NULL
	,@txtOperator varchar(10) = NULL
	,@bitInActive bit = NULL
	,@intShift int = NULL								--WO#14866
	,@dteShiftProductionDate datetime = NULL			--WO#14866
AS
BEGIN
	
	SET NOCOUNT ON;

	BEGIN TRY
		IF @vchAction = 'CurrentSessionTopX'
		BEGIN
			SELECT  TOP 12  tDTL.*
					,DATEDIFF(mi, tDTL.DownTimeBegin, tDTL.DownTimeEnd) AS Duration 
                    ,tRC.[Description]
			FROM    tblDownTimeLog AS tDTL 
            LEFT OUTER JOIN  tblDTReasonCode AS tRC 
            ON		tDTL.MachineSubType = tRC.MachineSubType AND tDTL.Facility = tRC.Facility AND
					tDTL.MachineType = tRC.MachineType AND tDTL.ReasonCode = tRC.ReasonCode
			WHERE	(@bitInActive is NULL OR tDTL.InActive = @bitInActive) 
					AND (@chrFacility IS NULL OR tDTL.Facility = @chrFacility)  
					AND (@vchMachineType IS NULL OR tDTL.MachineType = @vchMachineType)
					AND (@dteDownTimeBegin IS NULL OR tDTL.DownTimeBegin = @dteDownTimeBegin) 
					AND (@txtOperator IS NULL OR tDTL.Operator = @txtOperator)
                    AND ((@vchMachineType = 'P') AND (LEFT(tDTL.MachineID, 7) = LEFT(@vchMachineID, 7)) 
						OR (@vchMachineType <> 'P') AND (tDTL.MachineID = @vchMachineID))
			ORDER BY RRN DESC
		END
		ELSE
		  IF @vchAction = 'FilterByStartTime'
		  BEGIN
			SELECT  tDTL.*
					,DATEDIFF(mi, tDTL.DownTimeBegin, tDTL.DownTimeEnd) AS Duration 
                    ,tRC.[Description]
			FROM    tblDownTimeLog AS tDTL 
            LEFT OUTER JOIN  tblDTReasonCode AS tRC 
            ON		tDTL.MachineSubType = tRC.MachineSubType AND tDTL.Facility = tRC.Facility AND
					tDTL.MachineType = tRC.MachineType AND tDTL.ReasonCode = tRC.ReasonCode
			WHERE	(@bitInActive is NULL OR tDTL.InActive = @bitInActive) 
					AND (@chrFacility IS NULL OR tDTL.Facility = @chrFacility)  
					AND (@vchMachineType IS NULL OR tDTL.MachineType = @vchMachineType)
					AND (@dteDownTimeBegin IS NULL OR tDTL.DownTimeBegin = @dteDownTimeBegin) 
					AND (@txtOperator IS NULL OR tDTL.Operator = @txtOperator)
                    AND	((@vchMachineType = 'P') AND (LEFT(tDTL.MachineID, 7) = LEFT(@vchMachineID, 7)) 
						OR (@vchMachineType <> 'P') AND (tDTL.MachineID = @vchMachineID))
			ORDER BY tDTL.RRN DESC
		  END
		  ELSE
			IF @vchAction = 'ALL'
			BEGIN
				SELECT  tDTL.*
						,DATEDIFF(mi, tDTL.DownTimeBegin, tDTL.DownTimeEnd) AS Duration 
						,tRC.[Description]
				FROM    tblDownTimeLog AS tDTL 
				LEFT OUTER JOIN  tblDTReasonCode AS tRC 
				ON		tDTL.MachineSubType = tRC.MachineSubType AND tDTL.Facility = tRC.Facility AND
						tDTL.MachineType = tRC.MachineType AND tDTL.ReasonCode = tRC.ReasonCode
				WHERE	(@bitInActive is NULL OR tDTL.InActive = @bitInActive) 
						AND (@chrFacility IS NULL OR tDTL.Facility = @chrFacility)  
						AND (@vchMachineType IS NULL OR tDTL.MachineType = @vchMachineType)
						AND (@dteDownTimeBegin IS NULL OR tDTL.DownTimeBegin = @dteDownTimeBegin) 
						AND (@txtOperator IS NULL OR tDTL.Operator = @txtOperator) 
						AND (@vchMachineID IS NULL OR tDTL.MachineID = @vchMachineID)
				ORDER BY tDTL.RRN
			END
			--WO#14866 ADD Start
			ELSE
				IF @vchAction = 'ByShift'
				BEGIN
					SELECT  tDTL.*
							,DATEDIFF(mi, tDTL.DownTimeBegin, tDTL.DownTimeEnd) AS Duration 
							,tRC.[Description]
					FROM    tblDownTimeLog AS tDTL 
					LEFT OUTER JOIN  tblDTReasonCode AS tRC 
					ON		tDTL.MachineSubType = tRC.MachineSubType AND tDTL.Facility = tRC.Facility AND
							tDTL.MachineType = tRC.MachineType AND tDTL.ReasonCode = tRC.ReasonCode
					WHERE	(@bitInActive is NULL OR tDTL.InActive = @bitInActive) 
							AND (@chrFacility IS NULL OR tDTL.Facility = @chrFacility)  
							AND (@vchMachineType IS NULL OR tDTL.MachineType = @vchMachineType)
							AND (@dteDownTimeBegin IS NULL OR tDTL.DownTimeBegin = @dteDownTimeBegin) 
							AND (@txtOperator IS NULL OR tDTL.Operator = @txtOperator)
							AND (@vchMachineID IS NULL OR ( ((@vchMachineType = 'P') AND (LEFT(tDTL.MachineID, 7) = LEFT(@vchMachineID, 7)) )
								OR ((@vchMachineType <> 'P') AND (tDTL.MachineID = @vchMachineID)) ) )
							AND (@dteShiftProductionDate IS NULL OR tDTL.ShiftProductionDate = @dteShiftProductionDate) 
							AND (@intShift IS NULL OR tDTL.[Shift] = @intShift) 
					ORDER BY tDTL.RRN DESC
				END
			--WO#14866 ADD Stop

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

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
	END CATCH;	

END

GO

