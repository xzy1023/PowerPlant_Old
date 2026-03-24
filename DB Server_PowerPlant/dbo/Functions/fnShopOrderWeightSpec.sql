
-- =============================================
-- Author:		Bong Lee
-- Create date: June 16, 2009
-- Description:	Get Shop Order Weight Specifications
-- WO#1297      Sep. 16, 2015   Bong Lee
-- Description: Use different method to determine the shop order is for coffee and whole or ground.
-- V7.02		July 2 2025 Zhiyuan Xiao/Sagar Kumthekar
-- Description:	Replace ItemMajorClass with OrderType because ItemMajorClass is no longer apped from D365

-- =============================================
CREATE FUNCTION [dbo].[fnShopOrderWeightSpec] 
(
	@chrFacility as char(3),
	@intShopOrder as int
)
RETURNS 
@T TABLE 
(
	-- Add the column definitions for the TABLE variable here
	ItemNumber varchar(35),
	Blend varchar(11),
	Grind varchar(6), 
	LabelWeight decimal(10,3),
	TargetWeight decimal(6,1),
	MinWeight decimal(6,1),
	MaxWeight decimal(6,1),
	LowerControlLimit decimal(6,1)
)
AS
BEGIN
	Declare 
	@intWBT1_Pct as int,	-- Whole Bean Weight tolerance 1 in percentage
	@intWBT2_Value as int,	-- Whole Bean Weight tolerance 2 in value (gram)
	@decLabelWgt as decimal(6,1),
	@decTargetWgt as decimal(6,1),
	@decMinWgt as decimal(6,1),
	@decMaxWgt as decimal(6,1),
	@decLowerControlLimit as decimal(6,1),
	@decNonWholeBeanTargetWgt as decimal(6,1),
	@bitWholeBean as bit,
	@bitOverFill as bit,
	@bitPercentage as bit,
	@bitMPPercentage as bit,
	@decTolerance as decimal(6,2),
	@decMPTolerance as decimal(6,2),
	@vchItemNumber as varchar(35),
	@vchBlend as varchar(11),
	@vchGrind as varchar(6);

	-- WO#1297 ADD Start
	Declare @cstCoffee as varchar(10);
	Declare @cstWholeBeanOrder as tinyint;

	SET @cstCoffee = 'Coffee';
	SET @cstWholeBeanOrder = 3;
	-- WO#1297 ADD Stop

	Select @intWBT1_Pct = Cast(value1 as int) ,@intWBT2_Value = Cast(value2 as int) From tblControl Where [Key] = 'WholeBeanWeightTolerance' and SubKey = 'WeightLog'

	-- If the item does not has Grind no, it is a whole bean item.
	-- In tblItemMaster, the Label Weight is Smallest Unit Net Net Weight in BPCS.
	/* If the item is an overfill item (i.e. product loses its mass as the gas
	   is released from the product in valve bags), add the overfill % of the label weight (i.e. BPCS smallest net net weight) 
	   to target weight. So, Target weight = label weight * (1 + % from the overfill table) */

	/* Note: If the product is a whole bean product and also appears in the overfill table, the target weight will be
			  based on the factor on the overfill table, otherwise Target weight = LabelWeight * (1 + @intWBT1_Pct / 100.00) . */

	-- WO#1297	Select @vchItemNumber = tSO.ItemNumber, @vchBlend = tBOM.Blend, @vchGrind = tBOM.Grind, 
	Select @vchItemNumber = tSO.ItemNumber,	@vchBlend = '', @vchGrind ='',							-- WO#1297
			@decLabelWgt = tIM.LabelWeight,
			-- WO#1297	@bitWholeBean = Case When tBOM.Blend <> '' And tBOM.Grind = '' Then 1 Else 0 End,
			@bitWholeBean = Case When tSO.OrderType = @cstWholeBeanOrder Then 1 Else 0 End,			-- WO#1297
			@bitOverFill = Case When tOF.[overfill%] is not null Then 1 Else 0 End,
			@decTargetWgt = Case When tOF.[overfill%] is not NULL Then tIM.LabelWeight * (1 + tOF.[overfill%]/100.00)
			-- WO#1297	Else Case When tBOM.Blend <> '' And tBOM.Grind = ''		
			Else Case When tSO.OrderType = @cstWholeBeanOrder										-- WO#1297
					Then tIM.LabelWeight * (1 + @intWBT1_Pct / 100.00)
					Else tIM.LabelWeight End 
			End,
		   @decNonWholeBeanTargetWgt = tIM.LabelWeight * (1 + ISNULL(tOF.[overfill%]/100.00,0))
	-- WO#1297 From tblShopOrder tSO Inner Join tblBillOfMaterials tBOM on tSO.Shoporder = tBOM.Shoporder
	From tblShopOrder tSO				-- WO#1297
	Left Outer Join tblItemMaster tIM on tSO.itemNumber = tIM.ItemNumber
	Left Outer Join tblOverFill tOF on tSO.itemNumber = tOF.ItemNumber
	-- WO#1297 Where tSO.Facility = @chrFacility And tSO.Shoporder = @intShopOrder And tBOM.Blend <> ''
	
	--Where tSO.Facility = @chrFacility And tSO.Shoporder = @intShopOrder And tIM.ItemMajorClass = @cstCoffee / Changed for V.02 (no longer using ItemMajorClass)
	-- V7.02 Replace ItemMajorClass with OrderType because ItemMajorClass is no longer apped from D365
	Where tSO.Facility = @chrFacility And tSO.Shoporder = @intShopOrder And tSO.OrderType IN (3, 5)

	-- Calculate legal min. weight and Mother Parkers' Max and Min control weight
	-- The max weight for the whole bean product is net weight + the added on value in the control table

	SELECT TOP 1 @bitPercentage = tDWT.Percentage, @decTolerance = tDWT.Tolerance,
			@bitMPPercentage= tDWT.MPPercentage, @decMPTolerance = tDWT.MPTolerance 
			FROM tblDeclaredWeightTolerance tDWT 
			WHERE tDWT.DeclaredNetQuantity >=  @decNonWholeBeanTargetWgt 

	-- Calculate legal min. weight
	If @bitPercentage = 1 
		Select @decLowerControlLimit = @decTargetWgt * (1 - @decTolerance / 100.00)
	Else
		Select @decLowerControlLimit = @decTargetWgt - @decTolerance

	-- Calculate Mother Parkers' Max and Min control weight
	If @bitWholeBean = 1 and @bitOverFill = 0
		Select @decMaxWgt = @decTargetWgt + @intWBT2_Value, @decMinWgt = @decTargetWgt - @intWBT2_Value 
	Else
		If	@bitMPPercentage = 1 
			Select @decMaxWgt = @decTargetWgt * (1 + @decMPTolerance / 100.00), @decMinWgt = @decTargetWgt * (1 - @decMPTolerance / 100.00)
		Else
			Select @decMaxWgt = @decTargetWgt + @decMPTolerance, @decMinWgt = @decTargetWgt - @decMPTolerance;


	INSERT INTO @t
		VALUES(@vchItemNumber, @vchBlend, @vchGrind, @decLabelWgt,@decTargetWgt, @decMinWgt, @decMaxWgt, @decLowerControlLimit)

	RETURN 
END

GO

