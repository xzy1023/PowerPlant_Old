CREATE TABLE [dbo].[tblDTReasonCode] (
    [Facility]        CHAR (3)      NOT NULL,
    [MachineType]     VARCHAR (3)   NOT NULL,
    [MachineSubType]  VARCHAR (10)  NOT NULL,
    [ReasonType]      SMALLINT      NOT NULL,
    [ReasonCode]      SMALLINT      NOT NULL,
    [Description]     VARCHAR (255) NOT NULL,
    [CommentRequired] BIT           NOT NULL,
    [Active]          BIT           NOT NULL,
    [AsSOStarted]     BIT           NULL,
    CONSTRAINT [PK_tblDTReasonCode] PRIMARY KEY CLUSTERED ([Facility] ASC, [MachineType] ASC, [MachineSubType] ASC, [ReasonType] ASC, [ReasonCode] ASC)
);


GO




-- =====================================================================
-- Author:		Bong Lee
-- Create date: Mar 09, 2010
-- Description:	Flag the Down Load Table List to require to download the
--              tblDTReasonCode when the data in the table is changed.
-- =====================================================================
CREATE TRIGGER [tgrDTReasonCode]
ON [dbo].[tblDTReasonCode]
AFTER INSERT, UPDATE, DELETE 
AS
   update dbo.tblDownLoadTableList set active = 1 where TableName = 'tblDTReasonCode'

GO

