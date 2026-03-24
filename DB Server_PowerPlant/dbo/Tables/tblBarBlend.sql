CREATE TABLE [dbo].[tblBarBlend] (
    [Facility]   NCHAR (3)      NULL,
    [BarBlend]   NCHAR (6)      NOT NULL,
    [Blend]      NCHAR (6)      NOT NULL,
    [Percentage] DECIMAL (3, 3) NOT NULL,
    CONSTRAINT [PK_tblBarBlend] PRIMARY KEY CLUSTERED ([BarBlend] ASC, [Blend] ASC)
);


GO

