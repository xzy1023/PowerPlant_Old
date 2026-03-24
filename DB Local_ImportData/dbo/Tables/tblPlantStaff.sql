CREATE TABLE [dbo].[tblPlantStaff] (
    [ActiveRecord]     BIT          NOT NULL,
    [Facility]         CHAR (3)     NOT NULL,
    [WorkGroup]        VARCHAR (10) NOT NULL,
    [StaffID]          VARCHAR (10) NOT NULL,
    [FirstName]        VARCHAR (50) NOT NULL,
    [LastName]         VARCHAR (50) NOT NULL,
    [StaffClass]       VARCHAR (50) NOT NULL,
    [Password]         VARCHAR (50) NULL,
    [ResetPassword]    BIT          NOT NULL,
    [DateLastChange]   DATETIME     NULL,
    [WorkSubGroup]     VARCHAR (10) NOT NULL,
    [PrintAccessLevel] INT          CONSTRAINT [DF_tblPlantStaff_PrintAccessLevel] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_tblUserProfile] PRIMARY KEY CLUSTERED ([Facility] ASC, [WorkGroup] ASC, [StaffID] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Access level to print LPs and Case Labels 1=Everything 2=No reprint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPlantStaff', @level2type = N'COLUMN', @level2name = N'PrintAccessLevel';


GO

