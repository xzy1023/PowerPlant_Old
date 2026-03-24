CREATE TABLE [dbo].[tblWeightLog] (
    [RRN]                INT             IDENTITY (1, 1) NOT NULL,
    [Facility]           CHAR (3)        NOT NULL,
    [RecordNumber]       INT             NOT NULL,
    [ShopOrder]          INT             NOT NULL,
    [ShopOrderStartTIme] DATETIME        NOT NULL,
    [ItemNumber]         VARCHAR (35)    NOT NULL,
    [TimeTest]           DATETIME        NOT NULL,
    [PackagingLine]      CHAR (10)       NOT NULL,
    [ShiftNo]            TINYINT         NOT NULL,
    [WeightSource]       SMALLINT        NOT NULL,
    [LabelWeight]        DECIMAL (6, 1)  NOT NULL,
    [TargetWeight]       DECIMAL (6, 1)  NOT NULL,
    [MinWeight]          DECIMAL (6, 1)  NOT NULL,
    [MaxWeight]          DECIMAL (6, 1)  NOT NULL,
    [ActualWeight]       DECIMAL (6, 1)  NOT NULL,
    [AugerRevs]          DECIMAL (9, 3)  NOT NULL,
    [GramsPerRevolution] DECIMAL (6, 1)  NOT NULL,
    [BagsBetweenSamples] SMALLINT        NOT NULL,
    [BagsProduced]       INT             NOT NULL,
    [prod_record_Link]   INT             NOT NULL,
    [Blend]              VARCHAR (11)    NOT NULL,
    [Grind]              VARCHAR (6)     NOT NULL,
    [BinOrTote]          VARCHAR (1)     NOT NULL,
    [BinToteNo]          VARCHAR (10)    NOT NULL,
    [LowerControlLimit]  DECIMAL (6, 1)  NOT NULL,
    [TareWeight]         DECIMAL (8, 4)  NOT NULL,
    [DialSetting]        DECIMAL (10, 4) NOT NULL,
    [DialNo]             SMALLINT        NOT NULL,
    [QCInitial]          VARCHAR (10)    NULL,
    [BlockId]            DATETIME        NOT NULL,
    [ErrorCode]          SMALLINT        NOT NULL,
    [Inactive]           BIT             NOT NULL,
    [BatchStartTime]     DATETIME        NOT NULL,
    CONSTRAINT [PK_tblWeightLog] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblWeightLog]
    ON [dbo].[tblWeightLog]([TimeTest] ASC, [PackagingLine] ASC, [ShopOrder] ASC, [Inactive] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 = Auto; 1 = Manual', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblWeightLog', @level2type = N'COLUMN', @level2name = N'WeightSource';


GO

