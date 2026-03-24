CREATE TABLE [dbo].[tblQATTaskMaster] (
    [Active]          BIT          CONSTRAINT [DF_tblQATTaskMaster_Active] DEFAULT ((1)) NOT NULL,
    [Facility]        VARCHAR (3)  NOT NULL,
    [TaskID]          INT          NOT NULL,
    [TaskDescription] VARCHAR (50) NOT NULL,
    [UpdatedAt]       DATETIME     NULL,
    [UpdatedBy]       VARCHAR (50) NULL,
    CONSTRAINT [PK_tblQATTaskMaster] PRIMARY KEY CLUSTERED ([TaskID] ASC)
);


GO

