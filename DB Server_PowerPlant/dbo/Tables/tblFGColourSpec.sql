CREATE TABLE [dbo].[tblFGColourSpec] (
    [Active]        BIT            CONSTRAINT [DF_tblFGColourSpec_Active] DEFAULT ((1)) NOT NULL,
    [EffectiveDate] DATETIME       NOT NULL,
    [Blend]         VARCHAR (6)    NOT NULL,
    [Grind]         VARCHAR (6)    NOT NULL,
    [SpecMin]       DECIMAL (5, 2) NULL,
    [SpecMax]       DECIMAL (5, 2) NULL,
    [SpecTarg]      DECIMAL (5, 2) NULL,
    [CreationDate]  DATETIME       NULL,
    [CreatedBy]     VARCHAR (50)   NULL,
    CONSTRAINT [PK_tblFGColourSpec] PRIMARY KEY CLUSTERED ([EffectiveDate] ASC, [Blend] ASC, [Grind] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblFGColourSpec]
    ON [dbo].[tblFGColourSpec]([Blend] ASC);


GO


-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 22, 2013
-- Description:	Flag the Roasting Specifications Update List table to indicate 
--				tblFGColourSpec has been changed and will be required 
--				synchronization with the same table in other server. 
-- =====================================================================
CREATE TRIGGER [dbo].[tgrFGColourSpec]
   ON  [dbo].[tblFGColourSpec]
   AFTER INSERT, UPDATE, DELETE
AS 
BEGIN
	UPDATE [dbo].[tblRoastingSpecsUpdList] 
		SET Updated = 1, Lastupdate = getdate()  
		WHERE Facility = '09' AND TableName = 'tblFGColourSpec'
END

GO

