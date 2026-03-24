CREATE TABLE [dbo].[tblQATDefinition] (
    [Active]             BIT           CONSTRAINT [DF_tblQATDefinition_Active] DEFAULT ((1)) NOT NULL,
    [Facility]           VARCHAR (3)   NOT NULL,
    [Alert]              BIT           NOT NULL,
    [AllowOverride]      BIT           NOT NULL,
    [ExpiryCount]        INT           NOT NULL,
    [InProcFreqType]     TINYINT       NOT NULL,
    [InProcNoOfFreq]     INT           NOT NULL,
    [NoOfLanes]          INT           NOT NULL,
    [NoOfSamples]        INT           NOT NULL,
    [NoteID]             INT           NOT NULL,
    [QATDefnDescription] VARCHAR (100) NOT NULL,
    [QATDefnID]          INT           NOT NULL,
    [QATEntryPoint]      CHAR (1)      NOT NULL,
    [TestFormID]         INT           NOT NULL,
    [TestFormTitle]      VARCHAR (25)  NULL,
    [TestSpecID]         INT           NOT NULL,
    [UpdatedAt]          DATETIME      CONSTRAINT [DF_tblQATDefinition_UpdateAt] DEFAULT (getdate()) NULL,
    [UpdatedBy]          VARCHAR (50)  NULL,
    CONSTRAINT [PK_tblQATDefinition] PRIMARY KEY CLUSTERED ([QATDefnID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATDefinition]
    ON [dbo].[tblQATDefinition]([Facility] ASC, [QATDefnID] ASC, [Active] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 = Active; 0 = Inactive', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATDefinition', @level2type = N'COLUMN', @level2name = N'Active';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 = Allow; 0 = Not Allow', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATDefinition', @level2type = N'COLUMN', @level2name = N'AllowOverride';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'S = Test on Start-Up; I = Test on In-Process', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATDefinition', @level2type = N'COLUMN', @level2name = N'QATEntryPoint';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 = Alert QA; 0 = Do Not Alert QA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATDefinition', @level2type = N'COLUMN', @level2name = N'Alert';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 - By Checkweigher; 2 - By Pallet; 3 - Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATDefinition', @level2type = N'COLUMN', @level2name = N'InProcFreqType';


GO

