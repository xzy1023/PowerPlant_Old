CREATE ROLE [PPUserDBGrp]
    AUTHORIZATION [dbo];


GO

ALTER ROLE [PPUserDBGrp] ADD MEMBER [dataimporter];


GO

