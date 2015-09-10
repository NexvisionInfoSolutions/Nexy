
/****** Object:  StoredProcedure [dbo].[sp_Acc_rptLedger]    Script Date: 09/05/2015 05:41:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE type = 'P'
			AND NAME = 'sp_Acc_rptLedger'
		)
	DROP PROCEDURE sp_Acc_rptLedger
GO

CREATE PROCEDURE [dbo].[sp_Acc_rptLedger]
	@OperationId int =0,
	@FromDate varchar(30)='',
	@ToDate varchar(30)='',
	@AccountsIds varchar(max) =''
as 
 
	
	--declare @AccountsIds varchar(max)
	--declare @FromDate varchar(30)
	--declare @ToDate varchar(30)

	--set @FromDate='2015-06-10 00:00:00'
	--set @ToDate='2015-06-21 00:00:00'
	--set @AccountsIds='27,28,29,30,31,32,33,34,35,36,37,38,39,40'
	
	IF OBJECT_ID('tempdb.dbo.#MyTableOp', 'U') IS NOT NULL
		DROP TABLE #MyTableOp; 
	IF OBJECT_ID('tempdb.dbo.#MyTableRec', 'U') IS NOT NULL
		DROP TABLE #MyTableRec; 
	IF OBJECT_ID('tempdb.dbo.#MyTablePay', 'U') IS NOT NULL
		DROP TABLE #MyTablePay; 
	IF OBJECT_ID('tempdb.dbo.#MyTableBankDep', 'U') IS NOT NULL
		DROP TABLE #MyTableBankDep; 
	IF OBJECT_ID('tempdb.dbo.#MyTableBankWith', 'U') IS NOT NULL
		DROP TABLE #MyTableBankWith; 
	IF OBJECT_ID('tempdb.dbo.#MyTableJournal', 'U') IS NOT NULL
		DROP TABLE #MyTableJournal; 
		
	CREATE table #MyTableOp  
			(
			TrDate varchar(30),VoucherNo varchar(30),AccountId int,GroupAcc varchar(2000),OppAccountId int,
			OppAccCode varchar(2000),OppAccName varchar(2000) ,Narration varchar(2000),DrAmount decimal(18,2),CrAmount  decimal(18,2)
			);
	  CREATE table #MyTableRec  
			(
			TrDate varchar(30),VoucherNo varchar(30),AccountId int,GroupAcc varchar(2000),OppAccountId int,
			OppAccCode varchar(2000),OppAccName varchar(2000) ,Narration varchar(2000),DrAmount decimal(18,2),CrAmount  decimal(18,2)
			);	
	  CREATE table #MyTablePay 
			(
			TrDate varchar(30),VoucherNo varchar(30),AccountId int,GroupAcc varchar(2000),OppAccountId int,
			OppAccCode varchar(2000),OppAccName varchar(2000) ,Narration varchar(2000),DrAmount decimal(18,2),CrAmount  decimal(18,2)
			);	
	  CREATE table #MyTableBankDep 
			(
			TrDate varchar(30),VoucherNo varchar(30),AccountId int,GroupAcc varchar(2000),OppAccountId int,
			OppAccCode varchar(2000),OppAccName varchar(2000) ,Narration varchar(2000),DrAmount decimal(18,2),CrAmount  decimal(18,2)
			);
	  CREATE table #MyTableBankWith 
			(
			TrDate varchar(30),VoucherNo varchar(30),AccountId int,GroupAcc varchar(2000),OppAccountId int,
			OppAccCode varchar(2000),OppAccName varchar(2000) ,Narration varchar(2000),DrAmount decimal(18,2),CrAmount  decimal(18,2)
			);
	  CREATE table #MyTableJournal 
			(
			TrDate varchar(30),VoucherNo varchar(30),AccountId int,GroupAcc varchar(2000),OppAccountId int,
			OppAccCode varchar(2000),OppAccName varchar(2000) ,Narration varchar(2000),DrAmount decimal(18,2),CrAmount  decimal(18,2)
			);	
	  
	  declare @sql nvarchar(Max) 
	  --set @ToDate=@ToDate + ' 23:59:59'
	if(@OperationId =0)
		begin
			print 'Opening'
	  Set @sql='
					insert into #MyTableOp  (TrDate,VoucherNo,AccountId,GroupAcc,OppAccountId,OppAccCode,OppAccName,Narration,DrAmount,CrAmount) 	 	
					select TrDate,VoucherNo,AccountId,GroupAcc,OppAccountId,OppAccCode,OppAccName,Narration,sum(DrAmount),sum(CrAmount) from (
						
						SELECT     '''' as TrDate ,'''' as VoucherNo, AccReceiptsDetail.AccountId, AccHeads_1.AccountName AS GroupAcc,0 as OppAccountId,
						    '''' AS OppAccCode, 
							''Opening Balance'' AS OppAccName, ''Opening Balance'' as Narration, case when sum( AccReceiptsDetail.Amount) >0 then 
							sum( AccReceiptsDetail.Amount) else 0  
							end AS DrAmount, case when sum( AccReceiptsDetail.Amount) <0 then sum( AccReceiptsDetail.Amount) else 0  end AS CrAmount 
							FROM   AccReceiptsDetail INNER JOIN
								   AccReceiptsHeader ON AccReceiptsDetail.ReceiptsId = AccReceiptsHeader.Id INNER JOIN
								   AccountHead AS AccHeads_1 ON AccReceiptsDetail.AccountId = AccHeads_1.AccountHeadId
							WHERE       
							convert(datetime,AccReceiptsHeader.TransDate,102) <
								   CONVERT(DATETIME, ''' + @FromDate + ''', 102)
							and (AccReceiptsHeader.TransType = 1) and AccReceiptsDetail.AccountId in ('+ @AccountsIds +') 
							and AccReceiptsHeader.Cancelled=0
							GROUP BY AccReceiptsDetail.AccountId,AccHeads_1.AccountName
							
							Union all
							
							SELECT    '''' as TrDate ,'''' as VoucherNo, AccReceiptsDetail.AccountId, AccHeads_1.AccountName AS GroupAcc,0 as OppAccountId, '''' AS OppAccCode, 
							''Opening Balance'' AS OppAccName, ''Opening Balance'' as Narration,
							case when sum( AccReceiptsDetail.Amount) >0 then sum( AccReceiptsDetail.Amount) else 0  end AS DrAmount, 
							case when sum( AccReceiptsDetail.Amount) <0 then (-1)* sum( AccReceiptsDetail.Amount) else 0  end AS CrAmount 
							FROM         AccReceiptsDetail INNER JOIN
												  AccReceiptsHeader ON AccReceiptsDetail.ReceiptsId = AccReceiptsHeader.Id 
												  --left outer JOIN
												  --AccountHead ON AccReceiptsDetail.OppAccountId = AccountHead.AccountHeadId 
												  INNER JOIN
												  AccountHead AS AccHeads_1 ON AccReceiptsDetail.AccountId = AccHeads_1.AccountHeadId
							WHERE      
							convert(datetime,AccReceiptsHeader.TransDate,102) <
												   CONVERT(DATETIME, ''' + @FromDate + ''', 102)
							and (AccReceiptsHeader.TransType = 2) and AccReceiptsDetail.AccountId in ('+ @AccountsIds +')
							and AccReceiptsHeader.Cancelled=0
							GROUP BY AccReceiptsDetail.AccountId,AccHeads_1.AccountName 
							
							union all
							
							SELECT   '''' as TrDate , '''' as VoucherNo, AccBankDepositDetail.AccountId, AccHeads_1.AccountName AS GroupAcc, 0 as OppAccountId, '''' AS OppAccCode, 
							  ''Opening Balance'' AS OppAccName, ''Opening Balance'' as Narration, 0 AS DrAmount, sum(AccBankDepositDetail.Amount) AS CrAmount
							FROM         AccBankDepositDetail INNER JOIN
												  AccBankDepositHeader ON AccBankDepositDetail.BankDepositId = AccBankDepositHeader.Id 
												  --LEFT OUTER JOIN
												  --AccountHead ON AccBankDepositDetail.OppAccountId = AccountHead.AccountHeadId 
												  INNER JOIN
												  AccountHead AS AccHeads_1 ON AccBankDepositDetail.AccountId = AccHeads_1.AccountHeadId
							WHERE     
							convert(datetime,AccBankDepositHeader.TransDate,102) <
												   CONVERT(DATETIME, ''' + @FromDate + ''', 102)
							AND (AccBankDepositHeader.TransType = 1) and AccBankDepositDetail.AccountId in ('+ @AccountsIds +')
							and AccBankDepositHeader.Cancelled=0
							GROUP BY AccBankDepositDetail.AccountId,  AccHeads_1.AccountName

							union all

							SELECT  '''' as TrDate , '''' as VoucherNo,  AccBankDepositDetail.AccountId, AccHeads_1.AccountName AS GroupAcc, 0 as OppAccountId, '''' AS OppAccCode, 
									''Opening Balance'' AS OppAccName, ''Opening Balance'' as Narration,  sum(AccBankDepositDetail.Amount) AS DrAmount, 0 AS CrAmount
							FROM    AccBankDepositDetail INNER JOIN
												  AccBankDepositHeader ON AccBankDepositDetail.BankDepositId = AccBankDepositHeader.Id 
												  --LEFT OUTER JOIN
												  --AccountHead ON AccBankDepositDetail.OppAccountId = AccountHead.AccountHeadId 
												  INNER JOIN
												  AccountHead AS AccHeads_1 ON AccBankDepositDetail.AccountId = AccHeads_1.AccountHeadId
							WHERE      
								convert(datetime,AccBankDepositHeader.TransDate,102) <
												   CONVERT(DATETIME, ''' + @FromDate + ''', 102)
								AND (AccBankDepositHeader.TransType = 2) and AccBankDepositDetail.AccountId in ('+ @AccountsIds +')
								and AccBankDepositHeader.Cancelled=0
								GROUP BY AccBankDepositDetail.AccountId,  AccHeads_1.AccountName

							union all

							SELECT  '''' as TrDate , '''' as VoucherNo, AccJournalDetail.AccountId, AccountHead.AccountName, 0 AS OppAccId, '''' AS code,
													   ''Opening Balance'', ''Opening Balance'' as Narration, sum(AccJournalDetail.DrAmount),(-1)* sum(AccJournalDetail.CrAmount)
							FROM         AccJournalDetail INNER JOIN
												  AccJournalHeader ON AccJournalDetail.JournalId = AccJournalHeader.Id INNER JOIN
												  AccountHead ON AccJournalDetail.AccountId = AccountHead.AccountHeadId

							WHERE       
								convert(datetime,AccJournalHeader.TransDate,102) <
								CONVERT(DATETIME, ''' + @FromDate + ''', 102) and AccJournalDetail.AccountId in ('+ @AccountsIds +')
								and AccJournalHeader.Cancelled=0
							group by  AccJournalDetail.AccountId, AccountHead.AccountName   
							
							union all
							
							SELECT    '''' as TrDate ,'''' as VoucherNo, OpeningBalance.AccountHeadId, AccountHead.AccountName, 0 as OppAccountId, '''' AS OppAccCode, 
												  ''Opening Balance'' AS OppAccName, ''Opening Balance'' as Narration,   OpeningBalance.DebitOpeningBalance, 
										  OpeningBalance.CreditOpeningBalance
							FROM         OpeningBalance INNER JOIN
												  AccountHead ON OpeningBalance.AccountHeadId = AccountHead.AccountHeadId		
												  where OpeningBalance.FinancialYearId=1		 and OpeningBalance.AccountHeadId in ('+ @AccountsIds +')
							  
							)
		
		as OpBal
		group by      AccountId           ,GroupAcc ,OppAccountId,OppAccCode,OppAccName,Narration,VoucherNo ,TrDate                                    
		'
		exec sp_executesql @sql	
		
		--select * from #MyTableOp
		
		print 'Receipt & payments'
		Set @sql='
		insert into #MyTableRec  (TrDate,VoucherNo,AccountId,GroupAcc,OppAccountId,OppAccCode,OppAccName,Narration,DrAmount,CrAmount) 
		SELECT    AccReceiptsHeader.TransDate as TrDate, AccReceiptsHeader.VoucherNo as VoucherNo,  AccReceiptsDetail.AccountId, AccHeads_1.AccountName 
		AS GroupAcc, 0 as OppAccountId, AccHeads_1.AccountCode AS OppAccCode, 
		CASE WHEN AccReceiptsDetail.Display = 0 THEN
                        ''To '' +  (SELECT     TOP (1)   AccountHead.AccountName
									FROM         AccReceiptsDetail AS AccReceiptsDetail_2 INNER JOIN
									AccountHead ON AccReceiptsDetail_2.AccountId = AccountHead.AccountHeadId
									WHERE     (AccReceiptsDetail_2.ReceiptsId = AccReceiptsDetail.ReceiptsId) AND (AccReceiptsDetail_2.Display = 1)
									ORDER BY CASE WHEN AccReceiptsDetail_2.amount < 0 THEN (- 1) * AccReceiptsDetail_2.amount ELSE 
									AccReceiptsDetail_2.amount END DESC) 
        ELSE
                        ''By '' +  (SELECT     TOP (1)   AccountHead.AccountName
									FROM         AccReceiptsDetail AS AccReceiptsDetail_2 INNER JOIN
									AccountHead ON AccReceiptsDetail_2.AccountId = AccountHead.AccountHeadId
									WHERE     (AccReceiptsDetail_2.ReceiptsId = AccReceiptsDetail.ReceiptsId) AND (AccReceiptsDetail_2.Display = 0)
									ORDER BY CASE WHEN AccReceiptsDetail_2.amount < 0 THEN (- 1) * AccReceiptsDetail_2.amount ELSE 
									AccReceiptsDetail_2.amount END DESC) 
        END    AS OppAccName, AccReceiptsDetail.Narration, 
		--0 AS DrAmount, 
		--(-1)*sum( AccReceiptsDetail.Amount) AS CrAmount 
		case when sum( AccReceiptsDetail.Amount) >0 then sum( AccReceiptsDetail.Amount) else 0  end AS DrAmount, 
		case when sum( AccReceiptsDetail.Amount) <0 then (-1)* sum( AccReceiptsDetail.Amount) else 0  end AS CrAmount 
		FROM        AccReceiptsDetail INNER JOIN
					AccReceiptsHeader ON AccReceiptsDetail.ReceiptsId = AccReceiptsHeader.Id INNER JOIN
					AccountHead AS AccHeads_1 ON AccReceiptsDetail.AccountId = AccHeads_1.AccountHeadId
		WHERE      
					convert(datetime,AccReceiptsHeader.TransDate,102)
					between CONVERT(DATETIME, ''' + @FromDate + ''', 102) AND 
					dateadd(day,0, CONVERT(DATETIME, ''' + @ToDate + ''', 102))
					and (AccReceiptsHeader.TransType = 1)  
					and AccReceiptsDetail.AccountId in ('+ @AccountsIds +') and AccReceiptsHeader.Cancelled=0
		GROUP BY  AccReceiptsDetail.Display, AccReceiptsDetail.ReceiptsId,AccReceiptsDetail.AccountId,  
					AccReceiptsDetail.Narration, AccReceiptsDetail.Amount, AccHeads_1.AccountCode, 
					AccHeads_1.AccountName,AccReceiptsHeader.VoucherNo,AccReceiptsHeader.TransDate
		Union all			
		SELECT    AccReceiptsHeader.TransDate as TrDate, AccReceiptsHeader.VoucherNo as VoucherNo,  AccReceiptsDetail.AccountId, AccHeads_1.AccountName 
		AS GroupAcc, null, AccHeads_1.AccountCode AS OppAccCode, 
		CASE WHEN AccReceiptsDetail.Display = 0 THEN
                        ''By '' +  (SELECT     TOP (1)   AccountHead.AccountName
									FROM         AccReceiptsDetail AS AccReceiptsDetail_2 INNER JOIN
									AccountHead ON AccReceiptsDetail_2.AccountId = AccountHead.AccountHeadId
									WHERE     (AccReceiptsDetail_2.ReceiptsId = AccReceiptsDetail.ReceiptsId) AND (AccReceiptsDetail_2.Display = 1)
									ORDER BY CASE WHEN AccReceiptsDetail_2.amount < 0 THEN (- 1) * AccReceiptsDetail_2.amount ELSE 
									AccReceiptsDetail_2.amount END DESC) 
        ELSE
                        ''To '' +  (SELECT     TOP (1)   AccountHead.AccountName
									FROM         AccReceiptsDetail AS AccReceiptsDetail_2 INNER JOIN
									AccountHead ON AccReceiptsDetail_2.AccountId = AccountHead.AccountHeadId
									WHERE     (AccReceiptsDetail_2.ReceiptsId = AccReceiptsDetail.ReceiptsId) AND (AccReceiptsDetail_2.Display = 0)
									ORDER BY CASE WHEN AccReceiptsDetail_2.amount < 0 THEN (- 1) * AccReceiptsDetail_2.amount ELSE 
									AccReceiptsDetail_2.amount END DESC) 
        END    AS OppAccName, AccReceiptsDetail.Narration, 
		--sum( AccReceiptsDetail.Amount) AS DrAmount, 
		--0 AS CrAmount 
		case when sum( AccReceiptsDetail.Amount) >0 then sum( AccReceiptsDetail.Amount) else 0  end AS DrAmount, 
		case when sum( AccReceiptsDetail.Amount) <0 then (-1)* sum( AccReceiptsDetail.Amount) else 0  end AS CrAmount 
		FROM        AccReceiptsDetail INNER JOIN
					AccReceiptsHeader ON AccReceiptsDetail.ReceiptsId = AccReceiptsHeader.Id INNER JOIN
					AccountHead AS AccHeads_1 ON AccReceiptsDetail.AccountId = AccHeads_1.AccountHeadId
		WHERE      
					convert(datetime,AccReceiptsHeader.TransDate,102)
					between CONVERT(DATETIME, ''' + @FromDate + ''', 102) AND 
					dateadd(day,0, CONVERT(DATETIME, ''' + @ToDate + ''', 102))
					and (AccReceiptsHeader.TransType = 2)  
					and AccReceiptsDetail.AccountId in ('+ @AccountsIds +') and AccReceiptsHeader.Cancelled=0
		GROUP BY  AccReceiptsDetail.Display, AccReceiptsDetail.ReceiptsId,AccReceiptsDetail.AccountId,  
					AccReceiptsDetail.Narration, AccReceiptsDetail.Amount, AccHeads_1.AccountCode, 
					AccHeads_1.AccountName,AccReceiptsHeader.VoucherNo,AccReceiptsHeader.TransDate	
					
		'
							  
		exec sp_executesql @sql	
		
		--select * from #MyTableRec
		
		print 'Bank withdrawal'
		
		Set @sql='
		insert into #MyTableBankWith  (TrDate,VoucherNo,AccountId,GroupAcc,OppAccountId,OppAccCode,OppAccName,Narration,DrAmount,CrAmount) 
		SELECT   AccBankDepositHeader.TransDate as TrDate, AccBankDepositHeader.VoucherNo as VoucherNo,  AccBankDepositDetail.AccountId, 
				 AccHeads_1.AccountName AS GroupAcc, 0 as OppAccountId, '''' AS OppAccCode, 
				 ''By ''  AS OppAccName, AccBankDepositDetail.Narration, 0  AS DrAmount,  (-1)*AccBankDepositDetail.Amount AS CrAmount
		FROM     AccBankDepositDetail INNER JOIN
				 AccBankDepositHeader ON AccBankDepositDetail.BankDepositId = AccBankDepositHeader.Id 
				 --LEFT OUTER JOIN
				 --AccountHead ON AccBankDepositDetail.OppAccountId = AccountHead.AccountHeadId 
				 INNER JOIN
				 AccountHead AS AccHeads_1 ON AccBankDepositDetail.AccountId = AccHeads_1.AccountHeadId
		WHERE      
							  convert(datetime,AccBankDepositHeader.TransDate,102)
							  between CONVERT(DATETIME,  ''' + @FromDate + ''', 102) AND 
							  dateadd(day,0, CONVERT(DATETIME,  ''' + @ToDate + ''', 102))
	                          and AccBankDepositHeader.Cancelled=0 
							  AND (AccBankDepositHeader.TransType = 1)  and AccBankDepositDetail.AccountId in ('+ @AccountsIds +')
		GROUP BY AccBankDepositDetail.AccountId,   AccBankDepositDetail.Narration, AccBankDepositDetail.Amount,-- AccountHead.AccountCode, 
							  --AccountHead.AccountName, 
							  AccHeads_1.AccountName, AccBankDepositHeader.VoucherNo , AccBankDepositHeader.TransDate
							  
							  
		union all						  
	
		SELECT  AccBankDepositHeader.TransDate as TrDate, AccBankDepositHeader.VoucherNo as VoucherNo,   AccBankDepositDetail.AccountId, 
		AccHeads_1.AccountName AS GroupAcc,0 , '''' AS OppAccCode, 
							  ''To ''   AS OppAccName, AccBankDepositDetail.Narration, AccBankDepositDetail.Amount AS DrAmount,
							   0 AS CrAmount
		FROM         AccBankDepositDetail INNER JOIN
							  AccBankDepositHeader ON AccBankDepositDetail.BankDepositId = AccBankDepositHeader.Id 
							  --LEFT OUTER JOIN
							  --AccountHead ON AccBankDepositDetail.OppAccountId = AccountHead.AccountHeadId
							   INNER JOIN
							  AccountHead AS AccHeads_1 ON AccBankDepositDetail.AccountId = AccHeads_1.AccountHeadId
		WHERE     
		convert(datetime,AccBankDepositHeader.TransDate,102)
							  between CONVERT(DATETIME, ''' + @FromDate + ''', 102) AND 
							  dateadd(day,0, CONVERT(DATETIME,  ''' + @ToDate + ''', 102)) 
							  and AccBankDepositHeader.Cancelled=0
							  AND (AccBankDepositHeader.TransType = 2)  and AccBankDepositDetail.AccountId in ('+ @AccountsIds +')
		GROUP BY AccBankDepositDetail.AccountId,  AccBankDepositDetail.Narration, AccBankDepositDetail.Amount
		--, 
		--AccountHead.AccountCode, AccountHead.AccountName
							  , AccHeads_1.AccountName, AccBankDepositHeader.VoucherNo , AccBankDepositHeader.TransDate
							  
		'
		
		exec sp_executesql @sql	
		
		--select * from #MyTableBankWith			
		
		print 'Journal'
		Set @sql='
		insert into #MyTableJournal  (TrDate,VoucherNo,AccountId,GroupAcc,OppAccountId,OppAccCode,OppAccName,Narration,DrAmount,CrAmount) 			
			SELECT   AccJournalHeader.TransDate as TrDate, AccJournalHeader.VoucherNo as VoucherNo,  AccJournalDetail.AccountId, AccountHead.AccountName, 
				CASE WHEN AccJournalDetail.DrAmount > 0 THEN
								  (SELECT     TOP (1) AccountId
									FROM          AccJournalDetail AS accjrn
									WHERE      (JournalId = AccJournalDetail.JournalId)
									ORDER BY CrAmount DESC) ELSE
								  (SELECT     TOP (1) AccountId
									FROM          AccJournalDetail AS accjrn
									WHERE      (JournalId = AccJournalDetail.JournalId)
									ORDER BY drAmount DESC) END AS OppAccId, '''' AS code,
		                            
								  CASE WHEN AccJournalDetail.DrAmount > 0 then ''To '' else ''By '' end  + (SELECT     AccountName
									FROM          AccountHead AS hd
									WHERE      (AccountHead.AccountHeadId = CASE WHEN AccJournalDetail.DrAmount > 0 THEN
															   (SELECT     TOP (1) AccountId
																 FROM          AccJournalDetail AS accjrn
																 WHERE      (JournalId = AccJournalDetail.JournalId)
																 ORDER BY (-1)*CrAmount DESC) ELSE
															   (SELECT     TOP (1) AccountId
																 FROM          AccJournalDetail AS accjrn
																 WHERE      (JournalId = AccJournalDetail.JournalId)
																 ORDER BY drAmount DESC) END)) AS Account
																 , AccJournalDetail.Narration, AccJournalDetail.DrAmount,(-1)* AccJournalDetail.CrAmount
		FROM         AccJournalDetail INNER JOIN
							  AccJournalHeader ON AccJournalDetail.JournalId = AccJournalHeader.Id INNER JOIN
							  AccountHead ON AccJournalDetail.AccountId = AccountHead.AccountHeadId
		WHERE     
		AccJournalHeader.Cancelled=0 and 
		convert(datetime,AccJournalHeader.TransDate,102)
							  between CONVERT(DATETIME, ''' + @FromDate + ''', 102) AND 
							  dateadd(day,0, CONVERT(DATETIME, ''' + @ToDate + ''', 102))  and AccJournalDetail.AccountId in ('+ @AccountsIds +')	'		 
							  
		exec sp_executesql @sql	
		
		--select * from #MyTableJournal
		
		select * from #MyTableOp
		union all
		select * from #MyTableRec	
		union all
		select * from #MyTablePay
		union all
		select * from #MyTableBankDep
		union all
		select * from #MyTableBankWith
		union all
		select * from #MyTableJournal	
	End	
	 	 	  
	else if(@OperationId =1)
	begin
	   
		set @FromDate='2015-04-01 00:00:00'
		
		
		print 'Opening'
	  Set @sql='
					insert into #MyTableOp  (TrDate,VoucherNo,AccountId,GroupAcc,OppAccountId,OppAccCode,OppAccName,Narration,DrAmount,CrAmount) 	 	
					select TrDate,VoucherNo,AccountId,GroupAcc,OppAccountId,OppAccCode,OppAccName,Narration,sum(DrAmount),sum(CrAmount) from (
						
						 
							
							SELECT    '''' as TrDate ,'''' as VoucherNo, OpeningBalance.AccountHeadId as AccountId, AccountHead.AccountName as GroupAcc, 0 as OppAccountId, '''' AS OppAccCode, 
												  ''Opening Balance'' AS OppAccName, '''' as Narration,   OpeningBalance.DebitOpeningBalance as DrAmount, 
										  OpeningBalance.CreditOpeningBalance as CrAmount
							FROM         OpeningBalance INNER JOIN
												  AccountHead ON OpeningBalance.AccountHeadId = AccountHead.AccountHeadId		
												  where OpeningBalance.FinancialYearId=1		  
							  
							)
		
		as OpBal
		group by      AccountId           ,GroupAcc ,OppAccountId,OppAccCode,OppAccName,Narration,VoucherNo ,TrDate                                    
		'
		exec sp_executesql @sql	
		
		
		print 'Receipt & payments'
		Set @sql='
		insert into #MyTableRec  (TrDate,VoucherNo,AccountId,GroupAcc,OppAccountId,OppAccCode,OppAccName,Narration,DrAmount,CrAmount) 
		SELECT    AccReceiptsHeader.TransDate as TrDate, AccReceiptsHeader.VoucherNo as VoucherNo,  AccReceiptsDetail.AccountId, AccHeads_1.AccountName 
		AS GroupAcc, 0 as OppAccountId, AccHeads_1.AccountCode AS OppAccCode, 
		CASE WHEN AccReceiptsDetail.Display = 0 THEN
                        ''To '' +  (SELECT     TOP (1)   AccountHead.AccountName
									FROM         AccReceiptsDetail AS AccReceiptsDetail_2 INNER JOIN
									AccountHead ON AccReceiptsDetail_2.AccountId = AccountHead.AccountHeadId
									WHERE     (AccReceiptsDetail_2.ReceiptsId = AccReceiptsDetail.ReceiptsId) AND (AccReceiptsDetail_2.Display = 1)
									ORDER BY CASE WHEN AccReceiptsDetail_2.amount < 0 THEN (- 1) * AccReceiptsDetail_2.amount ELSE 
									AccReceiptsDetail_2.amount END DESC) 
        ELSE
                        ''By '' +  (SELECT     TOP (1)   AccountHead.AccountName
									FROM         AccReceiptsDetail AS AccReceiptsDetail_2 INNER JOIN
									AccountHead ON AccReceiptsDetail_2.AccountId = AccountHead.AccountHeadId
									WHERE     (AccReceiptsDetail_2.ReceiptsId = AccReceiptsDetail.ReceiptsId) AND (AccReceiptsDetail_2.Display = 0)
									ORDER BY CASE WHEN AccReceiptsDetail_2.amount < 0 THEN (- 1) * AccReceiptsDetail_2.amount ELSE 
									AccReceiptsDetail_2.amount END DESC) 
        END    AS OppAccName, AccReceiptsDetail.Narration, 
		case when sum( AccReceiptsDetail.Amount) >0 then sum( AccReceiptsDetail.Amount) else 0  end AS DrAmount, 
		case when sum( AccReceiptsDetail.Amount) <0 then (-1)* sum( AccReceiptsDetail.Amount) else 0  end AS CrAmount 
		FROM        AccReceiptsDetail INNER JOIN
					AccReceiptsHeader ON AccReceiptsDetail.ReceiptsId = AccReceiptsHeader.Id INNER JOIN
					AccountHead AS AccHeads_1 ON AccReceiptsDetail.AccountId = AccHeads_1.AccountHeadId
		WHERE      
					convert(datetime,AccReceiptsHeader.TransDate,102)
					between CONVERT(DATETIME, ''' + @FromDate + ''', 102) AND 
					dateadd(day,0, CONVERT(DATETIME, ''' + @ToDate + ''', 102))
					And AccReceiptsHeader.Cancelled=0   
		GROUP BY  AccReceiptsDetail.Display, AccReceiptsDetail.ReceiptsId,AccReceiptsDetail.AccountId, 
					AccReceiptsDetail.Narration, AccReceiptsDetail.Amount, AccHeads_1.AccountCode, 
					AccHeads_1.AccountName,AccReceiptsHeader.VoucherNo,AccReceiptsHeader.TransDate
				
					
					
					'
		exec sp_executesql @sql	
		
		
		--select * from #MyTableRec
		
		print 'Bank withdrawal'
		Set @sql='
		insert into #MyTableBankWith  (TrDate,VoucherNo,AccountId,GroupAcc,OppAccountId,OppAccCode,OppAccName,Narration,DrAmount,CrAmount) 
		SELECT   AccBankDepositHeader.TransDate as TrDate, AccBankDepositHeader.VoucherNo as VoucherNo,  AccBankDepositDetail.AccountId, 
				 AccHeads_1.AccountName AS GroupAcc, 0 as OppAccountId, 0 AS OppAccCode, 
				 ''By ''   AS OppAccName, AccBankDepositDetail.Narration, 0  AS DrAmount, (-1)* AccBankDepositDetail.Amount AS CrAmount
		FROM     AccBankDepositDetail INNER JOIN
				 AccBankDepositHeader ON AccBankDepositDetail.BankDepositId = AccBankDepositHeader.Id 
				 --LEFT OUTER JOIN
				 --AccountHead ON AccBankDepositDetail.OppAccountId = AccountHead.AccountHeadId 
				 
				 INNER JOIN
				 AccountHead AS AccHeads_1 ON AccBankDepositDetail.AccountId = AccHeads_1.AccountHeadId
		WHERE      
							  convert(datetime,AccBankDepositHeader.TransDate,102)
							  between CONVERT(DATETIME,  ''' + @FromDate + ''', 102) AND 
							  dateadd(day,0, CONVERT(DATETIME,  ''' + @ToDate + ''', 102))
	                          And AccBankDepositHeader.Cancelled=0 
							  AND (AccBankDepositHeader.TransType = 1)   
		GROUP BY AccBankDepositDetail.AccountId,   AccBankDepositDetail.Narration, AccBankDepositDetail.Amount, 
							    AccHeads_1.AccountName, AccBankDepositHeader.VoucherNo , AccBankDepositHeader.TransDate
							    
							    
		union all						  
	
		SELECT  AccBankDepositHeader.TransDate as TrDate, AccBankDepositHeader.VoucherNo as VoucherNo,   AccBankDepositDetail.AccountId, 
		AccHeads_1.AccountName AS GroupAcc,0 , 0 AS OppAccCode, 
							  ''To ''   AS OppAccName, AccBankDepositDetail.Narration, AccBankDepositDetail.Amount AS DrAmount,
							   0 AS CrAmount
		FROM         AccBankDepositDetail INNER JOIN
							  AccBankDepositHeader ON AccBankDepositDetail.BankDepositId = AccBankDepositHeader.Id 
							  --LEFT OUTER JOIN
							  --AccountHead ON AccBankDepositDetail.OppAccountId = AccountHead.AccountHeadId 
							  INNER JOIN
							  AccountHead AS AccHeads_1 ON AccBankDepositDetail.AccountId = AccHeads_1.AccountHeadId
		WHERE     
		convert(datetime,AccBankDepositHeader.TransDate,102)
							  between CONVERT(DATETIME, ''' + @FromDate + ''', 102) AND 
							  dateadd(day,0, CONVERT(DATETIME,  ''' + @ToDate + ''', 102)) 
							  And AccBankDepositHeader.Cancelled=0 
							  AND (AccBankDepositHeader.TransType = 2)   
		GROUP BY AccBankDepositDetail.AccountId,   AccBankDepositDetail.Narration, AccBankDepositDetail.Amount, 
							    AccHeads_1.AccountName, AccBankDepositHeader.VoucherNo , AccBankDepositHeader.TransDate
							  
		'
		exec sp_executesql @sql	
		
		--select * from #MyTableBankWith			
		
		print 'Journal'
		Set @sql='
		insert into #MyTableJournal  (TrDate,VoucherNo,AccountId,GroupAcc,OppAccountId,OppAccCode,OppAccName,Narration,DrAmount,CrAmount) 			
			SELECT   AccJournalHeader.TransDate as TrDate, AccJournalHeader.VoucherNo as VoucherNo,  AccJournalDetail.AccountId, AccountHead.AccountName, 
				CASE WHEN AccJournalDetail.DrAmount > 0 THEN
								  (SELECT     TOP (1) AccountId
									FROM          AccJournalDetail AS accjrn
									WHERE      (JournalId = AccJournalDetail.JournalId)
									ORDER BY CrAmount DESC) ELSE
								  (SELECT     TOP (1) AccountId
									FROM          AccJournalDetail AS accjrn
									WHERE      (JournalId = AccJournalDetail.JournalId)
									ORDER BY drAmount DESC) END AS OppAccId, '''' AS code,
		                            
								  CASE WHEN AccJournalDetail.DrAmount > 0 then ''To '' else ''By '' end  + (SELECT     AccountName
									FROM          AccountHead AS hd
									WHERE      (AccountheadId = CASE WHEN AccJournalDetail.DrAmount > 0 THEN
															   (SELECT     TOP (1) AccountId
																 FROM          AccJournalDetail AS accjrn
																 WHERE      (JournalId = AccJournalDetail.JournalId)
																 ORDER BY (-1)*CrAmount DESC) ELSE
															   (SELECT     TOP (1) AccountId
																 FROM          AccJournalDetail AS accjrn
																 WHERE      (JournalId = AccJournalDetail.JournalId)
																 ORDER BY drAmount DESC) END)) AS Account
																 , AccJournalDetail.Narration, AccJournalDetail.DrAmount,(-1)* AccJournalDetail.CrAmount
		FROM         AccJournalDetail INNER JOIN
							  AccJournalHeader ON AccJournalDetail.JournalId = AccJournalHeader.Id INNER JOIN
							  AccountHead ON AccJournalDetail.AccountId = AccountHead.AccountHeadId
		WHERE     
		convert(datetime,AccJournalHeader.TransDate,102)
							  between CONVERT(DATETIME, ''' + @FromDate + ''', 102) AND 
							  dateadd(day,0, CONVERT(DATETIME, ''' + @ToDate + ''', 102)) And AccJournalHeader.Cancelled=0  '-- and AccJournalDetail.AccountId in ('+ @AccountsIds +')	'		 
							  
		exec sp_executesql @sql	
		
		--select * from #MyTableJournal
		--select * from (
		select * from #MyTableOp
		union all
		select * from #MyTableRec	
		union all
		select * from #MyTablePay
		union all
		select * from #MyTableBankDep
		union all
		select * from #MyTableBankWith
		union all
		select * from #MyTableJournal	--) as der order by accountid desc
	End
  
  GO
  
  PRINT 'sp_Acc_rptLedger is created successfully'  
  GO
