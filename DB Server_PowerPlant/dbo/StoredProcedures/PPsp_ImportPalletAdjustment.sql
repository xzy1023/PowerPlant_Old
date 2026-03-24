
-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 02,2009
-- Description:	Import Pallet Adjustment from BPCS
-- WO#388:		Dec. 16, 2010	Bong Lee	
-- Description:	Import PR transactions with reason code 01 too from ITH
-- WO#359:		Jul. 12, 2011	Bong Lee	
-- Description:	Replace the zero transaction time in the Transaction Date with the
--				actual time from ITH. 
--				That makes PalletID and Transaction Date as the record unique key.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ImportPalletAdjustment]
	@dteFromCalenderDate as DateTime = NULL,
	@dteToCalenderDate as DateTime = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @vchProcEnv varchar(10)
	DECLARE @vchServerName as varchar(50);
	DECLARE @vchServerSQLStmt  varchar(1500);
	DECLARE @vchSQLStmt  varchar(1700);
	DECLARE @vchUserLib varchar(10);
	DECLARE @vchOriginalLib varchar(10);
	DECLARE @vchFromRow varchar(10);
	DECLARE @vchToRow varchar(10);
	DECLARE @vchFromDate as varchar(8), @vchToDate as varchar(8)
	DECLARE @vchFacilities as varchar(50)
	DECLARE @vchFacility as varchar(3)

	BEGIN TRY
