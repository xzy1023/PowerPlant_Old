CREATE TABLE [dbo].[tblDTReasonCode] (
    [Facility]        CHAR (3)      NOT NULL,
    [MachineType]     VARCHAR (3)   NOT NULL,
    [MachineSubType]  VARCHAR (10)  NOT NULL,
    [ReasonType]      SMALLINT      NOT NULL,
    [ReasonCode]      SMALLINT      NOT NULL,
    [Description]     VARCHAR (255) NOT NULL,
    [CommentRequired] BIT           NULL,
    [Active]          BIT           NOT NULL,
    [AsSOStarted]     BIT           NULL,
    CONSTRAINT [PK_tblDTReasonCode] PRIMARY KEY CLUSTERED ([MachineType] ASC, [MachineSubType] ASC, [ReasonType] ASC, [ReasonCode] ASC)
);


GO

