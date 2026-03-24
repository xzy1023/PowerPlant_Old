CREATE TABLE [dbo].[tblSpecialScrap] (
    [Active]      BIT          CONSTRAINT [DF_tblSpecialScrap_Active] DEFAULT ((1)) NOT NULL,
    [Component]   VARCHAR (35) NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    [Type]        VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_tblSpecialScrap] PRIMARY KEY CLUSTERED ([Component] ASC)
);


GO

