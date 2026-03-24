CREATE TABLE [dbo].[tblPLCLog] (
    [RRN]                          INT             IDENTITY (1, 1) NOT NULL,
    [Facility]                     CHAR (3)        NOT NULL,
    [PackagingLine]                CHAR (10)       NOT NULL,
    [Record_Number]                INT             NOT NULL,
    [ShopOrder]                    INT             NOT NULL,
    [ShopOrderStartTime]           DATETIME        NOT NULL,
    [ItemNumber]                   VARCHAR (35)    NOT NULL,
    [Start_Date_Time]              DATETIME        NOT NULL,
    [Stop_Date_Time]               DATETIME        NOT NULL,
    [r_LineNumber]                 INT             NOT NULL,
    [r_CaseCountPV]                INT             NOT NULL,
    [r_CaseCountSP]                INT             NOT NULL,
    [r_NumberStartsAuto]           INT             NOT NULL,
    [r_MaxRunTimeAuto]             INT             NOT NULL,
    [r_MaxBagsAuto]                INT             NOT NULL,
    [r_ProductNumberSelected]      INT             NOT NULL,
    [r_BagLength_top]              DECIMAL (5, 1)  NULL,
    [r_BagLength_bot]              DECIMAL (5, 1)  NULL,
    [r_TotalFilledBags_top]        INT             NOT NULL,
    [r_TotalEmptyBags_Top]         INT             NOT NULL,
    [r_TotalFilmFilled_Top]        INT             NOT NULL,
    [r_TotalFilmEmpty_Top]         INT             NOT NULL,
    [r_TotalFilledBags_Bot]        INT             NOT NULL,
    [r_TotalEmptyBags_Bot]         INT             NOT NULL,
    [r_TotalFilmFilled_Bottom]     INT             NOT NULL,
    [r_TotalFilmEmpty_Bottom]      INT             NOT NULL,
    [r_TotalCoffeeConsumed]        DECIMAL (10, 2) NOT NULL,
    [r_TotalActualCoffee]          DECIMAL (10, 1) NOT NULL,
    [r_TotalRecordTime]            INT             NOT NULL,
    [r_TotalFilledTime]            INT             NOT NULL,
    [r_AverageBPMRunning]          DECIMAL (3, 1)  NOT NULL,
    [r_AverageBPMRecord]           DECIMAL (3, 1)  NOT NULL,
    [r_BPM_Max]                    INT             NOT NULL,
    [r_BPM_Min]                    INT             NOT NULL,
    [r_NumberSamples]              INT             NOT NULL,
    [r_WeightCalibrations]         INT             NOT NULL,
    [r_MaxBagsCalibration]         INT             NOT NULL,
    [r_AverageBagsCalibration]     INT             NOT NULL,
    [r_AverageTempFESTop]          DECIMAL (4, 1)  NOT NULL,
    [r_AverageTempRESTop]          DECIMAL (4, 1)  NOT NULL,
    [r_AverageTempBSTop]           DECIMAL (4, 1)  NOT NULL,
    [r_AverageTempFESBot]          DECIMAL (4, 1)  NOT NULL,
    [r_AverageTempRESBot]          DECIMAL (4, 1)  NOT NULL,
    [r_AverageTempBSBot]           DECIMAL (4, 1)  NOT NULL,
    [r_AverageAugerRevs]           DECIMAL (4, 1)  NOT NULL,
    [r_AverageGramsPerRev]         DECIMAL (4, 1)  NOT NULL,
    [r_TargetWeightGrams]          DECIMAL (6, 1)  NOT NULL,
    [r_LabelWeightGrams]           DECIMAL (6, 1)  NOT NULL,
    [r_LCLWeightGrams]             DECIMAL (6, 1)  NOT NULL,
    [r_UCLWeightGrams]             DECIMAL (6, 1)  NOT NULL,
    [r_NumberBagsAboveLabelWeight] INT             NOT NULL,
    [r_WeightAboveLabelWeight]     DECIMAL (10, 1) NOT NULL,
    [r_NumberBagsBelowLabelWeight] INT             NOT NULL,
    [r_WeightBelowLabelWeight]     DECIMAL (10, 1) NOT NULL,
    [r_AverageWeightSamples]       DECIMAL (7, 1)  NOT NULL,
    [r_StandardDeviationSamples]   DECIMAL (7, 3)  NOT NULL,
    [r_StandardDeviationPerCent]   DECIMAL (7, 2)  NOT NULL,
    [r_MinBagWeight]               DECIMAL (6, 1)  NOT NULL,
    [r_MaxBagWeight]               DECIMAL (6, 1)  NOT NULL,
    [r_AmountReworkGrams]          INT             NOT NULL,
    [Field60]                      REAL            NULL,
    [Field67]                      REAL            NULL,
    [Field68]                      REAL            NULL,
    [Field69]                      REAL            NULL,
    [Field70]                      REAL            NULL,
    CONSTRAINT [PK_tblPLCLog] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblPLCLog]
    ON [dbo].[tblPLCLog]([PackagingLine] ASC, [ShopOrder] ASC, [Start_Date_Time] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblPLCLog_1]
    ON [dbo].[tblPLCLog]([PackagingLine] ASC, [Record_Number] ASC);


GO

