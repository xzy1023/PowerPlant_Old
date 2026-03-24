CREATE TABLE [dbo].[tblPackagingLineRecipe] (
    [Facility]              CHAR (3)       NOT NULL,
    [PackagingLine]         VARCHAR (50)   NOT NULL,
    [ItemNumber]            VARCHAR (35)   NOT NULL,
    [ChangePart]            VARCHAR (5)    NULL,
    [StartingServoPosition] DECIMAL (6, 2) NULL,
    [VibratorSetting]       DECIMAL (6, 2) NULL,
    [LastUpdate]            DATETIME       NOT NULL,
    CONSTRAINT [PK_tblCanLineRecipe] PRIMARY KEY CLUSTERED ([Facility] ASC, [PackagingLine] ASC, [ItemNumber] ASC)
);


GO

