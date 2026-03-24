CREATE TABLE [dbo].[tblDTReasonCode_20250613] (
    [Facility]        CHAR (3)      NOT NULL,
    [MachineType]     VARCHAR (3)   NOT NULL,
    [MachineSubType]  VARCHAR (10)  NOT NULL,
    [ReasonType]      SMALLINT      NOT NULL,
    [ReasonCode]      SMALLINT      NOT NULL,
    [Description]     VARCHAR (255) NOT NULL,
    [CommentRequired] BIT           NOT NULL,
    [Active]          BIT           NOT NULL,
    [AsSOStarted]     BIT           NULL
);


GO

