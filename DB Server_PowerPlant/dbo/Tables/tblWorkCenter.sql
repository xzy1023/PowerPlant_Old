CREATE TABLE [dbo].[tblWorkCenter] (
    [Facility]    CHAR (3)     NOT NULL,
    [WorkCenter]  INT          NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    [Active]      BIT          NULL,
    CONSTRAINT [PK_tblCostCenter] PRIMARY KEY CLUSTERED ([Facility] ASC, [WorkCenter] ASC)
);


GO

