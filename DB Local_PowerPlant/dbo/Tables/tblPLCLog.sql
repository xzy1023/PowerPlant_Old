CREATE TABLE [dbo].[tblPLCLog] (
    [RRN]                          INT             IDENTITY (1, 1) NOT NULL,
    [Facility]                     CHAR (3)        NOT NULL,
    [PackagingLine]                CHAR (10)       NOT NULL,
    [Record_Number]                INT             NOT NULL,
    [ShopOrder]                    INT             NOT NULL,
    [ShopOrderStartTime]           DATETIME        NOT NULL,
    [ItemNumber]                   VARCHAR (35)    NOT NULL,
    [Start_Date_Time]              INT             NULL,
    [Stop_Date_Time]               INT             NULL,
    [r_LineNumber]                 REAL            NULL,
    [r_CaseCountPV]                REAL            NULL,
    [r_CaseCountSP]                REAL            NULL,
    [r_NumberStartsAuto]           REAL            NULL,
    [r_MaxRunTimeAuto]             REAL            NULL,
    [r_MaxBagsAuto]                REAL            NULL,
    [r_ProductNumberSelected]      REAL            NULL,
    [r_BagLength_top]              REAL            NULL,
    [r_BagLength_bot]              REAL            NULL,
    [r_TotalFilledBags_top]        DECIMAL (7, 1)  NULL,
    [r_TotalEmptyBags_Top]         DECIMAL (7, 1)  NULL,
    [r_TotalFilmFilled_Top]        DECIMAL (7, 1)  NULL,
    [r_TotalFilmEmpty_Top]         DECIMAL (7, 1)  NULL,
    [r_TotalFilledBags_Bot]        DECIMAL (7, 1)  NULL,
    [r_TotalEmptyBags_Bot]         DECIMAL (7, 1)  NULL,
    [r_TotalFilmFilled_Bottom]     DECIMAL (7, 1)  NULL,
    [r_TotalFilmEmpty_Bottom]      DECIMAL (7, 1)  NULL,
    [r_TotalCoffeeConsumed]        DECIMAL (10, 2) NULL,
    [r_TotalActualCoffee]          DECIMAL (10, 2) NULL,
    [r_TotalRecordTime]            DECIMAL (8, 2)  NULL,
    [r_TotalFilledTime]            DECIMAL (8, 2)  NULL,
    [r_AverageBPMRunning]          DECIMAL (8, 2)  NULL,
    [r_AverageBPMRecord]           DECIMAL (8, 2)  NULL,
    [r_BPM_Max]                    INT             NULL,
    [r_BPM_Min]                    INT             NULL,
    [r_NumberSamples]              INT             NULL,
    [r_WeightCalibrations]         INT             NULL,
    [r_MaxBagsCalibration]         INT             NULL,
    [r_AverageBagsCalibration]     INT             NULL,
    [r_AverageTempFESTop]          DECIMAL (6, 1)  NULL,
    [r_AverageTempRESTop]          DECIMAL (6, 1)  NULL,
    [r_AverageTempBSTop]           DECIMAL (6, 1)  NULL,
    [r_AverageTempFESBot]          DECIMAL (6, 1)  NULL,
    [r_AverageTempRESBot]          DECIMAL (6, 1)  NULL,
    [r_AverageTempBSBot]           DECIMAL (6, 1)  NULL,
    [r_AverageAugerRevs]           DECIMAL (9, 3)  NULL,
    [r_AverageGramsPerRev]         DECIMAL (6, 1)  NULL,
    [r_TargetWeightGrams]          DECIMAL (6, 1)  NULL,
    [r_LabelWeightGrams]           DECIMAL (6, 1)  NULL,
    [r_LCLWeightGrams]             DECIMAL (6, 1)  NULL,
    [r_UCLWeightGrams]             DECIMAL (6, 1)  NULL,
    [r_NumberBagsAboveLabelWeight] REAL            NULL,
    [r_WeightAboveLabelWeight]     DECIMAL (7, 1)  NULL,
    [r_NumberBagsBelowLabelWeight] REAL            NULL,
    [r_WeightBelowLabelWeight]     DECIMAL (7, 1)  NULL,
    [r_AverageWeightSamples]       DECIMAL (7, 1)  NULL,
    [r_StandardDeviationSamples]   DECIMAL (7, 3)  NULL,
    [r_StandardDeviationPerCent]   DECIMAL (7, 3)  NULL,
    [r_MinBagWeight]               DECIMAL (7, 1)  NULL,
    [r_MaxBagWeight]               DECIMAL (7, 1)  NULL,
    [r_AmountReworkGrams]          DECIMAL (10, 2) NULL,
    [r_OperatorID_1]               VARCHAR (10)    NULL,
    [r_ItemDescription]            VARCHAR (50)    NULL,
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

