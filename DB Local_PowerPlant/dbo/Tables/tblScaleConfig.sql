CREATE TABLE [dbo].[tblScaleConfig] (
    [ScaleBrand]    VARCHAR (50) NOT NULL,
    [Model]         VARCHAR (50) NOT NULL,
    [BaudRate]      INT          NOT NULL,
    [Parity]        VARCHAR (50) NOT NULL,
    [DataBit]       SMALLINT     NOT NULL,
    [StopBit]       SMALLINT     NOT NULL,
    [SpecCommCable] BIT          NOT NULL
);


GO

