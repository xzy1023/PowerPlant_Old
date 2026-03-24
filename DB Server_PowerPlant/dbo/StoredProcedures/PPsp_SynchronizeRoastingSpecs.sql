
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 23, 2013
-- Description:	Synchronize Roasting Specs
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_SynchronizeRoastingSpecs]
	@vchFacility as varchar(3)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @vchEnvironment as varchar(255);
	DECLARE @vchDataBase as varchar(255);
	DECLARE @vchSourceTable as varchar(255);
	DECLARE @vchSQLStmt as nvarchar(1000);
	DECLARE @vchTableName as varchar(255);

	SELECT @vchEnvironment = Value2 FROM tblcontrol WHERE [Key]='CompanyName' and SubKey = 'General';
	SELECT @vchDataBase = 'MPHOPP01.Powerplant_' + @vchEnvironment

	SET @vchSQLStmt = 'Declare Object_Cursor CURSOR FOR SELECT TableName FROM ' + @vchDataBase + 
						'.[dbo].[tblRoastingSpecsUpdList] WHERE Facility = ''' + @vchFacility + ''' AND Updated = 1'

	PRINT @vchSQLStmt;
	EXEC sp_executesql @vchSQLStmt;

	OPEN Object_Cursor;
	FETCH NEXT FROM Object_Cursor
		INTO @vchTableName;

	WHILE @@FETCH_STATUS = 0
	BEGIN

		SELECT @vchSourceTable =  @vchDataBase + '.[dbo].' + @vchTableName;
		PRINT 'table names =' + @vchSourceTable;

		BEGIN TRY
		--Colour Tolerance
			IF @vchTableName = 'tblColourTolerance'
		BEGIN
			SET  @vchSQLStmt = '
			MERGE ' + @vchTableName + ' AS Target
				USING (Select * FROM ' + @vchSourceTable + ' WHERE Facility = ''' + @vchFacility + ''') AS Source
				 ON Target.Facility = Source.Facility
					AND Target.[RoasterNo] = Source.[RoasterNo]
					AND	Target.[Blend] = Source.[Blend]
			  WHEN MATCHED
				THEN
					UPDATE SET [Tolerance] = Source.[Tolerance]
			  WHEN NOT MATCHED BY TARGET
				THEN
					INSERT ([Facility],[RoasterNo],[Blend],[Tolerance]) 
					VALUES (Source.[Facility],Source.[RoasterNo],Source.[Blend],Source.[Tolerance])
			  WHEN NOT MATCHED BY SOURCE
				THEN
					DELETE 
			  ;';
		END;
		ELSE
			-- Final Tempreture Spec
			IF @vchTableName = 'tblFinalTempSpec'
			BEGIN
				SET  @vchSQLStmt = '
				MERGE ' + @vchTableName + ' AS Target
					USING (Select * FROM ' + @vchSourceTable + ' WHERE Facility = ''' + @vchFacility + ''') AS Source
					 ON Target.Facility = Source.Facility
						AND	Target.EffectiveDate = Source.EffectiveDate
						AND Target.RoasterNo = Source.RoasterNo
						AND	Target.Blend = Source.Blend
				  WHEN MATCHED
					THEN
						UPDATE SET [SpecFinalTemp] = Source.[SpecFinalTemp]
								,[CreationDate] = Source.[CreationDate]
								,[CreatedBy] = Source.[CreatedBy]
				  WHEN NOT MATCHED BY TARGET
					THEN
						INSERT ([Facility],[EffectiveDate],[RoasterNo],[Blend],[SpecFinalTemp],CreationDate,CreatedBy) 
						VALUES (Source.[Facility],Source.[EffectiveDate],Source.[RoasterNo],Source.[Blend],Source.[SpecFinalTemp],Source.CreationDate,Source.CreatedBy)
				  WHEN NOT MATCHED BY SOURCE
					THEN
						DELETE 
				  ;';
			END;
			ELSE
				-- Moisture Spec
				IF @vchTableName = 'tblMoistureSpec'
					BEGIN
				SET  @vchSQLStmt = '
				MERGE ' + @vchTableName + ' AS Target
					USING (Select * FROM ' + @vchSourceTable + ') AS Source
					 ON Target.[EffectiveDate] = Source.[EffectiveDate]
						AND	Target.[Blend] = Source.[Blend]
				  WHEN MATCHED
					THEN
						UPDATE SET [MaxMoisture] = Source.[MaxMoisture]
								,[TargetMoisture] = Source.[TargetMoisture]
								,[MinMoisture] = Source.[MinMoisture]
								,[CreationDate] = Source.[CreationDate]
								,[CreatedBy] = Source.[CreatedBy]
				  WHEN NOT MATCHED BY TARGET
					THEN
						INSERT (EffectiveDate,Blend,MaxMoisture,TargetMoisture,MinMoisture,CreationDate,CreatedBy) 
						VALUES (Source.EffectiveDate,Source.Blend,Source.MaxMoisture,Source.TargetMoisture,Source.MinMoisture,Source.CreationDate,Source.CreatedBy)
				  WHEN NOT MATCHED BY SOURCE
					THEN
						DELETE 
				  ;'; 
			END;
				ELSE
					-- Roast Colour Spec
					IF @vchTableName = 'tblRoastColourSpec'
					BEGIN
					SET  @vchSQLStmt = '
					MERGE ' + @vchTableName + ' AS Target
						USING (Select * FROM ' + @vchSourceTable + ') AS Source
						 ON Target.[EffectiveDate] = Source.[EffectiveDate]
							AND	Target.[Blend] = Source.[Blend]
					  WHEN MATCHED
						THEN
							UPDATE SET [SpecMin] = Source.[SpecMin]
									,[SpecMax] = Source.[SpecMax]
									,[SpecTarg] = Source.[SpecTarg]
									,[CreationDate] = Source.[CreationDate]
									,[CreatedBy] = Source.[CreatedBy]
					  WHEN NOT MATCHED BY TARGET
						THEN
							INSERT (EffectiveDate,Blend,SpecMin,SpecMax,SpecTarg,CreationDate,CreatedBy) 
							VALUES (Source.EffectiveDate,Source.Blend,Source.SpecMin,Source.SpecMax,Source.SpecTarg,Source.CreationDate,Source.CreatedBy)
					  WHEN NOT MATCHED BY SOURCE
						THEN
							DELETE 
					  ;';
				END;
					ELSE
						-- Darkening Factor
						IF @vchTableName = 'tblDarkeningFactor'
						BEGIN
						SET  @vchSQLStmt = '
						MERGE ' + @vchTableName + ' AS Target
							USING (Select * FROM ' + @vchSourceTable + ') AS Source
							 ON Target.[EffectiveDate] = Source.[EffectiveDate]
								AND	Target.[Blend] = Source.[Blend]
								AND	Target.[Grind] = Source.[Grind]
						  WHEN MATCHED
							THEN
								UPDATE SET [DarkeningFactor] = Source.[DarkeningFactor]
										,[CreationDate] = Source.[CreationDate]
										,[CreatedBy] = Source.[CreatedBy]
						  WHEN NOT MATCHED BY TARGET
							THEN
								INSERT (EffectiveDate,Blend,Grind,DarkeningFactor,CreationDate,CreatedBy) 
								VALUES (Source.EffectiveDate,Source.Blend,Source.Grind,Source.DarkeningFactor,Source.CreationDate,Source.CreatedBy)
						  WHEN NOT MATCHED BY SOURCE
							THEN
								DELETE 
						  ;';
						  END;
						  ELSE
							-- Finish Goods Colour Spec
							IF @vchTableName = 'tblFGColourSpec'
							BEGIN
							SET  @vchSQLStmt = '
							MERGE ' +  @vchTableName + ' AS Target
								USING (Select * FROM ' + @vchSourceTable + ') AS Source
									ON Target.[EffectiveDate] = Source.[EffectiveDate]
									AND	Target.[Blend] = Source.[Blend]
									AND	Target.[Grind] = Source.[Grind]
								WHEN MATCHED
								THEN
									UPDATE SET [Active] = Source.[Active]
									,[SpecMin] = Source.[SpecMin]
									,[SpecMax] = Source.[SpecMax]
									,[SpecTarg] = Source.[SpecTarg]
									,[CreationDate] = Source.[CreationDate]
									,[CreatedBy] = Source.[CreatedBy]
								WHEN NOT MATCHED BY TARGET
								THEN
									INSERT (Active,EffectiveDate,Blend,Grind,SpecMin,SpecMax,SpecTarg,CreationDate,CreatedBy) 
									VALUES (Source.Active,Source.EffectiveDate,Source.Blend,Source.Grind,Source.SpecMin,Source.SpecMax,Source.SpecTarg,Source.CreationDate,Source.CreatedBy)
								WHEN NOT MATCHED BY SOURCE
								THEN
									DELETE 
								;';
					END;
			
			  -- Synchronize the table
			  PRINT @vchSQLStmt;
			  EXEC sp_executesql @vchSQLStmt;

			  -- Reset the Udpate Flag in tblRoastingSpecsUpdList if the table is synchronized so it will not synchronized again.
			  SET @vchSQLStmt = 'UPDATE ' + @vchDataBase + '.[dbo].[tblRoastingSpecsUpdList] SET Updated = 0, LastSynchronize = getdate() ' + 
						 'WHERE Facility = ''' + @vchFacility + ''' AND Updated = 1';
			  PRINT @vchSQLStmt;
			  EXEC sp_executesql @vchSQLStmt; 

		END TRY
		BEGIN CATCH
			DECLARE @ErrorMessage NVARCHAR(4000);
			DECLARE @ErrorSeverity INT;
			DECLARE @ErrorState INT;
			DECLARE @vchSPName varchar(100);

			SELECT 
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();

			SELECT @ErrorMessage,@ErrorSeverity,@ErrorState
			SELECT @vchSPName = IsNull(OBJECT_NAME(@@PROCID),'Custom Script')
			EXEC PPsp_SndMsgForSupport @ErrorMessage,NULL, @vchSPName

		END CATCH;

		FETCH NEXT FROM Object_Cursor
			INTO @vchTableName;

	END;
	CLOSE Object_Cursor;
	DEALLOCATE Object_Cursor;
END;

GO

