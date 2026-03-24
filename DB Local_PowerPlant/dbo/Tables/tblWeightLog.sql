CREATE TABLE [dbo].[tblWeightLog] (
    [RRN]                INT            IDENTITY (1, 1) NOT NULL,
    [Facility]           CHAR (3)       NULL,
    [RecordNumber]       INT            NULL,
    [ShopOrder]          INT            NULL,
    [ShopOrderStartTIme] VARCHAR (50)   NULL,
    [ItemNumber]         VARCHAR (35)   NULL,
    [TimeTest]           DATETIME       NULL,
    [PackagingLine]      CHAR (10)      NULL,
    [ShiftNo]            TINYINT        NULL,
    [WeightSource]       SMALLINT       NULL,
    [LabelWeight]        REAL           NULL,
    [TargetWeight]       REAL           NULL,
    [MinWeight]          REAL           NULL,
    [MaxWeight]          REAL           NULL,
    [ActualWeight]       REAL           NULL,
    [AugerRevs]          DECIMAL (9, 3) NULL,
    [GramsPerRevolution] DECIMAL (6, 1) NULL,
    [BagsBetweenSamples] SMALLINT       NULL,
    [BagsProduced]       INT            NULL,
    [prod_record_Link]   INT            NULL,
    [Blend]              CHAR (10)      NULL,
    [Grind]              CHAR (10)      NULL,
    [BinOrTote]          VARCHAR (1)    NULL,
    [BinToteNo]          VARCHAR (10)   NULL,
    [LowerControlLimit]  REAL           NULL,
    [TareWeight]         REAL           NULL,
    [DialSetting]        REAL           NULL,
    [DialNo]             SMALLINT       NULL,
    [QCInitial]          VARCHAR (10)   NULL,
    [BlockId]            DATETIME       NULL,
    [ErrorCode]          SMALLINT       NULL,
    [Inactive]           BIT            NULL,
    [BatchStartTime]     DATETIME       NULL,
    CONSTRAINT [PK_tblWeightLog] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblWeightLog]
    ON [dbo].[tblWeightLog]([TimeTest] ASC, [PackagingLine] ASC, [ShopOrder] ASC, [Inactive] ASC);


GO

