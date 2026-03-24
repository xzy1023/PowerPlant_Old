
-- =============================================
-- V7.01	    Jun. 25, 2025
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_FixImpOrderReceivingStation]
AS
BEGIN
    DECLARE @maxFixedCustomerId VARCHAR(20);
    DECLARE @maxTableCustomerId VARCHAR(20);
    DECLARE @RowsUpdated INT;

	-- Get the last fixed customer ID from control table
    SELECT @maxFixedCustomerId = Value1 FROM tblControl WHERE [Key] = 'Probat' AND [SubKey] = 'FixedImpOrder';
	-- Get the max customer ID from the blends table
    SELECT @maxTableCustomerId = MAX(CUSTOMER_ID) FROM dbo.PRO_IMP_ORDER WHERE CUSTOMER_ID >= @maxFixedCustomerId;

    SET NOCOUNT ON;

    UPDATE dbo.PRO_IMP_ORDER
    SET 
        RECEIVING_STATION = 
            CASE 
                WHEN ORDER_TYP = 3 THEN STUFF(RECEIVING_STATION, 4, 1, '8')
                WHEN ORDER_TYP = 5 THEN STUFF(RECEIVING_STATION, 4, 1, '7') 
            END,
        LINE_CONTROL = 
            CASE 
                WHEN ORDER_TYP = 3 THEN STUFF(LINE_CONTROL, 4, 1, '8')
                WHEN ORDER_TYP = 5 THEN STUFF(LINE_CONTROL, 4, 1, '7') 
            END,
        TRANSFERED = 0
    WHERE CUSTOMER_ID > @maxFixedCustomerId AND CUSTOMER_ID <= @maxTableCustomerId AND ORDER_TYP IN (3, 5) AND TRANSFERED IN (0, -1);

	-- Capture number of rows updated
    SET @RowsUpdated = @@ROWCOUNT;

	-- Update control table with new max customer ID
    UPDATE tblControl SET Value1 = @maxTableCustomerId WHERE [Key] = 'Probat' AND [SubKey] = 'FixedImpOrder';

    -- Return the result
    SELECT @RowsUpdated AS RowsUpdated, @maxTableCustomerId AS MaxCustomerId;
END

GO

