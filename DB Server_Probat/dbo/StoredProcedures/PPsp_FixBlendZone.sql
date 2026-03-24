

CREATE PROCEDURE [dbo].[PPsp_FixBlendZone]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @maxFixedCustomerId VARCHAR(20);
    DECLARE @maxTableCustomerId VARCHAR(20);
    DECLARE @RowsUpdated INT;

    -- Get the last fixed customer ID from control table
    SELECT @maxFixedCustomerId = Value1 FROM tblControl WHERE [Key] = 'Probat' AND [SubKey] = 'FixBlendZone';

    -- Get the max customer ID from the blends table
    SELECT @maxTableCustomerId = MAX(CUSTOMER_ID) FROM dbo.PRO_IMP_BLENDS WHERE CUSTOMER_ID >= @maxFixedCustomerId;

    -- CTE to rank types
    WITH RankedTypes AS (
        SELECT *, 
               ROW_NUMBER() OVER (PARTITION BY CUSTOMER_CODE ORDER BY CUSTOMER_ID) AS rn
        FROM PRO_IMP_TYPES
    )
    -- Perform the update
    UPDATE b
		SET b.ZONE = t.ZONE, b.TRANSFERED = 0
    FROM PRO_IMP_BLENDS b
    LEFT JOIN RankedTypes t 
        ON b.CUSTOMER_CODE = t.CUSTOMER_CODE AND t.rn = 1
    WHERE 
        b.PART_COMP_03 = 0 
        AND b.PART_COMP_02 > 0 
        AND b.PART_COMP_01 < 50 
        AND b.ZONE = 3 
        AND t.ZONE = 2
        AND b.TRANSFERED IN (-1, 0) 
        AND b.CUSTOMER_ID > @maxFixedCustomerId 
        AND b.CUSTOMER_ID <= @maxTableCustomerId;

    -- Capture number of rows updated
    SET @RowsUpdated = @@ROWCOUNT;

    -- Update control table with new max customer ID
    UPDATE tblControl SET Value1 = @maxTableCustomerId WHERE [Key] = 'Probat' AND [SubKey] = 'FixBlendZone';

    -- Return result
    SELECT @RowsUpdated AS RowsUpdated, @maxTableCustomerId AS MaxFixedCustomerId;
END;

GO

