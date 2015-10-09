
----------------------------------------------------------
-- Author: Digeesh
-- Date: 11/Nov/14
-- Purpose: procedure usp_RptEmployeeShifts to get all the employee shifts
-- Returns the list of all users
----------------------------------------------------------
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE type = 'P'
			AND NAME = 'usp_GetBankDepositWithdrawal'
		)
	DROP PROCEDURE usp_GetBankDepositWithdrawal
GO

CREATE PROCEDURE [dbo].[usp_GetBankDepositWithdrawal] @vFilterValue varchar(50)	  
  ,@vCompanyId bigint  
AS
  
  /*		public int FromModule { get; set; } //0 for "From Accounts", 1 for "From Posting"
        public TransactionType Transaction { get; set; } //0 for deposit 1 for withdrawal, 2 for "Loan Entry", 3 for "Loan repayment"
        public long TransId { get; set; }// Transaction id 
        public int Cancelled { get; set; }//0 or 1
		TransactionType { CashReceipt = 0, CashPayment = 1, LoanEntry = 2, LoanRepayment = 3, DepositEntry = 4, DepositWithdrawal = 5 }		*/
    
Select ach.Id, ach.VoucherNo, cast(ach.VoucherTotal as decimal(18,2)) as VoucherTotal, case isnull(ach.FromModule,0) when 0 then 'From Accounts' when 1 then 'From Posting' end as FromModule
,case ach.TransType when 0 then 'Cash Receipt' when 1 then 'Cash Payment' end as TransType
,ach.[Transaction] as TransactionTypeId, case ach.[Transaction] when 0 then 'Cash Receipt' when 1 then 'Cash Payment' when 2 then 'Loan Entry' when 3 then 'Loan Repayment' when 4 then 'Deposit Entry' when 5 then 'Deposit Withdrawal' end as [Transaction], ach.TransDate, ach.TransId
,acd.Narration, cast(acd.Amount as decimal(18,2)) as Amount, case acd.Instrument when 0 then 'Cash' when 1 then 'Cheque' when 2 then 'Credit Card' end as Instrument, acd.InstrumentNo, isnull(acd.InstrumentDate, GetDate()) as InstrumentDate
,fp.PeriodName, ab.BookName, ah.AccountName, ah.AccountCode, at.AccountType
,asch.ScheduleName
from AccBankDepositHeader ach
inner join AccBankDepositDetail acd on acd.BankDepositId = ach.Id
left join FinancialPeriod fp on fp.FinancialPeriodId = ach.FinYear
left join AccountBook ab on ab.AccountBookId = ach.BookId
left join AccountHead ah on ah.AccountHeadId = acd.AccountId
left join AccountType at on at.AccountTypeId = ah.AccountTypeId
left join AccountSchedule asch on asch.ScheduleId = ah.ScheduleId
where isnull(ach.IsDeleted, 0) = 0
and isnull(acd.IsDeleted,0) = 0
and isnull(ach.Cancelled,0) = 0
and acd.Display = 1
GO

PRINT 'usp_RptSummary is created successfully..'
GO
