
-- =============================================
-- Created:	    Oct. 25, 2018   Bong Lee
-- Description:	Monitor Probat Green Bean Order Receiving Error
--				If it is great than the threshold, send out alert email.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_MonitorProbatOrderRecError]
	@intThresholdUpperLimit as integer = 45
	,@intThreholdBeyondLimit as integer = 90
AS
BEGIN

	Declare @tblTemp table (
	[Site]					varchar(3)
	,CUSTOMER_ID int
	,TRANSFERED_TIMESTAMP	datetime
	, WaitedTime			int
	, ORDER_NAME			varchar(20)
	, DELIVERY_NAME			varchar(20)
	, SCHEDULED_TIME		datetime
	, AMOUNT_ORDER			int
	, AMOUNT_DELIVERY		int
	, CUSTOMER_CODE			varchar(20)
	, LOT_NAME				varchar(20)
	, DATA_1				varchar(20)
	, DATA_11				varchar(20)
	, DATA_12				varchar(20)
	);

	INSERT INTO @tblTemp 
	SELECT '01', CUSTOMER_ID,  TRANSFERED_TIMESTAMP, datediff(MINUTE ,TRANSFERED_TIMESTAMP , getdate() ), ORDER_NAME, DELIVERY_NAME, SCHEDULED_TIME, AMOUNT_ORDER, AMOUNT_DELIVERY, CUSTOMER_CODE, 
	   LOT_NAME, DATA_1, DATA_11, DATA_12
	FROM          Probat01_Prd.dbo.PRO_IMP_ORDER_REC
	Where TRANSFERED = -1 and zone = 1 and ORDER_NAME = '' 
		and TRANSFERED_TIMESTAMP > '2022-02-09' 
		and (datediff(MINUTE ,TRANSFERED_TIMESTAMP , getdate() ) Between @intThresholdUpperLimit and @intThreholdBeyondLimit)
	union
	SELECT '09', CUSTOMER_ID,  TRANSFERED_TIMESTAMP, datediff(MINUTE ,TRANSFERED_TIMESTAMP , getdate() ), ORDER_NAME, DELIVERY_NAME, SCHEDULED_TIME, AMOUNT_ORDER, AMOUNT_DELIVERY, CUSTOMER_CODE, 
	   LOT_NAME, DATA_1, DATA_11, DATA_12
	FROM MPSPPP01.[Probat09_Prd].[dbo].[PRO_IMP_ORDER_REC]
	Where TRANSFERED = -1 and zone = 1 and ORDER_NAME = '' 
		and TRANSFERED_TIMESTAMP > '2022-02-09' 
		and (datediff(MINUTE ,TRANSFERED_TIMESTAMP , getdate() ) Between @intThresholdUpperLimit and @intThreholdBeyondLimit)
	union
	SELECT '07', CUSTOMER_ID,  TRANSFERED_TIMESTAMP, datediff(MINUTE ,TRANSFERED_TIMESTAMP ,DATEADD(minute,-60,getdate()) ), ORDER_NAME, DELIVERY_NAME, SCHEDULED_TIME, AMOUNT_ORDER, AMOUNT_DELIVERY, CUSTOMER_CODE, 
	   LOT_NAME, DATA_1, DATA_11, DATA_12
	FROM MPFWPP01.[Probat07_Prd].[dbo].[PRO_IMP_ORDER_REC]
	Where TRANSFERED = -1 and zone = 1 and ORDER_NAME = '' 
		and TRANSFERED_TIMESTAMP > '2022-02-09' 
		and (datediff(MINUTE ,TRANSFERED_TIMESTAMP , DATEADD(minute,-60,getdate())) Between @intThresholdUpperLimit and @intThreholdBeyondLimit)

	  Declare
	  @vchMsgBody nvarchar (MAX) = '' 
		,@vchSubject nvarchar(512) = 'Monitor Probat Order Rec. Error - Waited Time Between ' + CAST(@intThresholdUpperLimit as varchar(3)) + ' and ' + CAST(@intThreholdBeyondLimit as varchar(3)) + ' minutes' 
		,@vchProfileName nvarchar(128) = 'PowerPlantSupport'								 
		,@vchName nvarchar(128) = N'ZXiao@mother-parkers.com;SKumthekar@mother-parkers.com'


	select @vchMsgBody = N'
	<style>
	table.GeneratedTable {
	  width: 100%;
	  background-color: #ffffff;
	  border-collapse: collapse;
	  border-width: 2px;
	  border-color: #ffcc00;
	  border-style: solid;
	  color: #000000;
	}

	table.GeneratedTable td, table.GeneratedTable th {
	  border-width: 2px;
	  border-color: #ffcc00;
	  border-style: solid;
	  padding: 3px;
	}

	table.GeneratedTable thead {
	  background-color: #ffcc00;
	}
	</style>


	<table class="GeneratedTable">
	  <thead>
		<tr>
		  <th>Site</th>
		  <th>CUSTOMER ID</th>
		  <th>TRANSFERED TIMESTAMP</th>
		  <th>Waited (Min.)</th>
		  <th>ORDER NAME</th>
		  <th>DELIVERY NAME</th>
		  <th>SCHEDULED TIME</th>
		  <th>AMOUNT ORDER</th>
		  <th>AMOUNT DELIVERY</th>
		  <th>CUSTOMER CODE</th>
		  <th>LOT NAME</th>
		  <th>DATA 1</th>
		  <th>DATA 11</th>
		  <th>DATA 12</th>
		</tr>
	  </thead>
	  <tbody>' +

	--<table border="1">' +
	--		N'<tr><th>Test No</th><th>Product Date</th><th>Time</th><th>Blend</th><th>Grind</th><th style=''color:red''>Input</th><th>Spec. Max</th><th>Spec. Min</th><th>Reason</th>' +
	--		N'</tr>' +
			ISNULL( CAST ((
			SELECT 
				td=[Site],'',
				td=CUSTOMER_ID,'',
				td=TRANSFERED_TIMESTAMP, '',
				td=WaitedTime, '',
				td=ORDER_NAME, '', 
				td=DELIVERY_NAME,'',
				td=SCHEDULED_TIME,'',
				td=AMOUNT_ORDER, '',
				td=AMOUNT_DELIVERY, '',
				td=CUSTOMER_CODE,'',
				td=LOT_NAME,'',
				td=DATA_1,'',
				td=DATA_11,'', 
				td=DATA_12,'' 
				FROM @tblTemp --PRO_IMP_ORDER_REC
			FOR XML PATH('tr'), TYPE 
		
						) AS NVARCHAR(MAX) ) ,'') +
		N'</table>' ;

		IF exists (select 1 from @tblTemp)
			EXEC msdb.dbo.sp_send_dbmail
			@profile_name = @vchProfileName,  
			@recipients = @vchName,  
			@body = @vchMsgBody,
			@body_format = 'HTML',
			@subject =  @vchSubject
		else
			PRINT 'No Data'
END

--GO
--Grant execute on object :: PPsp_MonitorProbatOrderRecError to [ProbatUserDBGrp]

GO

