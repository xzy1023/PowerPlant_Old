CREATE TABLE [dbo].[tblQATForm] (
    [Active]          BIT          CONSTRAINT [DF_tblQATForm_Active] DEFAULT ((1)) NOT NULL,
    [Facility]        VARCHAR (3)  NOT NULL,
    [InterfaceFormID] VARCHAR (50) NOT NULL,
    [TestFormID]      INT          NOT NULL,
    [TestCategory]    VARCHAR (50) NOT NULL,
    [FormName]        VARCHAR (50) NOT NULL,
    [TableName]       VARCHAR (50) NULL,
    [UpdatedAt]       DATETIME     CONSTRAINT [DF_tblQATForm_UpdatedAt] DEFAULT (getdate()) NULL,
    [UpdatedBy]       VARCHAR (50) NULL,
    CONSTRAINT [PK_tblQATForm] PRIMARY KEY CLUSTERED ([TestFormID] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblQATForm]
    ON [dbo].[tblQATForm]([InterfaceFormID] ASC, [Active] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 = Active; 0 = Inactive', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATForm', @level2type = N'COLUMN', @level2name = N'Active';


GO