--	Select @dteFromCalenderDate = '2009-08-05 00:00:00' , @dteToCalenderDate = '2009-08-06 00:00:00', @vchFacility = '01'
		select @vchFacility = value1 From tblControl WHERE tblControl.[Key] ='Facility' AND tblControl.SubKey = 'General'

		select @vchProcEnv = UPPER(RTrim(Value2)) from tblcontrol Where Facility = @vchFacility and [Key] = 'CompanyName' and Subkey = 'General'

		If @vchProcEnv = 'PRD'
			BEGIN
				Select @vchServerName = value1 From tblControl Where [key] = 'iSeriesNames'
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibPRD'
			END
			ELSE
			BEGIN
				If @vchProcEnv = 'UA'
				BEGIN
					Select @vchServerName = value2 From tblControl Where [key] = 'iSeriesNames'
					Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibUA'
				END
				ELSE
				BEGIN
					Select @vchServerName = value2 From tblControl Where [key] = 'iSeriesNames'
					Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibDev'
				END
			END 

		IF @dteFromCalenderDate is not NULL AND @dteToCalenderDate is not NULL
		BEGIN
			SELECT @vchFromDate = Convert(varchar(8),@dteFromCalenderDate,112),
				   @vchToDate = Convert(varchar(8),@dteToCalenderDate,112)
		END
		ELSE
		BEGIN
			SELECT @vchFromDate = (SELECT Convert(varchar(8),DATEADD(day,1,Cast(VALUE1 as datetime)),112) FROM tblControl
				WHERE [Key] = 'LastTransactionDate' and SubKey = 'ImportPalletAdjustment'),
				@vchToDate = '99991231'
		END 

		Set @vchFacilities = ''

		DECLARE object_cursor CURSOR FOR
			SELECT tblFacility.Facility FROM tblFacility 
				INNER JOIN tblControl 
				ON tblFacility.Region = tblControl.Value1
				WHERE tblControl.[Key] ='Facility' AND tblControl.SubKey = 'General'
		OPEN object_cursor

		-- read the frist table name from the Table for download
		FETCH NEXT FROM object_cursor INTO @vchFacility

		WHILE @@FETCH_STATUS = 0
		BEGIN

			Set @vchFacilities = @vchFacilities + '''''' + @vchFacility + ''''','
			FETCH NEXT FROM object_cursor INTO @vchFacility
		END

		CLOSE object_cursor
		DEALLOCATE object_cursor

		Set @vchFacilities = substring(@vchFacilities,1, Len(@vchFacilities)-1)

		-- Load adjustment qty from BPCS
		Set @vchSQLStmt = 
		'Select * from openquery(' + @vchServerName + 
		',''Select T2.IPFAC as Facility, T2.IPREF as ShopOrder, T2.IPMACH as MachineID, Char(T2.IPEMP) as Operator, T2.IPPAL as PalletID, ' + 
		'T1.TLOT as LotNumber, T1.TQTY as AdjustedQty, T1.TRES as TransactionReasonCode, ' +
-- WO#359  'SUBSTR(T1.TTDTE,1,4) || ''''/'''' || SUBSTR(T1.TTDTE,5,2) || ''''/'''' || SUBSTR(T1.TTDTE,7,2) || '''' 00:00:00 '''' as TransactionDate ' + 
		'SUBSTR(T1.TTDTE,1,4) || ''''/'''' || SUBSTR(T1.TTDTE,5,2) || ''''/'''' || SUBSTR(T1.TTDTE,7,2) || '''' ''''  || ' +						-- WO#359
		'LEFT(DIGITS(T1.THTIME),2) || '''':'''' || SUBSTR(DIGITS(T1.THTIME),3,2) || '''':'''' || RIGHT(DIGITS(T1.THTIME),2) as TransactionDate ' +	-- WO#359
		'From ' + @vchOriginalLib + '.ITH T1 ' +
		'Left outer join ' + @vchUserLib + '.iipi$ T2 On T1.tlot = T2.iplot ' +
  		'WHERE (T1.TTDTE Between ' + @vchFromDate + ' and ' + @vchToDate + ') AND T1.TTYPE = ''''PR'''' AND T1.THWS <> ''''PBPCSECOFR'''' ' +
		'And T1.tlot <> '''''''' And (TRES = ''''01'''' OR TRES = ''''26'''' OR TRES = ''''27'''' ) and T1.TQTY < 0 And T2.IPFAC in (' + @vchFacilities + ') ' +	-- WO#388
-- WO#388	'And T1.tlot <> '''''''' And TRES = ''''26'''' and T1.TQTY < 0 And T2.IPFAC in (' + @vchFacilities + ') ' +
		'And T2.ipid = ''''IP'''' and T2.ipsts = ''''90'''' and T2.ipsts = ''''90'''' '')'

--		',''Select T3.IPREF as ShopOrder, T3.IPMACH as MachineID, Char(T3.IPEMP) as Operator, T1.TQTY as AdjustedQty, IPPCDT as TransactionDate' +
--		'From ' + @vchOriginalLib + '.ITH T1 ' +
--		'Inner Join (Select ipref From ' + @vchUserLib + '.iipi$L05 WHERE ipfac in (''' + @vchFacilities + ''') and ' +
--		  '(IPPCDT Between ' + @vchFromDate + ' and ' + @vchToDate + ') ' +
--		  'and ipid = ''''IP'''' and ipsts = ''''90'''' and ipsts = ''''90'''' Group By ipref) T2 ' +
--		'on T1.TREF = T2.ipref ' +
--		'Left outer join ' + @vchUserLib + '.iipi$L05 T3 on T1.tlot = T3.iplot ' +
--		'WHERE T1.TTYPE = ''''PR'''' and T1.TLOCT = ''''REC1'''' and  T3.ipid = ''''IP'''' and T3.ipsts = ''''90'''' and T3.ipsts = ''''90'''' )'

		print @vchSQLStmt
		BEGIN TRANSACTION T_PalletAdjustment
			DELETE From tblPalletAdjustment Where TransactionDate Between @vchFromDate and @vchToDate

			INSERT INTO tblPalletAdjustment
			Exec (@vchSQLStmt)

			UPDATE tblControl set Value1 = (Select Max(TransactionDate) from tblPalletAdjustment) 
				WHERE [Key] = 'LastTransactionDate' and SubKey = 'ImportPalletAdjustment'

		COMMIT TRANSACTION T_PalletAdjustment

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

