CREATE TABLE [dbo].[tblDTReasonType] (
    [Facility]       CHAR (3)      NOT NULL,
    [MachineType]    VARCHAR (3)   NOT NULL,
    [MachineSubType] VARCHAR (10)  NOT NULL,
    [ReasonType]     SMALLINT      NOT NULL,
    [Description]    VARCHAR (255) NOT NULL,
    [Active]         BIT           NOT NULL,
    CONSTRAINT [PK_tblDTReasonType] PRIMARY KEY CLUSTERED ([Facility] ASC, [MachineType] ASC, [MachineSubType] ASC, [ReasonType] ASC)
);


GO




-- =====================================================================
-- Author:		Bong Lee
-- Create date: Mar 09, 2010
-- Description:	Flag the Down Load Table List to require to download the
--              tblDTReasonType when the data in the table is changed.
-- =====================================================================
CREATE TRIGGER [tgrDTReasonType]
ON [dbo].[tblDTReasonType]
AFTER INSERT, UPDATE, DELETE 
AS
   update dbo.tblDownLoadTableList set active = 1 where TableName = 'tblDTReasonType'

GO

