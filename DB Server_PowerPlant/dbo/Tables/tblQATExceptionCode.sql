CREATE TABLE [dbo].[tblQATExceptionCode] (
    [Active]           BIT           CONSTRAINT [DF_tblQATExceptionCode_Active] DEFAULT ((1)) NOT NULL,
    [Description]      VARCHAR (150) NOT NULL,
    [ExceptionCode]    INT           NOT NULL,
    [ExceptionSubCode] INT           CONSTRAINT [DF_tblQATExceptionCode_ExceptionSubCode] DEFAULT ((0)) NOT NULL,
    [InclOrExcl]       BIT           CONSTRAINT [DF_tblQATExceptionCode_Action] DEFAULT ((1)) NOT NULL,
    [Facility]         VARCHAR (3)   NOT NULL,
    [UpdatedAt]        DATETIME      CONSTRAINT [DF_tblQATExceptionCode_UpdatedAt] DEFAULT (getdate()) NULL,
    [UpdatedBy]        VARCHAR (50)  NULL,
    [Value1]           VARCHAR (50)  NULL,
    [Value2]           VARCHAR (50)  NULL,
    [Value3]           VARCHAR (50)  NULL,
    [Value4]           VARCHAR (50)  NULL,
    CONSTRAINT [PK_tblQATExceptionCode] PRIMARY KEY CLUSTERED ([ExceptionCode] ASC, [Facility] ASC, [Active] ASC, [ExceptionSubCode] ASC)
);


GO


-- =====================================================================
-- Author:		Bong Lee
-- Create date: Mar 19, 2019
-- Description:	Flag the Down Load Table List to require to download the
--              tblQATExceptionCode when the data in the table is changed.
-- =====================================================================
CREATE TRIGGER [dbo].[tgrQATExceptionCode]
ON [dbo].[tblQATExceptionCode]
AFTER INSERT, UPDATE, DELETE 
AS
   UPDATE dbo.tblDownLoadTableList set active = 1 where TableName = 'tblQATExceptionCode'

GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The user defined values should be 1 = Inclusive or 0 = Exclusive to the exception.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATExceptionCode', @level2type = N'COLUMN', @level2name = N'InclOrExcl';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User defined', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATExceptionCode', @level2type = N'COLUMN', @level2name = N'Value1';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User defined', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATExceptionCode', @level2type = N'COLUMN', @level2name = N'Value2';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User defined', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATExceptionCode', @level2type = N'COLUMN', @level2name = N'Value3';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User defined', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATExceptionCode', @level2type = N'COLUMN', @level2name = N'Value4';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is an extension of the Exception Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATExceptionCode', @level2type = N'COLUMN', @level2name = N'ExceptionSubCode';


GO

