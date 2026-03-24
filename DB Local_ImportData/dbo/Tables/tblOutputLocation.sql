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

