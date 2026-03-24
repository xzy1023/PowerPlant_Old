CREATE TABLE [dbo].[tblOutputLocation] (
    [Facility]                 VARCHAR (3)  NOT NULL,
    [Active]                   BIT          CONSTRAINT [DF_tblOutputLocation_Active] DEFAULT ((1)) NOT NULL,
    [PackagingLine]            CHAR (10)    NOT NULL,
    [Location]                 VARCHAR (10) NOT NULL,
    [UpdatedBy]                VARCHAR (50) NOT NULL,
    [UpdatedAt]                DATETIME     CONSTRAINT [DF_tblOutputLocation_UpdatedAt] DEFAULT (getdate()) NOT NULL,
    [DestinationPackagingLine] CHAR (10)    NOT NULL,
    CONSTRAINT [PK_tblOutputLocation] PRIMARY KEY CLUSTERED ([Facility] ASC, [PackagingLine] ASC, [Location] ASC)
);


GO


-- =====================================================================
-- Author:		Bong Lee
-- Create date: Sep 28, 2015
-- Description:	Flag the Down Load Table List to require to download the
--              tblOutputLocation when the data in the table is changed.
-- =====================================================================
CREATE TRIGGER [dbo].[tgrOutputLocation]
ON [dbo].[tblOutputLocation]
AFTER INSERT, UPDATE, DELETE 
AS
   update dbo.tblDownLoadTableList set active = 1 where TableName = 'tblOutputLocation'

GO

