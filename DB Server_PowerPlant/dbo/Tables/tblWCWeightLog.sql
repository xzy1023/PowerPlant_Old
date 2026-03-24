CREATE TABLE [dbo].[tblWCWeightLog] (
    [DateTest]          DATETIME      NOT NULL,
    [ShopOrder]         INT           NOT NULL,
    [ItemNo]            NVARCHAR (35) NOT NULL,
    [ProductionLine]    NVARCHAR (50) NOT NULL,
    [Blend]             NVARCHAR (50) NOT NULL,
    [Grind]             NVARCHAR (6)  NULL,
    [OperatorInitial]   NVARCHAR (50) NOT NULL,
    [Shift]             NVARCHAR (1)  NULL,
    [EquipmentType]     NVARCHAR (1)  NULL,
    [Bin_tote]          NVARCHAR (50) NULL,
    [NetWeight]         REAL          NOT NULL,
    [MaxWeight]         REAL          NULL,
    [TargetWeight]      REAL          NULL,
    [MinWeight]         REAL          NULL,
    [LowerControlLimit] REAL          NULL,
    [TareWeight]        REAL          NULL,
    [DialSetting]       REAL          NULL,
    [DialNo]            SMALLINT      NULL,
    [QCInitial]         NVARCHAR (50) NULL,
    [BlockId]           DATETIME      NULL,
    [ErrorCode]         SMALLINT      NULL,
    [Facility]          NVARCHAR (3)  NULL,
    [Inactive]          BIT           NOT NULL,
    [BatchStartTime]    DATETIME      NOT NULL,
    [UploadStatus]      BIT           NOT NULL,
    CONSTRAINT [PK_tblWCWeightLog] PRIMARY KEY CLUSTERED ([DateTest] ASC, [ShopOrder] ASC, [ProductionLine] ASC, [Inactive] ASC)
);


GO

