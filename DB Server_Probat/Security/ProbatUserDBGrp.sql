CREATE ROLE [ProbatUserDBGrp]
    AUTHORIZATION [dbo];


GO

ALTER ROLE [ProbatUserDBGrp] ADD MEMBER [ppuser];


GO

ALTER ROLE [ProbatUserDBGrp] ADD MEMBER [MPHO\pp_it_support];


GO

ALTER ROLE [ProbatUserDBGrp] ADD MEMBER [MPHO\PP_IPC_User];


GO

ALTER ROLE [ProbatUserDBGrp] ADD MEMBER [probatuser];


GO

