CREATE TABLE [dbo].[tblQATWorkFlow] (
    [Active]        BIT          CONSTRAINT [DF_tblQATWorkFlow_Active] DEFAULT ((1)) NOT NULL,
    [Facility]      VARCHAR (3)  NOT NULL,
    [PackagingLine] VARCHAR (10) NOT NULL,
    [QATDefnID]     INT          NOT NULL,
    [SerialConnID]  INT          NULL,
    [TestSeq]       INT          NOT NULL,
    [TCPConnID]     INT          NULL,
    [UpdatedAt]     DATETIME     CONSTRAINT [DF_tblQATWorkFlow_UpdatedAt] DEFAULT (getdate()) NULL,
    [UpdatedBy]     VARCHAR (50) NULL,
    [ExceptionCode] INT          NOT NULL,
    CONSTRAINT [PK_tblQATWorkFlow] PRIMARY KEY CLUSTERED ([PackagingLine] ASC, [QATDefnID] ASC, [Facility] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATWorkFlow]
    ON [dbo].[tblQATWorkFlow]([PackagingLine] ASC, [QATDefnID] ASC, [Facility] ASC, [TestSeq] ASC, [Active] ASC);


GO


-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 15, 2018
-- Description:	Flag the Down Load Table List to require to download the
--              tblQATWorkFlow when the data in the table is changed.
-- =====================================================================
CREATE TRIGGER [dbo].[tgrQATWorkFlow]
ON [dbo].[tblQATWorkFlow]
AFTER INSERT, UPDATE, DELETE 
AS
   UPDATE dbo.tblDownLoadTableList set active = 1 where TableName = 'tblQATWorkFlow'

GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 = Active; 0 = Inactive', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATWorkFlow', @level2type = N'COLUMN', @level2name = N'Active';


GO

