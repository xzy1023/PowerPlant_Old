CREATE TABLE [dbo].[tblDateFmtCode] (
    [Active]      BIT          CONSTRAINT [DF_tblDateCodeFormat_Active] DEFAULT ((1)) NOT NULL,
    [DateCode]    CHAR (2)     NOT NULL,
    [DateFormat]  VARCHAR (25) NOT NULL,
    [Descripton]  VARCHAR (50) NOT NULL,
    [UpdatedBy]   VARCHAR (50) NOT NULL,
    [LastUpdated] DATETIME     CONSTRAINT [DF_tblDateCodeFormat_LastUpdated] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblDateCodeFormat] PRIMARY KEY CLUSTERED ([DateCode] ASC)
);


GO

