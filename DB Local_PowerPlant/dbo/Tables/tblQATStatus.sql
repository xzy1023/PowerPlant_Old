CREATE TABLE [dbo].[tblQATStatus] (
    [ByPassAllTests]      BIT          NOT NULL,
    [ByPassTest]          BIT          NOT NULL,
    [ShopOrder]           INT          NOT NULL,
    [QATEntryPoint]       CHAR (1)     NOT NULL,
    [QATDefnID]           INT          NOT NULL,
    [InterfaceID]         VARCHAR (24) NOT NULL,
    [NextInterfaceFormID] VARCHAR (50) NOT NULL,
    [NextQATDefnID]       INT          NOT NULL,
    [NextQATEntryPoint]   CHAR (1)     NOT NULL,
    [NextWFTestSeq]       INT          NOT NULL,
    [ShiftChanged]        BIT          NOT NULL,
    [ShopOrderClosed]     BIT          NOT NULL,
    [UpdateAt]            DATETIME     NOT NULL,
    [WFTestSeq]           INT          NOT NULL
);


GO

