CREATE TABLE [dbo].[@del_tblWeightCaptureRecords] (
    [wt_RecordNumber1]              FLOAT (53)   NULL,
    [wt_ShopOrderRec]               NVARCHAR (8) NULL,
    [wt_SKUNumberRec]               NVARCHAR (8) NULL,
    [Date and Time of Weight Check] DATETIME     NULL,
    [wt_LineNumber]                 SMALLINT     NULL,
    [wt_ShiftNumber]                SMALLINT     NULL,
    [wt_OperatorNumber]             SMALLINT     NULL,
    [wt_TechnicianNumber]           SMALLINT     NULL,
    [wt_WeightSource]               SMALLINT     NULL,
    [wt_LabelWeight]                FLOAT (53)   NULL,
    [wt_TargetWeight]               FLOAT (53)   NULL,
    [wt_LCL_Weight]                 FLOAT (53)   NULL,
    [wt_UCL_Weight]                 FLOAT (53)   NULL,
    [wt_ActualWeight]               FLOAT (53)   NULL,
    [wt_AugerRevs]                  FLOAT (53)   NULL,
    [wt_GramsPerRevolution]         FLOAT (53)   NULL,
    [# of Bags Between Samples]     SMALLINT     NULL,
    [wt_BagsProduced]               FLOAT (53)   NULL,
    [wt_prod_record_Link]           FLOAT (53)   NULL
);


GO

