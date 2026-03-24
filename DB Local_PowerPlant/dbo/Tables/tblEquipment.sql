CREATE TABLE [dbo].[tblEquipment] (
    [Active]                 BIT          CONSTRAINT [DF_tblEquipment_Active] DEFAULT ((1)) NOT NULL,
    [facility]               CHAR (3)     NOT NULL,
    [EquipmentID]            CHAR (10)    NOT NULL,
    [Type]                   CHAR (1)     NOT NULL,
    [SubType]                CHAR (1)     NULL,
    [Description]            VARCHAR (30) NOT NULL,
    [GroupID]                VARCHAR (10) NULL,
    [ProbatID]               VARCHAR (10) NULL,
    [IPCSharedGroup]         TINYINT      NULL,
    [WorkCenter]             VARCHAR (10) NULL,
    [EnableDownTimeDuration] BIT          NULL,
    CONSTRAINT [PK_tblEquipment] PRIMARY KEY CLUSTERED ([facility] ASC, [EquipmentID] ASC, [Type] ASC)
);


GO

