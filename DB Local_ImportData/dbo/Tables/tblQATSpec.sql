CREATE TABLE [dbo].[tblQATSpec] (
    [Active]          BIT            CONSTRAINT [DF_tblQATSpec_Active] DEFAULT ((1)) NOT NULL,
    [Facility]        VARCHAR (3)    NOT NULL,
    [Formula]         TINYINT        NOT NULL,
    [LwLmtFromTarget] DECIMAL (5, 4) NOT NULL,
    [TestSpecDesc]    VARCHAR (125)  NOT NULL,
    [TestSpecID]      INT            NOT NULL,
    [UpdatedAt]       DATETIME       CONSTRAINT [DF_tblQATSpec_UpdatedAt] DEFAULT (getdate()) NULL,
    [UpdatedBy]       VARCHAR (50)   NULL,
    [UpLmtFromTarget] DECIMAL (5, 4) CONSTRAINT [DF_tblQATSpec_ULTFromTarget] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblQATSpec_1] PRIMARY KEY CLUSTERED ([TestSpecID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATSpec]
    ON [dbo].[tblQATSpec]([Facility] ASC, [Active] ASC, [TestSpecID] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 - actual tolerance; 2 - Actual Value, 4 - Percentage Tolerance; 10+ - Customized.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATSpec', @level2type = N'COLUMN', @level2name = N'Formula';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Upper Limit From Target', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATSpec', @level2type = N'COLUMN', @level2name = N'UpLmtFromTarget';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lower Limit from Target', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATSpec', @level2type = N'COLUMN', @level2name = N'LwLmtFromTarget';


GO

