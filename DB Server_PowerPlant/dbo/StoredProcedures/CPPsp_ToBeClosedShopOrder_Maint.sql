


-- =============================================
-- Author:		Bong Lee
-- Create date: Feb. 11, 2011
-- Description:	Closed Shop Order History Maintenance
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_ToBeClosedShopOrder_Maint] 
	@vchAction varchar(50),
	@Facility char(3) , 
	@ShopOrder int,
	@DefaultPkgLine char(10) = NULL,
	@Operator varchar(10) = NULL,
	@SessionStartTime datetime = NULL,
	@ClosingTime datetime = NULL,
	@UpdatedToBPCS bit = NULL,
	@BPCSClosingTime datetime = NULL,
	@LastUpdated datetime = NULL,
	@CreationTime datetime = NULL,
	@RRN int = NULL
AS
BEGIN

	SET NOCOUNT ON;

    IF @vchAction = 'ADD'
	BEGIN
		INSERT INTO tblToBeClosedShopOrder 
		(Facility, ShopOrder, DefaultPkgLine, Operator, SessionStartTime, ClosingTime, LastUpdated, CreationTime)
		VALUES(@Facility, @ShopOrder, @DefaultPkgLine, @Operator, @SessionStartTime, @ClosingTime, ISNULL(@LastUpdated,GetDate()), ISNULL(@CreationTime,GetDate()))
	END
	ELSE 
		IF @vchAction = 'UPD'
		BEGIN
			UPDATE tblToBeClosedShopOrder
			SET UpdatedToBPCS = '1', BPCSClosingTime = @BPCSClosingTime, LastUpdated = ISNULL(@LastUpdated,GetDate())
			WHERE Facility = @Facility and ShopOrder = @ShopOrder and ClosingTime = @ClosingTime
		END
		ELSE 
		IF @vchAction = 'DEL'
			BEGIN
				DELETE tblToBeClosedShopOrder WHERE RRN = @RRN
			END
END

GO

