CREATE TABLE [dbo].[tblQATSpec] (
    [Active]          BIT            CONSTRAINT [DF_tblQATSpec_Active] DEFAULT ((1)) NOT NULL,
    [Facility]        VARCHAR (3)    NOT NULL,
    [Formula]         TINYINT        NOT NULL,
    [LwLmtFromTarget] DECIMAL (5, 4) NOT NULL,
    [TestSpecDesc]    VARCHAR (125)  NOT NULL,
    [TestSpecID]      INT            IDENTITY (1, 1) NOT NULL,
    [UpdatedAt]       DATETIME       CONSTRAINT [DF_tblQATSpec_UpdatedAt] DEFAULT (getdate()) NULL,
    [UpdatedBy]       VARCHAR (50)   NULL,
    [UpLmtFromTarget] DECIMAL (5, 4) CONSTRAINT [DF_tblQATSpec_ULTFromTarget] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblQATSpec_1] PRIMARY KEY CLUSTERED ([TestSpecID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATSpec]
    ON [dbo].[tblQATSpec]([Facility] ASC, [Active] ASC, [TestSpecID] ASC);


GO


-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 15, 2018
-- Description:	Flag the Down Load Table List to require to download the
--              tblQATSpec when the data in the table is changed.
-- =====================================================================
CREATE TRIGGER [dbo].[tgrQATSpec]
ON [dbo].[tblQATSpec]
AFTER INSERT, UPDATE, DELETE 
AS
   UPDATE dbo.tblDownLoadTableList set active = 1 where TableName = 'tblQATSpec'

GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 - actual tolerance; 2 - Actual Value, 4 - Percentage Tolerance; 10+ - Customized.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATSpec', @level2type = N'COLUMN', @level2name = N'Formula';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lower Limit from Target', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATSpec', @level2type = N'COLUMN', @level2name = N'LwLmtFromTarget';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Upper Limit From Target', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATSpec', @level2type = N'COLUMN', @level2name = N'UpLmtFromTarget';


GO

