CREATE TABLE [dbo].[tblComputerConfig] (
    [Facility]                       CHAR (3)      NULL,
    [RecordStatus]                   BIT           NOT NULL,
    [ComputerName]                   VARCHAR (50)  NOT NULL,
    [Description]                    VARCHAR (50)  NULL,
    [AttachedPLC]                    BIT           NOT NULL,
    [PkgLineType]                    VARCHAR (10)  NOT NULL,
    [PalletStation]                  BIT           NOT NULL,
    [PrintPalletLabel]               BIT           NOT NULL,
    [ScaleBrand]                     VARCHAR (50)  NULL,
    [Model]                          VARCHAR (50)  NULL,
    [RunMode]                        CHAR (1)      NULL,
    [CommPort]                       TINYINT       NULL,
    [PackagingLine]                  CHAR (10)     NULL,
    [SamplingType]                   CHAR (1)      NULL,
    [SampleBlockTimeInterval]        TINYINT       NULL,
    [NoOfSamplesInABlock]            TINYINT       NULL,
    [SamplingTime]                   SMALLINT      NULL,
    [NoOfDials]                      TINYINT       NULL,
    [Dial1DecPlace]                  TINYINT       NULL,
    [QATWorkFlowInitiation]          TINYINT       NULL,
    [InterfaceType]                  VARCHAR (20)  NULL,
    [EnableOutputLocation]           BIT           NOT NULL,
    [AutoCountUnit]                  VARCHAR (10)  NULL,
    [Enable2SOIn1Line]               BIT           NOT NULL,
    [EnableOvrExpDate]               BIT           NOT NULL,
    [EnableDownLoad]                 BIT           NOT NULL,
    [ProbatEnabled]                  BIT           NOT NULL,
    [ProcessType]                    VARCHAR (20)  NULL,
    [SaveTareWgt]                    BIT           CONSTRAINT [DF_tblComputerConfig_SaveTareWgt] DEFAULT ((0)) NOT NULL,
    [ReadyForDownLoad]               BIT           CONSTRAINT [DF_tblComputerConfig_ReadyForDownLoad] DEFAULT ((0)) NOT NULL,
    [NoOfLabels]                     INT           NOT NULL,
    [IndusoftPgmName]                VARCHAR (100) NULL,
    [WorkShiftType]                  VARCHAR (10)  NOT NULL,
    [ShowEfficiency]                 BIT           NOT NULL,
    [CreatePallet]                   BIT           NOT NULL,
    [VirtualIPC]                     BIT           NOT NULL,
    [EnableCaseLabelMidnightRefresh] BIT           CONSTRAINT [DF_tblComputerConfig_EnableMidnightRefreshCaseLabel] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblComputerConfig] PRIMARY KEY CLUSTERED ([ComputerName] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0=QAT disabled; 1=From external system; 2=From Power Plant', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblComputerConfig', @level2type = N'COLUMN', @level2name = N'QATWorkFlowInitiation';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Use Indusoft. 0 = No; 1 = Yes.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblComputerConfig', @level2type = N'COLUMN', @level2name = N'AttachedPLC';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packaging line type. It drives the down time reason code.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblComputerConfig', @level2type = N'COLUMN', @level2name = N'PkgLineType';


GO

