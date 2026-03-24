
-- =============================================
-- Author:		Bong Lee
-- Create date: Feb 2, 2008
-- Description:	Check Finished Good Color Sample data
-- Mod.			Date			Author
-- #0001		Dec.14 2012		Bong Lee	
-- Description: Arithmetic overflow on the value of the columne, 'col1'
-- #0002		Apr.30 2013		Bong Lee	
-- Description: Skip records that the value of Blend is NULL
-- #0003		Jun.25 2013		Bong Lee	
-- Description: Skip records that the Blend and Grind are 965/16, 966/16, 967/16, 968/16 and 969/16
-- =============================================
-- WO#745		Nov.15 2013		Bong Lee	
-- Description: Send error message to different user by table driving 
-- =============================================
-- Check Import data
--- If there are error, send email with error information
--- If there are no error, add new data to the table and send notification email

CREATE PROCEDURE [dbo].[PPsp_CheckFGImportData]
	@vchImportData as varchar(50)
AS
BEGIN
	Declare @vchUpTo varchar(60)
	DECLARE @tableHTML  NVARCHAR(MAX) ;
	Declare @vchSubject varchar(100);	-- WO#745
	
	BEGIN TRY

		SET NOCOUNT ON;

		if object_id('tempdb..#temp_QCData') is not null
		  DROP TABLE #temp_QCData

		CREATE TABLE #temp_QCData(
			[TestNo] [nchar](20)  NOT NULL,
			[ProdDate] [nchar](20) NULL,
			[ProdTime] [nchar](20) NULL,
			[Blend] [nchar](6)  DEFAULT ((0)),
			[Grind] [nchar](6)  DEFAULT ((0)),
			Col1 nvarchar (254),
			SpecMax [decimal](5, 2),
			SpecMin [decimal](5, 2),
			Reason varchar(50)
		)

		-- Production environment
		IF @vchImportData = 'ImportData'
		BEGIN
		
			insert into #temp_QCData
			SELECT * FROM (				--#0003
			Select * From 
			(
				SELECT T1.TestNo, CONVERT(varchar(10),T1.ProdDate,111) as ProdDate,cast(cast(T1.ProductionTime/36000000000 as int) as varchar(2)) + ':' + dbo.fnFillLeadingZeros(2,cast(cast(T1.ProductionTime%36000000000/600000000 as int) as varchar(2))) as ProdTime, 
					T1.Blend, T1.Grind, T1.Col1, T2.SpecMax, T2.SpecMin, 'Off Spec.' as Reason
					FROM (SELECT * from ImportData.dbo.tblFGColourLog WHERE (isnumeric(Col1) = 1) and Substring(Col1,1,1) between '0' and '9') T1	--#0002
										Left Outer JOIN vwLatestFGColourSpec T2 ON T1.Blend = T2.Blend AND Isnull(T1.Grind,'') = T2.Grind --#0002
					--#0002	FROM (SELECT * from ImportData.dbo.tblFGColourLog WHERE (isnumeric(Col1) = 1) and Substring(Col1,1,1) between '0' and '9') T1
					--#0002 Left Outer JOIN vwLatestFGColourSpec T2 ON T1.Blend = T2.Blend AND T1.Grind = T2.Grind 
					--#0001 WHERE (isnumeric(Col1) = 1) AND (CAST(T1.Col1 AS decimal(5, 2)) > T2.SpecMax + 5) OR		
					--#0001 (CAST(T1.Col1 AS decimal(5, 2)) < T2.SpecMin - 5)											
					WHERE (isnumeric(Col1) = 1) AND (CAST(T1.Col1 AS decimal(7, 2)) > T2.SpecMax + 5) OR		--#0001
					(CAST(T1.Col1 AS decimal(7, 2)) < T2.SpecMin - 5)											--#0001
				Union
				SELECT T1.TestNo, CONVERT(varchar(10),T1.ProdDate,111) as ProdDate,cast(cast(T1.ProductionTime/36000000000 as int) as varchar(2)) + ':' + dbo.fnFillLeadingZeros(2,cast(cast(T1.ProductionTime%36000000000/600000000 as int) as varchar(2))) as ProdTime, 
					T1.Blend, T1.Grind, T1.Col1, T2.SpecMax, T2.SpecMin, 'Invalid Numeric' as Reason
					FROM ImportData.dbo.tblFGColourLog T1
					Left Outer JOIN vwLatestFGColourSpec T2 ON T1.Blend = T2.Blend AND T1.Grind = T2.Grind 
					--#0001 WHERE (isnumeric(T1.Col1) <> 1) or not (Substring(T1.Col1,1,1) between '0' and '9')
					--#0002 WHERE T1.Col1 is Not NULL And ((isnumeric(T1.Col1) <> 1) or Not (Substring(T1.Col1,1,1) Between '0' And '9') )	--#0001
					WHERE T1.Col1 is Not NULL AND T1.Blend is not null And ((isnumeric(T1.Col1) <> 1) or Not (Substring(T1.Col1,1,1) Between '0' And '9') )	--#0002
			) T4
			) T5 WHERE Blend + Grind not in ('96516','96616','96716','96816','96916')	--#0003
			SET @vchSubject = 'QC Finished GoodsColour Data Entry Exception Report'		-- WO#745
			SET @tableHTML =
				-- WO#745	N'<H2>QC Colour Data Entry Exception Report</H2>' +
				N'<H2>' + @vchSubject + '</H2>' +					-- WO#745
				N'<table border="1">' +
				N'<tr><th>Test No</th><th>Product Date</th><th>Time</th><th>Blend</th><th>Grind</th><th style=''color:red''>Input</th><th>Spec. Max</th><th>Spec. Min</th><th>Reason</th>' +
				N'</tr>' +
				ISNULL( CAST ((
					SELECT td=T1.TestNo,'', td=T1.ProdDate, '', td=ProdTime, '', 
					td=T1.Blend,'', td=isnull(T1.Grind,' '), '',td=T1.Col1, '',td=isnull(T1.SpecMax,0),'',td=isnull(T1.SpecMin,0),'',td=T1.Reason   
					From #temp_QCData T1
--				SELECT td=T1.TestNo,'', td=CONVERT(varchar(10),T1.ProdDate,111),'', td=cast(cast(T1.ProductionTime/36000000000 as int) as varchar(2)) + ':' + cast(cast(T1.ProductionTime%36000000000/600000000 as int) as varchar(2)), 
--				'',td=T1.Blend,'', td=T1.Grind, '',td=T1.Col1, '',td=T2.SpecMax,'',td=T2.SpecMin 
--				FROM (SELECT * from ImportData.dbo.tblFGColourLog WHERE (isnumeric(Col1) = 1)) T1
--				INNER JOIN vwLatestFGColourSpec T2 ON T1.Blend = T2.Blend AND T1.Grind = T2.Grind 
--				WHERE (isnumeric(Col1) = 1) AND (CAST(T1.Col1 AS decimal(5, 2)) > T2.SpecMax + 5) OR 
--				(CAST(T1.Col1 AS decimal(5, 2)) < T2.SpecMin - 5) 
--				Order by TestNo
--						FOR XML PATH('tr'), TYPE 
--				) AS NVARCHAR(MAX) ) ,'') +
--				ISNULL(CAST ((
--				SELECT td=T1.TestNo,'', td=CONVERT(varchar(10),T1.ProdDate,111),'', td=cast(cast(T1.ProductionTime/36000000000 as int) as varchar(2)) + ':' + cast(cast(T1.ProductionTime%36000000000/600000000 as int) as varchar(2)), 
--				'',td=T1.Blend,'', td=T1.Grind, '',td=T1.Col1, '',td=T2.SpecMax,'',td=T2.SpecMin 
--				FROM ImportData.dbo.tblFGColourLog T1
--				INNER JOIN 	vwLatestFGColourSpec T2 ON T1.Blend = T2.Blend AND T1.Grind = T2.Grind 
--				WHERE (isnumeric(Col1) <> 1)
				FOR XML PATH('tr'), TYPE 
					) AS NVARCHAR(MAX) ) ,'') +
				N'</table>' ;
		END
		ELSE
		-- UA environment
		BEGIN

			Insert into #temp_QCData
			SELECT * FROM (				--#0003
			Select * From 
			(
				SELECT T1.TestNo, CONVERT(varchar(10),T1.ProdDate,111) as ProdDate,cast(cast(T1.ProductionTime/36000000000 as int) as varchar(2)) + ':' + dbo.fnFillLeadingZeros(2,cast(cast(T1.ProductionTime%36000000000/600000000 as int) as varchar(2))) as ProdTime, 
					T1.Blend, T1.Grind, T1.Col1, T2.SpecMax, T2.SpecMin, 'Off Spec.' as Reason 
					FROM (SELECT * from ImportData_UA.dbo.tblFGColourLog WHERE (isnumeric(Col1) = 1) and Substring(Col1,1,1) between '0' and '9') T1
					Left Outer JOIN vwLatestFGColourSpec T2 ON T1.Blend = T2.Blend AND T1.Grind = T2.Grind 
					-- #0001 WHERE (isnumeric(Col1) = 1) AND (CAST(T1.Col1 AS decimal(5, 2)) > T2.SpecMax + 5) OR		
					-- #0001 (CAST(T1.Col1 AS decimal(5, 2)) < T2.SpecMin - 5)											
					WHERE (isnumeric(Col1) = 1) AND (CAST(T1.Col1 AS decimal(7, 2)) > T2.SpecMax + 5) OR		--#0001
					(CAST(T1.Col1 AS decimal(7, 2)) < T2.SpecMin - 5)											--#0001
				Union
				SELECT T1.TestNo, CONVERT(varchar(10),T1.ProdDate,111) as ProdDate,cast(cast(T1.ProductionTime/36000000000 as int) as varchar(2)) + ':' + dbo.fnFillLeadingZeros(2,cast(cast(T1.ProductionTime%36000000000/600000000 as int) as varchar(2))) as ProdTime, 
					T1.Blend, T1.Grind, T1.Col1, T2.SpecMax, T2.SpecMin, 'Invalid Numeric' as Reason 
					FROM ImportData_UA.dbo.tblFGColourLog T1
					Left Outer JOIN vwLatestFGColourSpec T2 ON T1.Blend = T2.Blend AND T1.Grind = T2.Grind 
					--#0001 WHERE (isnumeric(T1.Col1) <> 1) or not (Substring(T1.Col1,1,1) between '0' and '9')
					WHERE T1.Col1 is Not NULL And ((isnumeric(T1.Col1) <> 1) or Not (Substring(T1.Col1,1,1) Between '0' And '9') )	--#0001
			) T4
			) T5 WHERE Blend + Grind not in ('96516','96616','96716','96816','96916')	--#0003
			SET @vchSubject = 'QC Finished Goods Colour Data Entry Exception Report - UAT'	-- WO#745
			SET @tableHTML =
				-- WO#745	N'<H2>QC Finished Goods Colour Data Entry Exception Report</H2>' +
				N'<H2>' + @vchSubject + '</H2>' +		-- WO#745
				N'<table border="1">' +
				N'<tr><th>Test No</th><th>Product Date</th><th>Time</th><th>Blend</th><th>Grind</th><th style=''color:red''>Input</th><th>Spec. Max</th><th>Spec. Min</th><th>Reason</th>' +
				N'</tr>' +
				ISNULL( CAST ((
					SELECT td=T1.TestNo,'', td=T1.ProdDate, '', td=ProdTime, '', 
					td=T1.Blend,'', td=isnull(T1.Grind,' '), '',td=T1.Col1, '',td=isnull(T1.SpecMax,0),'',td=isnull(T1.SpecMin,0),'',td=T1.Reason   
					From #temp_QCData T1
--				SELECT td=T1.TestNo,'', td=CONVERT(varchar(10),T1.ProdDate,111),'', td=cast(cast(T1.ProductionTime/36000000000 as int) as varchar(2)) + ':' + cast(cast(T1.ProductionTime%36000000000/600000000 as int) as varchar(2)), 
--				'',td=T1.Blend,'', td=T1.Grind, '',td=T1.Col1, '',td=T2.SpecMax,'',td=T2.SpecMin 
--				FROM (SELECT * from ImportData_UA.dbo.tblFGColourLog WHERE (isnumeric(Col1) = 1)) T1
--				INNER JOIN 	vwLatestFGColourSpec T2 ON T1.Blend = T2.Blend AND T1.Grind = T2.Grind 
--				WHERE (isnumeric(Col1) = 1) AND (CAST(T1.Col1 AS decimal(5, 2)) > T2.SpecMax + 5) OR 
--				(CAST(T1.Col1 AS decimal(5, 2)) < T2.SpecMin - 5) 
--				Order by TestNo
--						FOR XML PATH('tr'), TYPE 
--				) AS NVARCHAR(MAX) ) ,'') +
--				ISNULL(CAST ((
--				SELECT td=T1.TestNo,'', td=CONVERT(varchar(10),T1.ProdDate,111),'', td=cast(cast(T1.ProductionTime/36000000000 as int) as varchar(2)) + ':' + cast(cast(T1.ProductionTime%36000000000/600000000 as int) as varchar(2)), 
--				'',td=T1.Blend,'', td=T1.Grind, '',td=T1.Col1, '',td=T2.SpecMax,'',td=T2.SpecMin 
--				FROM ImportData_UA.dbo.tblFGColourLog T1
--				INNER JOIN 	vwLatestFGColourSpec T2 ON T1.Blend = T2.Blend AND T1.Grind = T2.Grind 
--				WHERE (isnumeric(Col1) <> 1)
				FOR XML PATH('tr'), TYPE 
					) AS NVARCHAR(MAX) ) ,'') +
				N'</table>' ;
		END
