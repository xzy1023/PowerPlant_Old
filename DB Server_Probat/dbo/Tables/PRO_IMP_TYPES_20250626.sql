CREATE TABLE [dbo].[PRO_IMP_TYPES_20250626] (
    [CUSTOMER_ID]          INT           NOT NULL,
    [TRANSFERED]           INT           NOT NULL,
    [TRANSFERED_TIMESTAMP] DATETIME      NULL,
    [ACTIVITY]             VARCHAR (1)   NOT NULL,
    [ZONE]                 INT           NOT NULL,
    [CUSTOMER_CODE]        VARCHAR (20)  NOT NULL,
    [NAME]                 VARCHAR (20)  NULL,
    [MACO]                 VARCHAR (20)  NULL,
    [HOLDING_TIME]         INT           NULL,
    [COLOR]                INT           NULL,
    [COLOR_MIN]            INT           NULL,
    [COLOR_MAX]            INT           NULL,
    [DENSITY]              INT           NULL,
    [DENSITY_MIN]          INT           NULL,
    [DENSITY_MAX]          INT           NULL,
    [PACKAGE_SIZE]         INT           NULL,
    [HUMIDITY]             INT           NULL,
    [HUMIDITY_MIN]         INT           NULL,
    [HUMIDITY_MAX]         INT           NULL,
    [CALC_LOSS]            INT           NULL,
    [DESCRIPTION]          VARCHAR (200) NULL,
    [SIEVE_TARGET_1]       INT           NULL,
    [SIEVE_MIN_1]          INT           NULL,
    [SIEVE_MAX_1]          INT           NULL,
    [SIEVE_TARGET_2]       INT           NULL,
    [SIEVE_MIN_2]          INT           NULL,
    [SIEVE_MAX_2]          INT           NULL,
    [SIEVE_TARGET_3]       INT           NULL,
    [SIEVE_MIN_3]          INT           NULL,
    [SIEVE_MAX_3]          INT           NULL,
    [KIND_OF_COFFEE]       INT           NULL
);


GO

