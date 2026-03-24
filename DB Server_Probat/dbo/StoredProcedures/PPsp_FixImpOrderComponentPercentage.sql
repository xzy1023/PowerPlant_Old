
Create PROCEDURE [dbo].[PPsp_FixImpOrderComponentPercentage]
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @maxFixedCustomerId VARCHAR(20);
    DECLARE @maxTableCustomerId VARCHAR(20);
    DECLARE @RowsUpdated INT;
	-- Get the last fixed customer ID from control table
    SELECT @maxFixedCustomerId = Value1 FROM tblControl WHERE [Key] = 'Probat' AND [SubKey] = 'FixImpOrderComponentPercentage';
	-- Get the max customer ID from the blends table
    SELECT @maxTableCustomerId = MAX(CUSTOMER_ID) FROM dbo.PRO_IMP_ORDER WHERE CUSTOMER_ID >= @maxFixedCustomerId;

	DECLARE @total INT;
	DECLARE @id INT;

	-- Cursor to loop through all CUSTOMER_IDs where more than tontrol table @MaxCustomerId 
	DECLARE cur CURSOR FOR
	SELECT CUSTOMER_ID FROM dbo.PRO_IMP_ORDER WHERE CUSTOMER_ID > @maxFixedCustomerId and CUSTOMER_ID <= @maxTableCustomerId;

	OPEN cur; FETCH NEXT FROM cur INTO @id;

	WHILE @@FETCH_STATUS = 0 BEGIN
		-- Calculate the total percentage of all PART_COMP_XX columns for the current CUSTOMER_ID
		SELECT @total = 
			ISNULL(PART_COMP_01, 0) + ISNULL(PART_COMP_02, 0) + ISNULL(PART_COMP_03, 0) + ISNULL(PART_COMP_04, 0) +
			ISNULL(PART_COMP_05, 0) + ISNULL(PART_COMP_06, 0) + ISNULL(PART_COMP_07, 0) + ISNULL(PART_COMP_08, 0) +
			ISNULL(PART_COMP_09, 0) + ISNULL(PART_COMP_10, 0) + ISNULL(PART_COMP_11, 0) + ISNULL(PART_COMP_12, 0) +
			ISNULL(PART_COMP_13, 0) + ISNULL(PART_COMP_14, 0)
		FROM dbo.PRO_IMP_ORDER WHERE CUSTOMER_ID = @id;

		-- If the total is not 1000, proceed to adjust the last non-zero component
		IF @total <> 1000
		BEGIN
			-- Identify the last non-zero PART_COMP_XX column for the current CUSTOMER_ID
			WITH LastNonZero AS (
				SELECT CUSTOMER_ID, LastComp = v.CompName
				FROM dbo.PRO_IMP_ORDER b
				CROSS APPLY (
					SELECT TOP 1 CompName
					FROM (VALUES
						('PART_COMP_14', PART_COMP_14), ('PART_COMP_13', PART_COMP_13), ('PART_COMP_12', PART_COMP_12),
						('PART_COMP_11', PART_COMP_11), ('PART_COMP_10', PART_COMP_10), ('PART_COMP_09', PART_COMP_09),
						('PART_COMP_08', PART_COMP_08), ('PART_COMP_07', PART_COMP_07), ('PART_COMP_06', PART_COMP_06),
						('PART_COMP_05', PART_COMP_05), ('PART_COMP_04', PART_COMP_04), ('PART_COMP_03', PART_COMP_03),
						('PART_COMP_02', PART_COMP_02), ('PART_COMP_01', PART_COMP_01)
					) AS v(CompName, CompValue)
					WHERE CompValue > 0
					ORDER BY CompName DESC
				) v
				WHERE b.CUSTOMER_ID = @id
			)
			-- Update the identified last non-zero component by incrementing it by 1
			-- Also reset TRANSFERED to 0 to mark the row as needing reprocessing
			UPDATE b
			SET 
				PART_COMP_01 = CASE WHEN l.LastComp = 'PART_COMP_01' THEN PART_COMP_01 + 1 ELSE PART_COMP_01 END,
				PART_COMP_02 = CASE WHEN l.LastComp = 'PART_COMP_02' THEN PART_COMP_02 + 1 ELSE PART_COMP_02 END,
				PART_COMP_03 = CASE WHEN l.LastComp = 'PART_COMP_03' THEN PART_COMP_03 + 1 ELSE PART_COMP_03 END,
				PART_COMP_04 = CASE WHEN l.LastComp = 'PART_COMP_04' THEN PART_COMP_04 + 1 ELSE PART_COMP_04 END,
				PART_COMP_05 = CASE WHEN l.LastComp = 'PART_COMP_05' THEN PART_COMP_05 + 1 ELSE PART_COMP_05 END,
				PART_COMP_06 = CASE WHEN l.LastComp = 'PART_COMP_06' THEN PART_COMP_06 + 1 ELSE PART_COMP_06 END,
				PART_COMP_07 = CASE WHEN l.LastComp = 'PART_COMP_07' THEN PART_COMP_07 + 1 ELSE PART_COMP_07 END,
				PART_COMP_08 = CASE WHEN l.LastComp = 'PART_COMP_08' THEN PART_COMP_08 + 1 ELSE PART_COMP_08 END,
				PART_COMP_09 = CASE WHEN l.LastComp = 'PART_COMP_09' THEN PART_COMP_09 + 1 ELSE PART_COMP_09 END,
				PART_COMP_10 = CASE WHEN l.LastComp = 'PART_COMP_10' THEN PART_COMP_10 + 1 ELSE PART_COMP_10 END,
				PART_COMP_11 = CASE WHEN l.LastComp = 'PART_COMP_11' THEN PART_COMP_11 + 1 ELSE PART_COMP_11 END,
				PART_COMP_12 = CASE WHEN l.LastComp = 'PART_COMP_12' THEN PART_COMP_12 + 1 ELSE PART_COMP_12 END,
				PART_COMP_13 = CASE WHEN l.LastComp = 'PART_COMP_13' THEN PART_COMP_13 + 1 ELSE PART_COMP_13 END,
				PART_COMP_14 = CASE WHEN l.LastComp = 'PART_COMP_14' THEN PART_COMP_14 + 1 ELSE PART_COMP_14 END,
				TRANSFERED = 0 -- Reset transfer status to indicate the row needs reprocessing
			FROM dbo.PRO_IMP_ORDER b
			JOIN LastNonZero l ON b.CUSTOMER_ID = l.CUSTOMER_ID;

			SET @RowsUpdated = @@ROWCOUNT;
		END

		-- Move to the next CUSTOMER_ID
		FETCH NEXT FROM cur INTO @id;
	END

	-- Clean up cursor resources
	CLOSE cur; DEALLOCATE cur;

	-- Update @MaxCustomerId to control table
	UPDATE tblControl SET Value1 = @maxTableCustomerId WHERE [Key] = 'Probat' AND [SubKey] = 'FixImpOrderComponentPercentage';

	-- Return the result
    SELECT 
        @RowsUpdated AS RowsUpdated,
        @maxTableCustomerId AS MaxCustomerId;

END

GO

