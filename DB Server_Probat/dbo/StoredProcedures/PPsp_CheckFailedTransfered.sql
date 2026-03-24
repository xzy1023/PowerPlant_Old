CREATE PROCEDURE [dbo].[PPsp_CheckFailedTransfered] AS BEGIN

	DECLARE @currentTime DATETIME = GETDATE();
	DECLARE @timeDiffMins as int = -15;
    DECLARE @countBlends INT, @countCommand INT, @countOrder INT, @countOrderRec INT, @countPallet INT, @countTypes INT;
    DECLARE @vchMsgBody NVARCHAR(MAX) = '';

	-- Count failed records for each table
	SELECT @countBlends = COUNT(*) FROM dbo.PRO_IMP_BLENDS WHERE TRANSFERED = -1 AND TRANSFERED_TIMESTAMP >= DATEADD(MINUTE, @timeDiffMins, @currentTime);
    SELECT @countCommand = COUNT(*) FROM dbo.PRO_IMP_COMMAND WHERE TRANSFERED = -1 AND TRANSFERED_TIMESTAMP >= DATEADD(MINUTE, @timeDiffMins, @currentTime);
    SELECT @countOrder = COUNT(*) FROM dbo.PRO_IMP_ORDER WHERE TRANSFERED = -1 AND TRANSFERED_TIMESTAMP >= DATEADD(MINUTE, @timeDiffMins, @currentTime);
    SELECT @countOrderRec = COUNT(*) FROM dbo.PRO_IMP_ORDER_REC WHERE TRANSFERED = -1 AND TRANSFERED_TIMESTAMP >= DATEADD(MINUTE, @timeDiffMins, @currentTime);
    SELECT @countPallet = COUNT(*) FROM dbo.PRO_IMP_PALLET WHERE TRANSFERED = -1 AND TRANSFERED_TIMESTAMP >= DATEADD(MINUTE, @timeDiffMins, @currentTime);
    SELECT @countTypes = COUNT(*) FROM dbo.PRO_IMP_TYPES WHERE TRANSFERED = -1 AND TRANSFERED_TIMESTAMP >= DATEADD(MINUTE, @timeDiffMins, @currentTime);

	-- Build email body with details
    IF @countBlends > 0 SET @vchMsgBody += 'PRO_IMP_BLENDS: ' + CAST(@countBlends AS NVARCHAR) + ' failed records.' + CHAR(13) + CHAR(10);
    IF @countCommand > 0 SET @vchMsgBody += 'PRO_IMP_COMMAND: ' + CAST(@countCommand AS NVARCHAR) + ' failed records.' + CHAR(13) + CHAR(10);
    IF @countOrder > 0 SET @vchMsgBody += 'PRO_IMP_ORDER: ' + CAST(@countOrder AS NVARCHAR) + ' failed records.' + CHAR(13) + CHAR(10);
    IF @countOrderRec > 0 SET @vchMsgBody += 'PRO_IMP_ORDER_REC: ' + CAST(@countOrderRec AS NVARCHAR) + ' failed records.' + CHAR(13) + CHAR(10);
--     IF @countPallet > 0 SET @vchMsgBody += 'PRO_IMP_PALLET: ' + CAST(@countPallet AS NVARCHAR) + ' failed records.' + CHAR(13) + CHAR(10);
    IF @countTypes > 0 SET @vchMsgBody += 'PRO_IMP_TYPES: ' + CAST(@countTypes AS NVARCHAR) + ' failed records.' + CHAR(13) + CHAR(10);

	select @vchMsgBody, LEN(@vchMsgBody)

    -- Send email if any failures found
    IF LEN(@vchMsgBody) > 0
    BEGIN
        DECLARE @vchServerName VARCHAR(50) = '';
        SELECT @vchServerName = CAST(SERVERPROPERTY('ServerName') AS NVARCHAR(50));
		DECLARE @msgSubject VARCHAR(100) = @vchServerName + ' Probat Failed Transfers Detected'

        EXEC msdb..sp_notify_operator   
            @profile_name = 'PowerPlantSupport',
            @name = 'PP ZXiao',    
            @subject = @msgSubject,
            @body = @vchMsgBody;
    END
END

GO

