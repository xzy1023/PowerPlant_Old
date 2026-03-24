


-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 04, 2006
-- Description:	Export Server Tables To Local DataBase
-- Mod.			Date			Author
-- #0001		Dec.14 2012		Bong Lee	
-- Description: Skip records that the value of col1 is NULL
-- #0002		Apr.30 2013		Bong Lee	
-- Description: Skip records that the value of Blend is NULL
-- #0003		Jun.25 2013		Bong Lee	
-- Description: Skip records that the Blend and Grind are 965/16, 966/16, 967/16, 968/16 and 969/16
-- WO#745		Nov.18 2013		Bong Lee	
-- Description: Send to different Recipients by facility
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_FGColourLog_Upd]
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		Declare @vchFromDate as varchar(20)
-- WO#745	Declare @vchUpTo varchar(60)
		Declare @vchUpTo varchar(80);			-- WO#745
		DECLARE @tableHTML  NVARCHAR(MAX) ;
		DECLARE @vchFacility as varchar(10);	-- WO#745
		DECLARE @chrProcEnv as char(1);			-- WO#745

		SELECT @vchFacility = Value1, @chrProcEnv = LEFT(value2,1)
			FROM tblControl 
			WHERE [Key] = 'Facility' and subkey = 'General';

		-- Back up the table
		Truncate table tblFGColourLogBK

		INSERT into tblFGColourLogBK
		Select * from tblFGColourLog

		-- Remove the record of the maximum date in the table. It is because those may be
		-- the partial records of that date
		SELECT @vchFromDate = Convert(varchar(10), MAX(DateTest), 120) + ' 00:00:00'
		from dbo.tblFGColourLog

		Print 'Fill data From date: ' +  @vchFromDate

		Delete from tblFGColourLog where DateTest = @vchFromDate

		-- Write record to production from data in the staging area
		IF @chrProcEnv = 'P'			--WO#745
		BEGIN							--WO#745
			Insert into dbo.tblFGColourLog 
			SELECT TestNo, ProdDate, ProductionTime, dbo.fnFillLeadingZeros(3,Blend),
			dbo.fnFillLeadingZeros(2,Cast(ISNULL(Grind ,'') as nvarchar(6))), 
			CAST(Col1 as decimal(5,2)) 
			FROM importData.dbo.tblFGColourLog
			-- 0002 Where col1 is not NULL							-- #0001
			Where col1 is not NULL AND Blend is not null	-- #0002
				AND Blend + Grind not in ('96516','96616','96716','96816','96916')	-- #0003
				AND Facility = @vchFacility											-- WO#745
		--WO#745 ADD Start
		END
		ELSE
			IF @chrProcEnv = 'U'
			BEGIN
			Insert into dbo.tblFGColourLog 
				SELECT TestNo, ProdDate, ProductionTime, dbo.fnFillLeadingZeros(3,Blend),
				dbo.fnFillLeadingZeros(2,Cast(ISNULL(Grind ,'') as nvarchar(6))), 
				CAST(Col1 as decimal(5,2)) 
				FROM importData_UA.dbo.tblFGColourLog
				Where col1 is not NULL AND Blend is not null	
					AND Blend + Grind not in ('96516','96616','96716','96816','96916')	
					AND Facility = @vchFacility											-- WO#745
			END
		--WO#745 ADD Stop

		-- Update the control record to indicate what is date of the up-to-date data.
		-- It will be the maximum date in the production file minus one. This is because
		-- there are no gurantee the data in the latest date contains full date data.
		Select @vchFromDate = convert(varchar(50),  dateadd(day,-1,max(DateTest)), 107)
		From tblFGColourLog 

		Print 'Facility: ' + @vchFacility +  ' The data is up-to date: ' +  @vchFromDate

		Update dbo.tblControl set value1 = @vchFromDate where [key] = 'FGLastRefresh'

--WO#745	Select @vchUpTo = 'QC Finished Goods Colour Updated Up-to ' + @vchFromDate
		Select @vchUpTo = 'QC Finished Goods Colour for Facility ' + @vchFacility + ' is Updated Up-to ' + @vchFromDate	--WO#745

		-- WO#745 ADD Start
		DECLARE @recipients as varchar(100), @copy_recipients as varchar(100);
		SELECT @recipients = Value1, @copy_recipients = Value2 FROM tblControl 
			WHERE Facility = @vchFacility 
				AND [Key] = 'FGErrRecipients'
				AND SubKey = 'NotesFG_QA';
		-- WO#745 ADD Stop

		EXEC msdb.dbo.sp_send_dbmail
			@profile_name = 'PowerPlantSupport',
-- WO#745	@recipients = 'GMobo@Mother-Parkers.com',
-- WO#745	@copy_recipients ='blee@Mother-Parkers.com',
			@recipients = @recipients,						-- WO#745
			@copy_recipients = @copy_recipients,			-- WO#745
			@body = @tableHTML,
			@body_format = 'HTML', 
			@subject = @vchUpTo

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

