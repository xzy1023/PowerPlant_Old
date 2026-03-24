CREATE TABLE [dbo].[tblEquipment_ScrapRejectPoint] (
    [RRN]           INT       IDENTITY (1, 1) NOT NULL,
    [EquipmentID]   CHAR (10) NOT NULL,
    [RejectPointID] INT       NOT NULL,
    [VisibleInIPC]  BIT       NOT NULL,
    CONSTRAINT [PK_tblEquipment_ScrapRejectPoint] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

