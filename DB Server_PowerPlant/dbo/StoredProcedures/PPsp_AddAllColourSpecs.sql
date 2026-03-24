



-- =============================================
-- Author:		<Bong Lee>
-- Create date: <10/5/2007>
-- Description:	Add All Colour Specifications 
-- =============================================

CREATE PROCEDURE [dbo].[PPsp_AddAllColourSpecs] 
	-- Add the parameters for the stored procedure here
	@chrBlend char(6),
	@chrGrind char(6),
	@decSpecMin decimal(5,2),
	@decSpecTarg decimal(5,2),
	@decSpecMax decimal(5,2),
	@decDarkeningFactor decimal(4,2),
	@bitRoastingSpec bit,
	@bitDarkeningFactor bit,
	@bitFGSpec bit,
	@dteEffectiveDate datetime = NULL
AS
BEGIN
	DECLARE @dteToday DATETIME
	DECLARE @vchSQLStmt as nvarchar(500)
	DECLARE @vchSpecMin as nvarchar(10)
	DECLARE @vchSpecMax as nvarchar(10)
	DECLARE @vchSpecTarg as nvarchar(10)
	DECLARE @vchEffectiveDate as nvarchar(30)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON 

	SET @dteToday = GETDATE()
	if @dteEffectiveDate IS NULL
		Set @vchEffectiveDate = convert(nvarchar(30),@dteToday,120)
	Else
		Set @vchEffectiveDate = convert(nvarchar(30),@dteEffectiveDate,120)

	SELECT @vchSpecMin = cast(@decSpecMin as nvarchar(10)) ,
			@vchSpecMax = cast(@decSpecMax as nvarchar(10))   ,
			@vchSpecTarg = cast(@decSpecTarg as nvarchar(10)) 

	If @bitRoastingSpec = 1
	BEGIN
		Set @vchSQLStmt = 'PPsp_AddRoastColourSpecs ''' + @chrBlend + ''',' + @vchSpecMin + ',' + @vchSpecTarg + ',' + 
							@vchSpecMax + ',''' + @vchEffectiveDate + ''''
Print @vchSQLStmt
		Execute sp_executeSql @vchSQLStmt
	END

	If @bitDarkeningFactor = 1
	BEGIN
		Set @vchSQLStmt = 'PPsp_AddDarkeningFactor ''' + @chrBlend + ''',''' + @chrGrind + ''',' + cast(@decDarkeningFactor as nvarchar(10)) + ',''' + @vchEffectiveDate + ''''
Print @vchSQLStmt
		Execute sp_executeSql @vchSQLStmt
	END
	ELSE
	BEGIN
		IF  @decDarkeningFactor = 0
			select @decDarkeningFactor = isnull(DarkeningFactor,0) from dbo.vwLastestCurrentDarkeningFactor where blend = @chrBlend and Grind = @chrGrind
	END

	If @bitFGSpec = 1
	BEGIN
		SET @vchSpecMin = cast(@decSpecMin + @decDarkeningFactor as nvarchar(10))
		SET @vchSpecMax = cast(@decSpecMax + @decDarkeningFactor as nvarchar(10))
		SET @vchSpecTarg = cast(@decSpecTarg + @decDarkeningFactor as nvarchar(10))

		Set @vchSQLStmt = 'PPsp_AddFGColourSpecs ''' + @chrBlend + ''',''' + @chrGrind + ''',' + @vchSpecMin + ',' + @vchSpecTarg + ',' + 
							@vchSpecMax + ',''' + @vchEffectiveDate + ''''
Print @vchSQLStmt
		Execute sp_executeSql @vchSQLStmt
	END

END

GO

