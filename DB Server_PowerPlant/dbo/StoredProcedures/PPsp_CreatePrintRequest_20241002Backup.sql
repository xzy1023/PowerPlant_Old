

-- =========================================================================+
-- Author:		Bong Lee
-- Create date: Sept. 05, 2006
-- Description:	Create Print Request
-- Mod: #001	Bong Lee
--		Support multiple devices for the same device type
-- Mod: #002	Jun. 25, 2007	Bong Lee
--		Allow to select print driver (Use Native or not)
-- WO#512       Mar. 26, 2012   Bong Lee
-- Description: Track who printed the UPI label. Add an optional input parameter, Requestor, 
--				for PPsp_CreatePrintRequest
-- WO#37864		Feb. 14, 2021	Bong Lee
-- Description: Do not print pallet labels for direct feed WIP lines (i.e the output location 
--				on the pallet record is not NULL nor 'RAF'). Even it is for reprint.
-- ==========================================================================
CREATE PROCEDURE [dbo].[PPsp_CreatePrintRequest_20241002Backup]
	-- Add the parameters for the stored procedure here
	
	@chrLabelType char(1) = NULL,
	@chrFacility char(3) = NULL,
	@chrDftPkgLine char(10) = NULL, 
	@chrDeviceType char(1) = NULL,
	@dteStartTime datetime = NULL,
	@vchJobName varchar(50) = NULL,
	@bitSbmFromPalletStation bit = 0,						-- #002
	@intCopies smallint = 1
	,@vchRequestor varchar(10) = NULL						--WO#512
AS
BEGIN
	DECLARE @vchDeviceName AS varchar(50);
	DECLARE @bitUseNativeDriver as Bit;						-- #002
	DECLARE @dteTimeSubmit as datetime						--WO#512
	DECLARE @intPalletID as int								-- WO#37864
	DECLARE @bitSkipPrinting as bit							-- WO#37864

	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	SET XACT_ABORT ON;	--WO#512

	BEGIN TRY	-- #001
	-- #001 SET @vchDeviceName = (SELECT DeviceName FROM dbo.tblPkgLinePrinterDevice 
	DECLARE object_cursor CURSOR FOR						-- #001
	-- #002	SELECT DeviceName FROM dbo.tblPkgLinePrinterDevice 
		SELECT DeviceName,UseNativeDriver FROM dbo.tblPkgLinePrinterDevice	-- #002
			WHERE facility = @chrFacility 
			AND PackagingLine = @chrDftPkgLine
			AND DeviceType = @chrDeviceType;

	OPEN object_cursor										-- #001

	-- read the frist device name from the Table 
	-- #002 FETCH NEXT FROM object_cursor INTO @vchDeviceName		-- #001
	FETCH NEXT FROM object_cursor INTO @vchDeviceName,@bitUseNativeDriver	--#002

	WHILE @@FETCH_STATUS = 0								-- #001
	BEGIN													-- #001
		SET @dteTimeSubmit = GetDate()						--WO#512
		BEGIN TRANSACTION	--WO#512
		-- #1 BEGIN TRY	
			-- Label Type can be: C=Case Label; F=Filter Coder; P=Pallet Label; X=Package Coder; 
			-- WO#37864 ADD Start
			SELECT @bitSkipPrinting = 0
			IF  @chrLabelType = 'P'
			BEGIN
				SELECT TOP 1 @intPalletID = Value from (
					SELECT Value, ROW_NUMBER() OVER (ORDER BY value) as Number FROM STRING_SPLIT(@vchJobName, ' ') 
				) t
				ORDER BY number DESC

				SELECT @bitSkipPrinting = 1 FROM (
					SELECT * FROM tblPallet
					UNION ALL 
					SELECT * FROM tblPalletHst
				) tP WHERE tp.Facility = @chrFacility
					AND tP.PalletID = @intPalletID 
					AND NOT (isNULL(tP.OutputLocation,'') = '' OR tP.OutputLocation = 'RAF')

			END

			IF @bitSkipPrinting = 0
			BEGIN
			-- WO#37864 ADD Stop

				INSERT INTO tblCimControlJob
				   (TimeSubmit, facility, LabelType, DefaultPkgLine, StartTime, JobName, DeviceName, DeviceType, NoOfCopies, UseNativeDriver,SbmFromPalletStation)										--WO#512
				VALUES (@dteTimeSubmit, @chrFacility, @chrLabelType,@chrDftPkgLine, @dteStartTime, @vchJobName, @vchDeviceName,@chrDeviceType,@intCopies,@bitUseNativeDriver,@bitSbmFromPalletStation)	--WO#512
			
			END		-- WO#37864

--WO#512	   (facility, LabelType, DefaultPkgLine, StartTime, JobName, DeviceName, DeviceType, NoOfCopies, UseNativeDriver,SbmFromPalletStation)									-- #002
--WO#512	VALUES (@chrFacility, @chrLabelType,@chrDftPkgLine, @dteStartTime, @vchJobName, @vchDeviceName,@chrDeviceType,@intCopies,@bitUseNativeDriver,@bitSbmFromPalletStation)	-- #002
		-- #002		(LabelType, DefaultPkgLine, StartTime, JobName, DeviceName, DeviceType, NoOfCopies)
		-- #002	VALUES (@chrLabelType,@chrDftPkgLine, @dteStartTime, @vchJobName, @vchDeviceName,@chrDeviceType,@intCopies)
		-- #1 END TRY
		--WO#512 ADD Start
			IF  @chrLabelType = 'P'
				INSERT INTO tblLabelPrintLog
					(TimeSubmit, facility, LabelType, DefaultPkgLine, StartTime, JobName, DeviceName, SbmFromPalletStation, Requestor, LabelID)
				VALUES (@dteTimeSubmit, @chrFacility, @chrLabelType,@chrDftPkgLine, @dteStartTime, @vchJobName, @vchDeviceName, @bitSbmFromPalletStation, @vchRequestor, 
					LTRIM(Substring(@vchJobName, LEN(@chrDftPkgLine) + 1, LEN(@vchJobName)- LEN(@chrDftPkgLine) + 1)))
		COMMIT TRANSACTION
		--WO#512 ADD Stop

		-- #002 FETCH NEXT FROM object_cursor INTO @vchDeviceName	-- #001
		FETCH NEXT FROM object_cursor INTO @vchDeviceName,@bitUseNativeDriver	--#002
	END

	CLOSE object_cursor										-- #001
	DEALLOCATE object_cursor								-- #001

	END TRY													-- #001
	BEGIN CATCH
		IF (XACT_STATE()) = -1			--WO#512
				ROLLBACK TRANSACTION	--WO#512 
		SELECT 
				@ErrorMessage = 'PPsp_CreatePrintRequest - ' + ERROR_MESSAGE(),
				@ErrorNumber = ERROR_NUMBER(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE(),
				@ErrorLine = ERROR_LINE(),
				@ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

			RAISERROR 
				(
				@ErrorMessage, 
				@ErrorSeverity, 
				@ErrorState,     -- parameter: original error state.               
				@ErrorNumber,    -- parameter: original error number.
				@ErrorSeverity,  -- parameter: original error severity.
				@ErrorProcedure, -- parameter: original error procedure name.
				@ErrorLine       -- parameter: original error line number.
				);

	END CATCH
	
END

GO

