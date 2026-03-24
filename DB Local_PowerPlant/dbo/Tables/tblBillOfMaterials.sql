CREATE TABLE [dbo].[tblBillOfMaterials] (
    [Facility]      CHAR (3)        NOT NULL,
    [ShopOrder]     INT             NOT NULL,
    [SequenceNo]    SMALLINT        NOT NULL,
    [ComponentItem] VARCHAR (35)    NOT NULL,
    [Quantity]      DECIMAL (15, 6) NOT NULL,
    [UnitOfMeasure] CHAR (2)        NOT NULL,
    [Blend]         VARCHAR (11)    NULL,
    [Grind]         VARCHAR (6)     NULL,
    CONSTRAINT [PK_tblBillOfMaterials_1] PRIMARY KEY CLUSTERED ([ShopOrder] ASC, [SequenceNo] ASC)
);


GO