--PRINT @tableHTML
		IF (Select count(*) as cnt from #temp_QCData) > 0 
		BEGIN
			-- WO#745 ADD Start
			DECLARE @recipients as varchar(100), @copy_recipients as varchar(100)
			SELECT @recipients = Value1, @copy_recipients = Value2 FROM tblControl 
				WHERE [Key] = 'FGErrRecipients'
					AND SubKey = 'NotesFG_QA';
			-- WO#745 ADD Stop
			EXEC msdb.dbo.sp_send_dbmail
				@profile_name = 'PowerPlantSupport',
-- WO#745		@recipients = 'GMobo@Mother-Parkers.com',
-- WO#745		@copy_recipients ='blee@Mother-Parkers.com',
				@recipients = @recipients,						-- WO#745
				@copy_recipients = @copy_recipients,			-- WO#745
				@body = @tableHTML,
				@body_format = 'HTML', 
				@subject = @vchSubject							-- WO#745
		-- WO#745	@subject = 'QC Finished Goods Colour Data Entry Exception Report'
		END
		ELSE
		BEGIN
			EXEC PPsp_FGColourLog_Upd

--			Select @vchUpTo = 'QC Finished Goods Colour Updated Up-to ' + value1  from dbo.tblControl  where [key] = 'FGLastRefresh'
--
--			EXEC msdb.dbo.sp_send_dbmail
--				@profile_name = 'PowerPlantSupport',
--				@recipients = 'GMobo@Mother-Parkers.com',
--				@copy_recipients ='blee@Mother-Parkers.com',
--				@body = @tableHTML,
--				@body_format = 'HTML', 
--				@subject = @vchUpTo
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
	END CATCH;
END

GO

