CREATE TABLE [dbo].[tblDTReasonType] (
    [Facility]       CHAR (3)      NOT NULL,
    [MachineType]    VARCHAR (3)   NOT NULL,
    [MachineSubType] VARCHAR (10)  NOT NULL,
    [ReasonType]     SMALLINT      NOT NULL,
    [Description]    VARCHAR (255) NULL,
    [Active]         BIT           NOT NULL,
    CONSTRAINT [PK_tblDTReasonType] PRIMARY KEY CLUSTERED ([Facility] ASC, [MachineType] ASC, [MachineSubType] ASC, [ReasonType] ASC)
);


GO

