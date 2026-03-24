

-- =============================================================================
-- Author:		Bong Lee
-- Create date: Aug. 16, 2017
-- Description:	WO#6059 Select jobs in label print queue
--
-- ==============================================================================
CREATE PROCEDURE [dbo].[PPsp_LabelPrintJobs_Sel] 
	@chrAction		varchar(30) = NULL,
	@chrFacility	char(3) = NULL,
	@vchJobName		varchar(50) = NULL,
	@vchDeviceType	char(1) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY
		IF @chrAction = 'JobList'
		BEGIN
			SELECT ROW_NUMBER() OVER(ORDER BY RRN) AS RowNumber, *
			FROM   tblCimControlJob WITH(NOLOCK)
			WHERE  (@chrFacility IS NULL OR Facility = @chrFacility)
				AND (@vchJobName IS NULL OR JobName = @vchJobName)
		END
		ELSE
			IF @chrAction = 'CheckOverLimit'
			BEGIN
				SELECT Facility, Jobname, DeviceType from
					(SELECT Facility, Jobname,  DeviceType, TimeSubmit
					  FROM tblCimControlJob WITH(NOLOCK)
					   WHERE  Facility = @chrFacility
						AND  JobName = @vchJobName
						AND DeviceType = @vchDeviceType
					  GROUP BY Facility, jobname, DeviceType, TimeSubmit) tCCJ
				GROUP BY Facility, jobname, DeviceType
				HAVING count(*) > 1
			END
			ELSE
			IF @chrAction = 'PositionInQueue'
				BEGIN
				;WITH cteCCJ as
					(
						SELECT ROW_NUMBER() OVER(ORDER BY RRN) as RowNumber, *
						FROM tblCimControlJob WITH(NOLOCK)
						WHERE Facility = @chrFacility
					)
				SELECT * FROM cteCCJ
				WHERE RRN in ( 
						  SELECT MIN(RRN) as MinRRN
						  FROM tblCimControlJob WITH(NOLOCK)
						   WHERE Facility = @chrFacility
								AND jobName = @vchJobName
								AND DeviceType = @vchDeviceType
						  GROUP BY jobname
						  )
				END
			
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
	END CATCH
END

GO

