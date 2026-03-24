
-- =============================================================================
-- Author:		
-- WO#17423:	Aug. 15, 2018	Bong Lee
-- Description:	Select IPC Control Table
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_IPCControl_Sel] 
	@vchControlKey varchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		SELECT  ControlKey, ISNULL(Value1, '') AS Value1, ISNULL(Value2, '') AS Value2, Description
FROM            tblIPCControl
				WHERE ControlKey = ISNULL(@vchControlKey, ControlKey)
	END TRY
	BEGIN CATCH
			THROW
	END CATCH
END

GO

