CREATE TABLE [dbo].[tblKronosCostCenterXref] (
    [Active]                      BIT           NOT NULL,
    [Facility]                    VARCHAR (3)   NOT NULL,
    [PPWorkCenter]                INT           NOT NULL,
    [PPWorkCenterDescription]     VARCHAR (255) NOT NULL,
    [KronosCostCentre]            INT           NOT NULL,
    [KronosCostCentreDescription] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_tblKronosCostCenterXref] PRIMARY KEY CLUSTERED ([Facility] ASC, [PPWorkCenter] ASC, [Active] ASC)
);


GO

