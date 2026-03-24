


-- =============================================
-- Author:		Bong Lee
-- Create date: Jan 28, 2008
-- Description:	Select Roaster
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_Roaster_Sel] 
	@vchAction varchar(30) = NULL,
	@chrFacility char(3),
	@vchRoaster varchar(10) = NULL 
AS
BEGIN
	SET NOCOUNT ON;
	IF	@vchAction = 'BlankForAll'
		SELECT '' as RoasterNo, 'All' as Description 
			Union
		SELECT RoasterNo, RoasterNo as Description FROM tblRoaster Where Facility = @chrFacility 
		ORDER BY RoasterNo
	ELSE
		IF	@vchAction = 'NullForAll'
			SELECT NULL as RoasterNo, 'All' as Description 
				Union
			SELECT RoasterNo, RoasterNo as Description FROM tblRoaster Where Facility = @chrFacility ORDER BY RoasterNo
		ELSE
			IF	@vchAction = 'ByRoaster'
				SELECT * FROM tblRoaster Where Facility = @chrFacility AND RoasterNo = @vchRoaster
			ELSE
				IF	@vchAction = 'All'
					SELECT * FROM tblRoaster Where Facility = @chrFacility ORDER BY RoasterNo
		
END

GO

